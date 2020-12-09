using Nps.Infrastructure.Extensions;

namespace Nps.Core.Security
{
    /// <summary>
    /// 用户访问令牌
    /// </summary>
    public class Tokens
    {
        /// <summary>
        /// 构造函数，初始化一个<see cref="Tokens"/>实例
        /// </summary>
        /// <param name="accessToken">访问Token</param>
        /// <param name="refreshToken">刷新Token</param>
        /// <param name="expiresIn">token到期时间</param>
        public Tokens(string accessToken, string refreshToken, string expiresIn)
        {
            accessToken.CheckNotNullOrEmpty(nameof(accessToken));
            refreshToken.CheckNotNullOrEmpty(nameof(refreshToken));
            expiresIn.CheckNotNullOrEmpty(nameof(expiresIn));

            AccessToken = accessToken;
            RefreshToken = refreshToken;
            ExpiresIn = expiresIn;
        }

        /// <summary>
        /// Access_Token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Refresh_Token
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// 到期时间
        /// </summary>
        public string ExpiresIn { get; set; }

        /// <summary>
        /// 重载
        /// </summary>
        public override string ToString()
        {
            return $"Tokens AccessToken:{AccessToken},RefreshToken:{RefreshToken}";
        }
    }
}