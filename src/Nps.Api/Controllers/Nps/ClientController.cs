using Microsoft.AspNetCore.Mvc;
using Nps.Application.Nps.Dots;
using Nps.Application.Nps.Services;
using Nps.Core.Data;
using System.Threading.Tasks;

namespace Nps.Api.Controllers.Nps
{
    /// <summary>
    /// Nps客户端
    /// </summary>
    public class ClientController : BaseAdminApiController
    {
        private readonly INpsClientService _npsClientService;

        /// <summary>
        /// 初始化一个<see cref="ClientController"/>实例
        /// </summary>
        /// <param name="npsClientService">NpsClient服务</param>
        public ClientController(INpsClientService npsClientService)
        {
            _npsClientService = npsClientService;
        }

        /// <summary>
        /// 设备查询服务，根据条件分页查询客户端信息
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns>客户端列表</returns>
        [HttpPost("search")]
        public async Task<IExecuteResult> SearchAsync(PagingInput<NpsClientSearchInput> input)
        {
            return ExecuteResult.Ok(await _npsClientService.SearchAsync(input));
        }

        /// <summary>
        /// 设备开通服务
        /// </summary>
        /// <param name="input">设备开通服务输入参数</param>
        /// <returns>返回执行结果</returns>
        [HttpPost("open")]
        public async Task<IExecuteResult> OpenAsync(NpsClientOpenInput input)
        {
            return ExecuteResult.Ok(await _npsClientService.OpenAsync(input));
        }

        /// <summary>
        /// 设备端口删除服务
        /// </summary>
        /// <param name="input">删除服务参数</param>
        /// <returns>返回删除结果</returns>
        [HttpDelete("delete")]
        public async Task<IExecuteResult> DeleteAsync(NpsClientDeleteInput input)
        {
            return ExecuteResult.Ok(await _npsClientService.DeleteAsync(input));
        }
    }
}