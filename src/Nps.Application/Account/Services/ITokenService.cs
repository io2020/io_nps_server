using Nps.Application.Account.Dtos;
using Nps.Core.Security;
using Nps.Data.Entities;
using System.Threading.Tasks;

namespace Nps.Application.Account.Services
{
    public interface ITokenService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="input">登录参数</param>
        /// <returns>返回Token</returns>
        Task<Tokens> LoginAsync(LoginInput input);

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="refreshToken">刷新Token</param>
        /// <returns>返回Token</returns>
        Task<Tokens> GetRefreshTokenAsync(string refreshToken);
    }
}