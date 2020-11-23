using Microsoft.Extensions.Logging;
using Nps.Application.Nps.Dots;
using Nps.Application.NpsApi;
using Nps.Application.NpsApi.Dtos;
using Nps.Core.Aop.Attributes;
using Nps.Core.Infrastructure;
using Nps.Core.Infrastructure.Exceptions;
using Nps.Core.Infrastructure.Extensions;
using Nps.Core.Infrastructure.Helpers;
using Nps.Core.Infrastructure.IdGenerators;
using Nps.Core.Repositories;
using Nps.Core.Services;
using Nps.Data.Entities;
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
        private readonly ILogger<NpsClientService> _logger;

        private readonly INpsApi _npsApi;

        private readonly IGuidGenerator _guidGenerator;

        private readonly IFreeSqlRepository<NpsServer> _npsServerRepository;

        private readonly IFreeSqlRepository<NpsAppSecret> _npsAppSecretRepository;

        private readonly IFreeSqlRepository<NpsClient> _npsClientRepository;

        private readonly IFreeSqlRepository<NpsChannel> _npsChannelRepository;

        private readonly string _auth_Key = "jyioself2020";

        private readonly int _minServerPort = 10010;

        private readonly string _protocolType = "tcp";

        /// <summary>
        /// 初始化一个<see cref="NpsClientService"/>实例
        /// </summary>
        /// <param name="logger">日志对象</param>
        /// <param name="npsApi">Nps服务器Api</param>
        /// <param name="guidGenerator">有序GUID生成器</param>
        /// <param name="npsServerRepository">Nps服务器仓储</param>
        /// <param name="npsAppSecretRepository">Nps应用密钥仓储</param>
        /// <param name="npsClientRepository">Nps客户端仓储</param>
        /// <param name="npsChannelRepository">Nps隧道仓储</param>
        public NpsClientService(
            ILogger<NpsClientService> logger,
            INpsApi npsApi,
            IGuidGenerator guidGenerator,
            IFreeSqlRepository<NpsServer> npsServerRepository,
            IFreeSqlRepository<NpsAppSecret> npsAppSecretRepository,
            IFreeSqlRepository<NpsClient> npsClientRepository,
            IFreeSqlRepository<NpsChannel> npsChannelRepository)
        {
            _logger = logger;
            _npsApi = npsApi;
            _guidGenerator = guidGenerator;
            _npsServerRepository = npsServerRepository;
            _npsAppSecretRepository = npsAppSecretRepository;
            _npsClientRepository = npsClientRepository;
            _npsChannelRepository = npsChannelRepository;
        }

        public Task<bool> GetListAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 设备开通服务
        /// </summary>
        /// <param name="input">设备开通服务输入参数</param>
        /// <returns>返回执行结果</returns>
        [Transactional]
        public async Task<NpsOpenedOutput> OpenAsync(NpsOpenInput input)
        {
            _logger.LogInformation($"开始准备开通设备端口，{input.ToJson()}");

            _logger.LogInformation($"检测用户是否存在");
            if (!CurrentUser?.UserId.HasValue ?? false || CurrentUser?.UserId.Value == 0)
            {
                throw new NpsException("用户不存在，请先登录", StatusCode.AuthenticationFailed);
            }

            var npsAppSecret = await GetNpsAppSecretAsync(input.DeviceUniqueId);
            npsAppSecret = await CreateNpsAppSecretIfCheckNullAsync(input.DeviceUniqueId, npsAppSecret);
            npsAppSecret = await CreateNpsClientIfCheckNullAsync(input, npsAppSecret);
            npsAppSecret = await UpdateNpsClientOrNpsServerIfCheckNotNullAsync(npsAppSecret);
            npsAppSecret = await CreateNpsChannelIfCheckNullAsync(input, npsAppSecret);
            npsAppSecret = await UpdateNpsChannelsIfCheckNotNullAsync(npsAppSecret);

            _logger.LogInformation($"已成功开通设备端口，{input.ToJson()}");

            try
            {
                var npsDeviceOpenOutput = Mapper.Map<NpsOpenedOutput>(npsAppSecret);

                return npsDeviceOpenOutput;
            }
            catch (Exception ex)
            {
                throw new NpsException(ex.Message, StatusCode.Error);
            }
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
        private async Task<NpsAppSecret> CreateNpsClientIfCheckNullAsync(NpsOpenInput input, NpsAppSecret npsAppSecret)
        {
            Check.NotNull(npsAppSecret, nameof(npsAppSecret));

            _logger.LogInformation($"检测设备是否已创建过客户端，设备唯一识别编码：{npsAppSecret.DeviceUniqueId}");
            if (npsAppSecret.NpsClient == null)
            {
                //调用远程Api执行添加客户端
                var (authKey, timestamp) = await BeforeRequestNpsApiAsync();
                var remoteApiResult = await _npsApi.AddClientAsync(new ClientAddInput
                {
                    AuthKey = authKey,
                    Timestamp = timestamp,
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

                    //将服务器信息与设备应用密钥关联
                    _npsAppSecretRepository.Attach(npsAppSecret);
                    npsAppSecret.NpsServerId = npsAppSecret.NpsServer.Id;
                    await _npsAppSecretRepository.UpdateAsync(npsAppSecret);
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
            var (authKey, timestamp) = await BeforeRequestNpsApiAsync();
            var searchApiResult = await _npsApi.ClientListAsync(new ClientListInput
            {
                AuthKey = authKey,
                Timestamp = timestamp,
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
        private async Task<NpsAppSecret> CreateNpsChannelIfCheckNullAsync(NpsOpenInput input, NpsAppSecret npsAppSecret)
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

                var (authKey, timestamp) = await BeforeRequestNpsApiAsync();
                for (int index = 0; index < needOpenPorts.Count; index++)
                {
                    //向远程服务器添加隧道
                    var remark = $"{input.Remark}_隧道_{startServerPort += 1}";
                    var remoteApiResult = await _npsApi.AddChannelAsync(new ChannelAddInput
                    {
                        AuthKey = authKey,
                        Timestamp = timestamp,
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

                    await Task.Delay(150);
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
            var (authKey, timestamp) = await BeforeRequestNpsApiAsync();
            var searchApiResult = await _npsApi.ChannelListAsync(new ChannelListInput
            {
                AuthKey = authKey,
                Timestamp = timestamp,
                ClientId = remoteClientId,
                KeyWords = "",
                Offset = "0",
                Limit = "100"
            });

            return searchApiResult ?? null;
        }

        /// <summary>
        /// 请求Api前准备验签内容
        /// </summary>
        /// <returns>返回验签内容</returns>
        private async Task<(string authKey, string timestamp)> BeforeRequestNpsApiAsync()
        {
            var serverTime = await _npsApi.ServerTimeAsync();
            string timestamp = serverTime?.Timestamp ?? DateTime.Now.ToTimestamp();

            string authKey = EncryptHelper.Md5By32($"{_auth_Key}{timestamp}").ToLower();

            return new(authKey, timestamp);
        }

        public Task<bool> DeleteAsync()
        {
            throw new NotImplementedException();
        }
    }
}