using Microsoft.AspNetCore.Mvc;
using Nps.Application.Nps.Dots;
using Nps.Application.Nps.Services;
using Nps.Core.Data;
using Nps.Core.Infrastructure.Extensions;
using Nps.Core.Infrastructure.Helpers;
using System.Threading.Tasks;

namespace Nps.Api.Controllers.Nps
{
    /// <summary>
    /// Nps服务器
    /// </summary>
    public class ServerController : BaseAdminApiController
    {
        private readonly INpsServerService _npsServerService;

        /// <summary>
        /// 初始化一个<see cref="ServerController"/>实例
        /// </summary>
        /// <param name="npsServerService">NpsServer服务</param>
        public ServerController(INpsServerService npsServerService)
        {
            _npsServerService = npsServerService;
        }

        /// <summary>
        /// 根据服务器IP查询服务器信息
        /// </summary>
        /// <param name="hostIP">服务IP</param>
        /// <returns>返回服务器信息</returns>
        [HttpGet("{hostIP}")]
        public async Task<IExecuteResult> GetAsync(string hostIP)
        {
            hostIP.CheckNotNullOrEmpty("服务器IP不能为空");
            if (!RegexHelper.IsIpAddress(hostIP))
            {
                return ExecuteResult.Error("请输入正确的IP地址格式", Core.Infrastructure.StatusCode.ParameterError);
            }

            return ExecuteResult.Ok(await _npsServerService.GetAsync(hostIP));
        }

        /// <summary>
        /// 根据条件分页查询服务器信息
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns>服务器信息列表</returns>
        [HttpPost("PageList")]
        public async Task<IExecuteResult> GetListAsync(PagingInput<NpsServerInput> input)
        {
            return ExecuteResult.Ok(await _npsServerService.GetListAsync(input));
        }
    }
}