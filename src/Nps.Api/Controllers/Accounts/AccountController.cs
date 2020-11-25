using Nps.Application.Account.Dtos;
using Nps.Application.Account.Services;
using Nps.Core.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Nps.Api.Controllers.Accounts
{
    /// <summary>
    /// 用户认证
    /// </summary>
    public class AccountController : BaseApiController
    {
        private readonly ITokenService _tokenService;

        /// <summary>
        /// 初始化一个<see cref="AccountController"/>实例
        /// </summary>
        /// <param name="tokenService">Jwt认证服务</param>
        public AccountController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="input">登录输入参数</param>
        /// <returns>返回执行结果</returns>
        [AllowAnonymous, HttpPost("login")]
        public async Task<IExecuteResult> LoginAsync(LoginInput input)
        {
            return ExecuteResult.Ok(await _tokenService.LoginAsync(input));
        }

        /// <summary>
        /// 刷新用户令牌
        /// </summary>
        /// <param name="refreshToken">刷新Token</param>
        /// <returns>返回执行结果</returns>
        [AllowAnonymous, HttpGet("{refreshToken}")]
        public async Task<IExecuteResult> GetRefreshTokenAsync(string refreshToken)
        {
            return ExecuteResult.Ok(await _tokenService.GetRefreshTokenAsync(refreshToken));
        }
    }
}