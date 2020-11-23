using Nps.Application.Nps.Dots;
using Nps.Core.Data;
using Nps.Core.Repositories;
using Nps.Core.Services;
using Nps.Data.Entities;
using Nps.Data.FreeSql;
using System.Threading.Tasks;

namespace Nps.Application.Nps.Services
{
    /// <summary>
    /// Nps服务器服务
    /// </summary>
    public class NpsServerService : DomainService, INpsServerService
    {
        private readonly IFreeSqlRepository<NpsServer> _npsServerRepository;

        /// <summary>
        /// 初始化一个<see cref="NpsServerService"/>实例
        /// </summary>
        /// <param name="npsServerRepository">NpsServer仓储</param>
        public NpsServerService(IFreeSqlRepository<NpsServer> npsServerRepository)
        {
            _npsServerRepository = npsServerRepository;
        }

        /// <summary>
        /// 根据服务器IP查询服务器信息
        /// </summary>
        /// <param name="serverIPAddress">服务器IP地址</param>
        /// <returns>返回服务器信息</returns>
        public async Task<NpsServerSearchOutput> GetAsync(string serverIPAddress)
        {
            var server = await _npsServerRepository
                .Where(x => x.ServerIPAddress == serverIPAddress)
                .ToOneAsync<NpsServerSearchOutput>();

            return server;
        }

        /// <summary>
        /// 根据条件分页查询服务器信息
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns>服务器信息列表</returns>
        public async Task<PagingOutput<NpsServerSearchOutput>> SearchAsync(PagingInput<NpsServerSearchInput> input)
        {
            var servers = await _npsServerRepository
                .WhereIf(input.Filter.ServerIPAddress.IsNotNullOrEmpty(), x => x.ServerIPAddress == input.Filter.ServerIPAddress)
                .OrderByDescending(x => x.CreateTime)
                .ToPagingListAsync<NpsServer, NpsServerSearchOutput>(input, out long count);

            return servers.ToPagingOutput(count);
        }
    }
}