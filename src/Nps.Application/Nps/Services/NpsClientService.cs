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
using System.Threading.Tasks;

namespace Nps.Application.Nps.Services
{
    /// <summary>
    /// Nps客户端服务
    /// </summary>
    public class NpsClientService : DomainService, INpsClientService
    {
        private readonly ILogger<NpsClientService> _logger;

        private readonly IFreeSqlRepository<NpsServer> _npsServerRepository;

        private readonly IFreeSqlRepository<NpsAppSecret> _npsAppSecretRepository;

        private readonly IFreeSqlRepository<NpsClient> _npsClientRepository;

        private readonly IFreeSqlRepository<NpsChannel> _npsChannelRepository;

        private readonly INpsApi _npsApi;

        private readonly IGuidGenerator _guidGenerator;

        private readonly string _auth_Key = "jyioself2020";

        /// <summary>
        /// 初始化一个<see cref="NpsClientService"/>实例
        /// </summary>
        /// <param name="logger">日志对象</param>
        /// <param name="npsServerRepository">Nps服务器仓储</param>
        /// <param name="npsAppSecretRepository">Nps应用密钥仓储</param>
        /// <param name="npsClientRepository">Nps客户端仓储</param>
        /// <param name="npsChannelRepository">Nps隧道仓储</param>
        /// <param name="npsApi">Nps服务器Api</param>
        /// <param name="guidGenerator">有序GUID生成器</param>
        public NpsClientService(
            ILogger<NpsClientService> logger,
            IFreeSqlRepository<NpsServer> npsServerRepository,
            IFreeSqlRepository<NpsAppSecret> npsAppSecretRepository,
            IFreeSqlRepository<NpsClient> npsClientRepository,
            IFreeSqlRepository<NpsChannel> npsChannelRepository,
            INpsApi npsApi,
            IGuidGenerator guidGenerator)
        {
            _logger = logger;
            _npsServerRepository = npsServerRepository;
            _npsAppSecretRepository = npsAppSecretRepository;
            _npsClientRepository = npsClientRepository;
            _npsChannelRepository = npsChannelRepository;
            _npsApi = npsApi;
            _guidGenerator = guidGenerator;
        }

        public Task<bool> DeleteAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetAsync(string deviceUniqueId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetListAsync()
        {
            throw new NotImplementedException();
        }

        [Transactional]
        public async Task<bool> OpenAsync(NpsOpenInput input)
        {
            _logger.LogInformation($"开始准备开通设备端口，{input.ToJson()}");

            _logger.LogInformation($"检测用户是否存在");

            if (!CurrentUser?.UserId.HasValue ?? false)
            {
                throw new NpsException("用户不存在，请先登录", StatusCode.AuthenticationFailed);
            }

            var appSecret = await GetNpsAppSecretAsync(input.DeviceUniqueId);
            appSecret.NpsClient = await GetNpsClientAsync(appSecret, input);

            return true;
        }

        public Task<bool> StartAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> StopAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 检测设备是否已创建过唯一识别密钥,
        /// 如果不存在，则创建
        /// </summary>
        /// <param name="deviceUniqueId">设备唯一识别码</param>
        /// <returns>返回设备密钥信息</returns>
        private async Task<NpsAppSecret> GetNpsAppSecretAsync(string deviceUniqueId)
        {
            _logger.LogInformation($"检测设备是否已创建过唯一识别密钥，设备唯一编号：{deviceUniqueId}");

            var appSecret = await _npsAppSecretRepository
                .Where(x => x.DeviceUniqueId == deviceUniqueId)
                .Where(x => x.CreateUserId == CurrentUser.UserId)
                .WhereCascade(x => x.IsDeleted == false)
                .Include(x => x.NpsClient)
                .IncludeMany(x => x.NpsClient.NpsChannels)
                .ToOneAsync();

            if (appSecret == null)
            {
                appSecret = await _npsAppSecretRepository.InsertAsync(new NpsAppSecret
                {
                    DeviceUniqueId = deviceUniqueId,
                    AppSecret = _guidGenerator.Create().ToString("N")
                });
            }

            return appSecret;
        }

        /// <summary>
        /// 检测设备是否已创建过客户端
        /// 如果不存在，则创建
        /// </summary>
        /// <param name="npsAppSecret">设备唯一密钥</param>
        /// <param name="input">开通客户端输入参数</param>
        /// <returns>返回设备客户端</returns>
        private async Task<NpsClient> GetNpsClientAsync(NpsAppSecret npsAppSecret, NpsOpenInput input)
        {
            _logger.LogInformation($"检测设备是否已创建过客户端，设备唯一密钥：{npsAppSecret.AppSecret}");

            if (npsAppSecret.NpsClient == null)
            {
                var authPrepare = await Prepare();

                var remouteApiResult =await _npsApi.AddClientAsync(new ClientAddInput
                {
                    AuthKey=authPrepare.authKey,
                    Timestamp=authPrepare.timestamp,
                    AppSecret= npsAppSecret.AppSecret,
                    IsConfigConnAllow=input.IsConfigConnAllow.ToInt32OrDefault(1),
                    IsCompress=input.IsCompress.ToInt32OrDefault(0),
                    IsCrypt=input.IsCrypt.ToInt32OrDefault(1)
                });
                if (remouteApiResult.Status == 1)
                {
                    authPrepare = await Prepare();
                    var searchApiResult = await _npsApi.ClientListAsync(new ClientListInput
                    {
                        AuthKey = authPrepare.authKey,
                        Timestamp = authPrepare.timestamp,
                        KeyWords = npsAppSecret.AppSecret,
                        OrderType = "asc",
                        Offset = "0",
                        Limit = "1"
                    });
                    if (searchApiResult != null)
                    {

                    }
                }
            }

            return new NpsClient();
        }

        /// <summary>
        /// 请求Api前准备验签内容
        /// </summary>
        /// <returns>返回验签内容</returns>
        private async Task<(string authKey, string timestamp)> Prepare()
        {
            var serverTime = await _npsApi.ServerTimeAsync();
            string timestamp = serverTime?.Timestamp ?? DateTime.Now.ToTimestamp();

            string authKey = EncryptHelper.Md5By32($"{_auth_Key}{timestamp}").ToLower();

            return new(authKey, timestamp);
        }
    }
}