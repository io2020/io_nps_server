using Nps.Application.Nps.Dtos;
using Nps.Core.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nps.Application.Nps.Services
{
    /// <summary>
    /// Nps客户端服务接口
    /// </summary>
    public interface INpsClientService
    {
        /// <summary>
        /// 分页查询所有已开通服务列表
        /// </summary>
        /// <param name="input">查询服务参数</param>
        /// <returns>分页返回查询结果</returns>
        Task<List<NpsClientOpenedOutput>> SearchAsync(PagingInput<NpsClientSearchInput> input);

        /// <summary>
        /// 开通服务
        /// </summary>
        /// <param name="input">开通服务参数</param>
        /// <returns>返回设备已开通端口结果</returns>
        Task<NpsClientOpenedOutput> OpenAsync(NpsClientOpenInput input);

        /// <summary>
        /// 删除服务
        /// </summary>
        /// <param name="input">删除服务参数</param>
        /// <returns>返回删除结果</returns>
        Task<List<NpsClientDeletedOutput>> DeleteAsync(NpsClientDeleteInput input);
    }
}