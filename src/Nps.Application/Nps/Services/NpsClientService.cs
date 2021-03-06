﻿using Microsoft.Extensions.Logging;
using Nps.Application.Nps.Dtos;
using Nps.Application.NpsApi;
using Nps.Application.NpsApi.Dtos;
using Nps.Core.Aop.Attributes;
using Nps.Core.Caching;
using Nps.Core.Data;
using Nps.Infrastructure;
using Nps.Infrastructure.Exceptions;
using Nps.Infrastructure.Extensions;
using Nps.Infrastructure.Helpers;
using Nps.Infrastructure.IdGenerators;
using Nps.Core.Repositories;
using Nps.Core.Services;
using Nps.Data.Entities;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nps.Application.Nps.Services
{
    /// <summary>
    /// Nps客户端服务
    /// </summary>
    public class NpsClientService : DomainService, INpsClientService
    {
        private readonly INpsApi _npsApi;

        private readonly ICaching _caching;

        private readonly IGuidGenerator _guidGenerator;

        private readonly ILogger<NpsClientService> _logger;

        private readonly IFreeSqlRepository<NpsServer> _npsServerRepository;

        private readonly IFreeSqlRepository<NpsAppSecret> _npsAppSecretRepository;

        private readonly IFreeSqlRepository<NpsClient> _npsClientRepository;

        private readonly IFreeSqlRepository<NpsChannel> _npsChannelRepository;

        private readonly string _auth_Key = "jyioself2020";

        private readonly int _minServerPort = 10010;

        private readonly string _protocolType = "tcp";

        private readonly string _npsApi_Auth_CacheKey = "Cache_Nps_RemoteApi_Auth_Key";

        /// <summary>
        /// 初始化一个<see cref="NpsClientService"/>实例
        /// </summary>
        /// <param name="npsApi">Nps服务器Api</param>
        /// <param name="caching">缓存对象</param>
        /// <param name="guidGenerator">有序GUID生成器</param>
        /// <param name="logger">日志对象</param>
        /// <param name="npsServerRepository">Nps服务器仓储</param>
        /// <param name="npsAppSecretRepository">Nps应用密钥仓储</param>
        /// <param name="npsClientRepository">Nps客户端仓储</param>
        /// <param name="npsChannelRepository">Nps隧道仓储</param>
        public NpsClientService(
            INpsApi npsApi,
            ICaching caching,
            IGuidGenerator guidGenerator,
            ILogger<NpsClientService> logger,
            IFreeSqlRepository<NpsServer> npsServerRepository,
            IFreeSqlRepository<NpsAppSecret> npsAppSecretRepository,
            IFreeSqlRepository<NpsClient> npsClientRepository,
            IFreeSqlRepository<NpsChannel> npsChannelRepository)
        {
            _npsApi = npsApi;
            _caching = caching;
            _guidGenerator = guidGenerator;
            _logger = logger;
            _npsServerRepository = npsServerRepository;
            _npsAppSecretRepository = npsAppSecretRepository;
            _npsClientRepository = npsClientRepository;
            _npsChannelRepository = npsChannelRepository;
        }

        #region 设备端口查询服务

        /// <summary>
        /// 分页查询所有已开通服务列表
        /// </summary>
        /// <param name="input">查询服务参数</param>
        /// <returns>分页返回查询结果</returns>
        public async Task<List<NpsClientOpenedOutput>> SearchAsync(PagingInput<NpsClientSearchInput> input)
        {
            var outputs = new List<NpsClientOpenedOutput>();

            var npsAppSecrets = await _npsAppSecretRepository
                .Where(x => x.CreateUserId == CurrentUser.UserId)
                .WhereIf(input.Filter?.DeviceUniqueId.IsNotNullOrEmpty() ?? false, x => x.DeviceUniqueId == input.Filter.DeviceUniqueId)
                .WhereCascade(x => x.IsDeleted == false)
                .Include(x => x.NpsServer)
                .Include(x => x.NpsClient)
                .ToListAsync();
            if (npsAppSecrets.Count == 0)
            {
                return outputs;
            }

            var npsClientIds = npsAppSecrets.Select(x => x.NpsClient?.Id ?? 0).ToList();

            var npsChannels = await _npsChannelRepository
                .Where(x => x.IsDeleted == false)
                .Where(x => npsClientIds.Contains(x.NpsClientId))
                .WhereIf(input.Filter?.SearchPorts.IsNotNull() ?? false, x => input.Filter.SearchPorts.Contains(x.DeviceAddress))
                .ToListAsync();
            if (npsChannels.Count == 0)
            {
                return outputs;
            }

            npsAppSecrets.ForEach(npsAppSecret =>
            {
                if (npsAppSecret.NpsClient != null)
                    npsAppSecret.NpsClient.NpsChannels = npsChannels.Where(x => x.NpsClientId == npsAppSecret.NpsClient?.Id).ToList();
            });

            npsAppSecrets.ForEach(npsAppSecret =>
            {
                var npsClientOpenedOutput = Mapper.Map<NpsClientOpenedOutput>(npsAppSecret);
                outputs.Add(npsClientOpenedOutput);
            });
            return outputs;
        }

        #endregion

        #region 设备端口开通服务

        /// <summary>
        /// 设备开通服务
        /// </summary>
        /// <param name="input">设备开通服务输入参数</param>
        /// <returns>返回执行结果</returns>
        [Transactional]
        public async Task<NpsClientOpenedOutput> OpenAsync(NpsClientOpenInput input)
        {
            _logger.LogInformation($"开始准备开通设备端口，{input.ToJson()}");

            _logger.LogInformation($"检测用户是否存在");
            if (!CurrentUser?.UserId.HasValue ?? false || CurrentUser?.UserId.Value == 0)
            {
                throw new NpsException("用户不存在，请先登录", StatusCode.AuthenticationFailed);
            }

            MiniProfiler.Current.Step("GetNpsAppSecretAsync");
            var npsAppSecret = await GetNpsAppSecretAsync(input.DeviceUniqueId);
            MiniProfiler.Current.Step("CreateNpsAppSecretIfCheckNullAsync");
            npsAppSecret = await CreateNpsAppSecretIfCheckNullAsync(input.DeviceUniqueId, npsAppSecret);
            MiniProfiler.Current.Step("CreateNpsClientIfCheckNullAsync");
            npsAppSecret = await CreateNpsClientIfCheckNullAsync(input, npsAppSecret);
            MiniProfiler.Current.Step("UpdateNpsClientOrNpsServerIfCheckNotNullAsync");
            npsAppSecret = await UpdateNpsClientOrNpsServerIfCheckNotNullAsync(npsAppSecret);
            MiniProfiler.Current.Step("CreateNpsChannelIfCheckNullAsync");
            npsAppSecret = await CreateNpsChannelIfCheckNullAsync(input, npsAppSecret);
            MiniProfiler.Current.Step("UpdateNpsChannelsIfCheckNotNullAsync");
            npsAppSecret = await UpdateNpsChannelsIfCheckNotNullAsync(npsAppSecret);

            _logger.LogInformation($"已成功开通设备端口，{input.ToJson()}");

            var npsClientOpenedOutput = Mapper.Map<NpsClientOpenedOutput>(npsAppSecret);
            return npsClientOpenedOutput;
        }

        /// <summary>
        /// 根据设备唯一识别编码，查询Nps应用密钥、Nps服务器、Nps客户端、Nps隧道
        /// </summary>
        /// <param name="deviceUniqueId">设备唯一识别编码</param>
        /// <returns>返回NpsAppSecret</returns>
        private async Task<NpsAppSecret> GetNpsAppSecretAsync(string deviceUniqueId)
        {
            _logger.LogInformation($"根据设备唯一识别编码，查询Nps应用密钥、Nps服务器、Nps客户端、Nps隧道，设备唯一识别编码：{deviceUniqueId}");

            return await _npsAppSecretRepository
                .Where(x => x.DeviceUniqueId == deviceUniqueId)
                .Where(x => x.CreateUserId == CurrentUser.UserId)
                .WhereCascade(x => x.IsDeleted == false)
                .Include(x => x.NpsServer)
                .Include(x => x.NpsClient)
                .IncludeMany(x => x.NpsClient.NpsChannels)
                .ToOneAsync();
        }

        /// <summary>
        /// 检测设备是否已创建过唯一识别密钥，如果不存在，则创建
        /// </summary>
        /// <param name="deviceUniqueId">设备唯一识别编码</param>
        /// <param name="npsAppSecret">NpsAppSecret</param>
        /// <returns>返回NpsAppSecret</returns>
        private async Task<NpsAppSecret> CreateNpsAppSecretIfCheckNullAsync(string deviceUniqueId, NpsAppSecret npsAppSecret)
        {
            _logger.LogInformation($"检测设备是否已创建过唯一识别密钥，设备唯一识别编码：{deviceUniqueId}");
            if (npsAppSecret == null)
            {
                npsAppSecret = await _npsAppSecretRepository.InsertAsync(new NpsAppSecret
                {
                    DeviceUniqueId = deviceUniqueId,
                    AppSecret = _guidGenerator.Create().ToString("N")
                });
            }

            return npsAppSecret;
        }

        /// <summary>
        /// 检测设备是否已创建过客户端，如果不存在，则创建
        /// </summary>
        /// <param name="input">开通客户端输入参数</param>
        /// <param name="npsAppSecret">NpsAppSecret</param>
        /// <returns>返回NpsAppSecret</returns>
        private async Task<NpsAppSecret> CreateNpsClientIfCheckNullAsync(NpsClientOpenInput input, NpsAppSecret npsAppSecret)
        {
            Check.NotNull(npsAppSecret, nameof(npsAppSecret));

            _logger.LogInformation($"检测设备是否已创建过客户端，设备唯一识别编码：{npsAppSecret.DeviceUniqueId}");
            if (npsAppSecret.NpsClient == null)
            {
                //调用远程Api执行添加客户端
                var baseAuthInput = await BeforeRequestNpsApiAsync();
                var remoteApiResult = await _npsApi.AddClientAsync(new ClientAddInput
                {
                    AuthKey = baseAuthInput.AuthKey,
                    Timestamp = baseAuthInput.Timestamp,
                    AppSecret = npsAppSecret.AppSecret,
                    IsConfigConnectAllow = input.IsConfigConnectAllow.ToInt32OrDefault(1),
                    IsCompress = input.IsCompress.ToInt32OrDefault(0),
                    IsCrypt = input.IsCrypt.ToInt32OrDefault(1),
                    Remark = input.Remark
                });
                if (remoteApiResult.Status == 1)
                {//远程添加成功后，将数据写入本地Nps客户端表中
                    npsAppSecret.NpsClient = await _npsClientRepository.InsertAsync(new NpsClient
                    {
                        Id = npsAppSecret.Id,
                        IsConfigConnectAllow = input.IsConfigConnectAllow,
                        IsCompress = input.IsCompress,
                        IsCrypt = input.IsCrypt,
                        Remark = input.Remark
                    });
                }
                else
                {
                    throw new NpsException(remoteApiResult.Message, StatusCode.Error);
                }
            }

            return npsAppSecret;
        }

        /// <summary>
        /// 检查是否更新NpsAppSecret、NpsServer、NpsClient
        /// </summary>
        /// <param name="npsAppSecret">NpsAppSecret</param>
        /// <returns>返回NpsAppSecret</returns>
        private async Task<NpsAppSecret> UpdateNpsClientOrNpsServerIfCheckNotNullAsync(NpsAppSecret npsAppSecret)
        {
            Check.NotNull(npsAppSecret, nameof(npsAppSecret));
            Check.NotNull(npsAppSecret.NpsClient, nameof(npsAppSecret.NpsClient));

            _logger.LogInformation($"检查是否更新NpsAppSecret、NpsServer、NpsClient，设备唯一识别编码：{npsAppSecret.DeviceUniqueId}");
            if (npsAppSecret.NpsClient.RemoteClientId == 0 || npsAppSecret.NpsServer == null || npsAppSecret.NpsServerId == 0)
            {
                var clientListOutput = await GetRemoteClientOutputAsync(npsAppSecret.AppSecret);
                if (clientListOutput == null || clientListOutput.Datas == null || clientListOutput.Datas.Count == 0)
                {
                    return npsAppSecret;
                }

                //若本地数据库Nps客户端表中无远程Nps客户端Id，则将远程客户端信息回写
                if (npsAppSecret.NpsClient.RemoteClientId == 0)
                {
                    var remoteNpsClient = clientListOutput.Datas[0];

                    _npsClientRepository.Attach(npsAppSecret.NpsClient);
                    npsAppSecret.NpsClient.RemoteClientId = remoteNpsClient.Id;
                    npsAppSecret.NpsClient.Status = remoteNpsClient.Status;
                    npsAppSecret.NpsClient.IsConnect = remoteNpsClient.IsConnect;
                    npsAppSecret.NpsClient.LastConnectAddress = remoteNpsClient.LastConnectAddress;
                    await _npsClientRepository.UpdateAsync(npsAppSecret.NpsClient);
                }

                //若该设备无对应的服务器信息，则将服务器写入本地数据库
                if (npsAppSecret.NpsServer == null)
                {
                    //先根据IP地址查询服务器是否存在
                    npsAppSecret.NpsServer = await _npsServerRepository.Where(x => x.ServerIPAddress == clientListOutput.ServerIPAddress).ToOneAsync();
                    if (npsAppSecret.NpsServer == null)
                    {
                        npsAppSecret.NpsServer = await _npsServerRepository.InsertAsync(new NpsServer
                        {
                            ServerIPAddress = clientListOutput.ServerIPAddress,
                            ClientConnectPort = clientListOutput.ClientConnectPort.ToInt32OrDefault(0),
                            ProtocolType = _protocolType
                        });
                    }
                }

                //若该设备无对应的服务器信息
                if (npsAppSecret.NpsServerId == 0)
                {
                    if (npsAppSecret?.NpsServer?.Id > 0)
                    {
                        //将服务器信息与设备应用密钥关联
                        _npsAppSecretRepository.Attach(npsAppSecret);
                        npsAppSecret.NpsServerId = npsAppSecret.NpsServer.Id;
                        await _npsAppSecretRepository.UpdateAsync(npsAppSecret);
                    }
                }
            }

            return npsAppSecret;
        }

        /// <summary>
        /// 根据设备应用唯一秘钥，从服务器查询应用密钥对应客户端信息
        /// </summary>
        /// <param name="deviceAppSecret">设备应用唯一秘钥</param>
        /// <returns>返回服务器客户端信息</returns>
        private async Task<ClientListOutput> GetRemoteClientOutputAsync(string deviceAppSecret)
        {
            MiniProfiler.Current.Step("GetRemoteClientOutputAsync");
            var baseAuthInput = await BeforeRequestNpsApiAsync();
            var searchApiResult = await _npsApi.ClientListAsync(new ClientListInput
            {
                AuthKey = baseAuthInput.AuthKey,
                Timestamp = baseAuthInput.Timestamp,
                KeyWords = deviceAppSecret,
                OrderType = "asc",
                Offset = "0",
                Limit = "1"
            });

            return searchApiResult ?? null;
        }

        /// <summary>
        /// 检查是否已创建服务器客户端隧道，如果不存在，则创建
        /// </summary>
        /// <param name="input">设备开通服务输入参数</param>
        /// <param name="npsAppSecret">NpsAppSecret</param>
        /// <returns>返回NpsAppSecret</returns>
        private async Task<NpsAppSecret> CreateNpsChannelIfCheckNullAsync(NpsClientOpenInput input, NpsAppSecret npsAppSecret)
        {
            Check.NotNull(npsAppSecret, nameof(npsAppSecret));
            Check.NotNull(npsAppSecret.NpsClient, nameof(npsAppSecret.NpsClient));

            _logger.LogInformation($"检查是否已创建服务器客户端隧道，设备唯一识别编码：{npsAppSecret.DeviceUniqueId}；开通端口列表：{input.OpenPorts.ToJson()}");
            if (npsAppSecret.NpsClient.NpsChannels == null) npsAppSecret.NpsClient.NpsChannels = new List<NpsChannel>();

            //查询已开通隧道列表
            var openedNpsChannels = npsAppSecret.NpsClient?.NpsChannels.Where(x => input.OpenPorts.Contains(x.DeviceAddress));
            //查询已开通隧道端口号列表
            var openedNpsChannelPorts = openedNpsChannels?.Select(x => x.DeviceAddress);
            //取需要开通端口号集合与已开通端口号集合差集，得到正在需要开通的端口列表
            var needOpenPorts = input.OpenPorts.Except(openedNpsChannelPorts).ToList();

            if (needOpenPorts.Count > 0)
            {
                var needOpenNpsChannels = new List<NpsChannel>();

                //取自定义服务器端口
                var nowMaxServerPort = await _npsChannelRepository.Select.MaxAsync(x => x.ServerPort);
                var startServerPort = nowMaxServerPort == 0 ? _minServerPort : nowMaxServerPort;

                var baseAuthInput = await BeforeRequestNpsApiAsync();
                for (int index = 0; index < needOpenPorts.Count; index++)
                {
                    //向远程服务器添加隧道
                    var remark = $"{input.Remark}_隧道_{startServerPort += 1}";
                    var remoteApiResult = await _npsApi.AddChannelAsync(new ChannelAddInput
                    {
                        AuthKey = baseAuthInput.AuthKey,
                        Timestamp = baseAuthInput.Timestamp,
                        Remark = remark,
                        ServerPort = startServerPort,
                        TargetAddress = needOpenPorts[index],
                        ClientId = npsAppSecret.NpsClient.RemoteClientId
                    });
                    if (remoteApiResult.Status == 1)
                    {//隧道添加成功后，写入本地数据库表
                        needOpenNpsChannels.Add(new NpsChannel
                        {
                            NpsClientId = npsAppSecret.NpsClient.Id,
                            ServerPort = startServerPort,
                            DeviceAddress = needOpenPorts[index],
                            Remark = remark
                        });
                    }
                    else
                    {
                        _logger.LogError($"开通客户端{npsAppSecret.DeviceUniqueId},端口{needOpenPorts[index]},{remoteApiResult.Message}");
                        continue;
                    }
                }

                if (needOpenNpsChannels.Count > 0)
                {
                    //将本次已开通隧道列表写入本地数据库
                    needOpenNpsChannels = await _npsChannelRepository.InsertAsync(needOpenNpsChannels);
                    //将本次已开通隧道列表附加至查询结果
                    npsAppSecret.NpsClient.NpsChannels.AddRange(needOpenNpsChannels);
                }
            }

            return npsAppSecret;
        }

        /// <summary>
        /// 检查是否更新NpsChannels
        /// </summary>
        /// <param name="npsAppSecret">NpsAppSecret</param>
        /// <returns>返回NpsAppSecret</returns>
        private async Task<NpsAppSecret> UpdateNpsChannelsIfCheckNotNullAsync(NpsAppSecret npsAppSecret)
        {
            Check.NotNull(npsAppSecret, nameof(npsAppSecret));
            Check.NotNull(npsAppSecret.NpsClient, nameof(npsAppSecret.NpsClient));
            Check.NotNull(npsAppSecret.NpsClient.NpsChannels, nameof(npsAppSecret.NpsClient.NpsChannels));

            _logger.LogInformation($"检查是否更新NpsChannels，设备唯一识别编码：{npsAppSecret.DeviceUniqueId}");
            var channelListOutput = await GetRemoteChannelOutputAsync(npsAppSecret.NpsClient.RemoteClientId);
            if (channelListOutput == null || channelListOutput.Datas == null || channelListOutput.Datas.Count == 0)
            {
                return npsAppSecret;
            }

            //先查询，再更新，否则报异常
            _npsChannelRepository.Attach(npsAppSecret.NpsClient.NpsChannels);
            for (int index = 0; index < npsAppSecret.NpsClient.NpsChannels.Count; index++)
            {
                var npsChannel = npsAppSecret.NpsClient.NpsChannels[index];
                var remoteChannel = channelListOutput.Datas.FirstOrDefault(x => x.Target.TargetAddress == npsChannel.DeviceAddress);
                if (remoteChannel != null)
                {//将远程隧道信息回写至本地Nps隧道表
                    npsChannel.RemoteChannelId = remoteChannel.Id;
                    npsChannel.Status = remoteChannel.Status;
                    npsChannel.RunStatus = remoteChannel.RunStatus;
                }
            }
            await _npsChannelRepository.UpdateAsync(npsAppSecret.NpsClient.NpsChannels);

            return npsAppSecret;
        }

        /// <summary>
        /// 根据服务器客户端Id，从服务器查询客户端隧道信息
        /// </summary>
        /// <param name="remoteClientId">服务器客户端Id</param>
        /// <returns>返回服务器客户端隧道信息</returns>
        private async Task<ChannelListOutput> GetRemoteChannelOutputAsync(int remoteClientId)
        {
            MiniProfiler.Current.Step("GetRemoteChannelOutputAsync");
            var baseAuthInput = await BeforeRequestNpsApiAsync();
            var searchApiResult = await _npsApi.ChannelListAsync(new ChannelListInput
            {
                AuthKey = baseAuthInput.AuthKey,
                Timestamp = baseAuthInput.Timestamp,
                ClientId = remoteClientId,
                KeyWords = "",
                Offset = "0",
                Limit = "100"
            });

            return searchApiResult ?? null;
        }

        #endregion

        #region 设备端口删除服务

        /// <summary>
        /// 设备端口删除服务
        /// </summary>
        /// <param name="input">删除服务参数</param>
        /// <returns>返回删除结果</returns>
        public async Task<List<NpsClientDeletedOutput>> DeleteAsync(NpsClientDeleteInput input)
        {
            var deletedOutputs = new List<NpsClientDeletedOutput>();

            //查询设备对应的应用密钥信息
            var npsAppSecret = await _npsAppSecretRepository
                .Where(x => x.DeviceUniqueId == input.DeviceUniqueId)
                .Where(x => x.CreateUserId == CurrentUser.UserId)
                .WhereCascade(x => x.IsDeleted == false)
                .Include(x => x.NpsServer)
                .Include(x => x.NpsClient)
                .ToOneAsync();
            if (npsAppSecret == null || npsAppSecret.NpsClient == null)
            {
                throw new NpsException("删除失败，查询不存在", StatusCode.NotFound);
            }

            //查询需要删除的隧道列表
            var npsChannels = await _npsChannelRepository
                .Where(x => x.NpsClientId == npsAppSecret.NpsClient.Id)
                .Where(x => input.DeletePorts.Contains(x.DeviceAddress))
                .ToListAsync();
            if (npsChannels.Count == 0)
            {
                throw new NpsException("删除失败，查询不存在", StatusCode.NotFound);
            }

            var deletedChannels = new List<NpsChannel>();
            var baseAuthInput = await BeforeRequestNpsApiAsync();
            for (int index = 0; index < npsChannels.Count; index++)
            {
                var npsChannel = npsChannels[index];

                var deletedOutput = new NpsClientDeletedOutput { DeviceAddress = npsChannel.DeviceAddress };
                //向远程服务发送删除指令
                var remoteApiResult = await _npsApi.DeleteChannelAsync(new ChannelIdInput
                {
                    AuthKey = baseAuthInput.AuthKey,
                    Timestamp = baseAuthInput.Timestamp,
                    Id = npsChannel.RemoteChannelId
                });
                deletedOutput.RemoteStatus = remoteApiResult.Status;
                deletedOutput.RemoteMessage = remoteApiResult.Message;
                deletedOutputs.Add(deletedOutput);

                if (remoteApiResult.Status == 1)
                {//远程服务器删除成功后，删除至删除集合列表，统一删除
                    deletedChannels.Add(npsChannel);
                }
            }
            if (deletedChannels.Count > 0)
            {
                await _npsChannelRepository.DeleteAsync(deletedChannels);
            }

            return deletedOutputs;
        }

        #endregion

        /// <summary>
        /// 请求Api前准备验签内容
        /// </summary>
        /// <returns>返回验签内容</returns>
        private async Task<BaseAuthInput> BeforeRequestNpsApiAsync()
        {
            var cacheValue = await _caching.GetAsync(_npsApi_Auth_CacheKey);
            if (cacheValue == null)
            {
                var serverTime = await _npsApi.ServerTimeAsync();
                string timestamp = serverTime?.Timestamp ?? DateTime.Now.ToTimestamp();
                string authKey = EncryptHelper.Md5By32($"{_auth_Key}{timestamp}").ToLower();

                var baseAuthInput = new BaseAuthInput
                {
                    AuthKey = authKey,
                    Timestamp = timestamp
                };

                await _caching.SetAsync(_npsApi_Auth_CacheKey, baseAuthInput.ToJson(), TimeSpan.FromSeconds(15));

                return baseAuthInput;
            }
            else
            {
                return cacheValue.FromJson<BaseAuthInput>();
            }
        }
    }
}