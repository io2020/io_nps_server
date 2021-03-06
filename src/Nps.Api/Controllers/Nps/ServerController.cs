﻿using Microsoft.AspNetCore.Mvc;
using Nps.Application.Nps.Dtos;
using Nps.Application.Nps.Services;
using Nps.Core.Data;
using Nps.Infrastructure.Extensions;
using Nps.Infrastructure.Helpers;
using System.Threading.Tasks;

namespace Nps.Api.Controllers.Nps
{
    /// <summary>
    /// Nps服务器
    /// </summary>
    public class ServerController : BaseApiController
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
        /// <param name="serverIPAddress">服务器IP地址</param>
        /// <returns>返回服务器信息</returns>
        [HttpGet("{serverIPAddress}")]
        public async Task<IExecuteResult> GetAsync(string serverIPAddress)
        {
            serverIPAddress.CheckNotNullOrEmpty("服务器IP不能为空");
            if (!RegexHelper.IsIpAddress(serverIPAddress))
            {
                return ExecuteResult.Error("请输入正确的IP地址格式", Infrastructure.StatusCode.ParameterError);
            }

            return ExecuteResult.Ok(await _npsServerService.GetAsync(serverIPAddress));
        }

        /// <summary>
        /// 根据条件分页查询服务器信息
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns>服务器信息列表</returns>
        [HttpPost("search")]
        public async Task<IExecuteResult> SearchAsync(PagingInput<NpsServerSearchInput> input)
        {
            return ExecuteResult.Ok(await _npsServerService.SearchAsync(input));
        }
    }
}