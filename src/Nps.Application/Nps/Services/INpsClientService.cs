using Nps.Application.Nps.Dots;
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
        /// <returns></returns>
        Task<bool> GetListAsync();

        /// <summary>
        /// 开通服务
        /// </summary>
        /// <param name="input">开通服务参数</param>
        /// <returns>返回设备已开通端口结果</returns>
        Task<NpsOpenedOutput> OpenAsync(NpsOpenInput input);

        /// <summary>
        /// 删除服务
        /// </summary>
        /// <returns></returns>
        Task<bool> DeleteAsync();
    }
}