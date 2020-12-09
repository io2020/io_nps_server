using Nps.Application.Nps.Dtos;
using Nps.Core.Data;
using System.Threading.Tasks;

namespace Nps.Application.Nps.Services
{
    /// <summary>
    /// Nps服务器服务
    /// </summary>
    public interface INpsServerService
    {
        /// <summary>
        /// 根据服务器IP查询服务器信息
        /// </summary>
        /// <param name="hostIP">服务IP</param>
        /// <returns>返回服务器信息</returns>
        Task<NpsServerSearchOutput> GetAsync(string hostIP);

        /// <summary>
        /// 根据条件分页查询服务器信息
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns>服务器信息列表</returns>
        Task<PagingOutput<NpsServerSearchOutput>> SearchAsync(PagingInput<NpsServerSearchInput> input);
    }
}