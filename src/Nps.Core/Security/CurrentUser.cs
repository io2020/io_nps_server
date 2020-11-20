using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using System.Threading;

namespace Nps.Core.Security
{
    /// <summary>
    /// 当前认证用户
    /// </summary>
    public class CurrentUser : ICurrentUser
    {
        private readonly ClaimsPrincipal _claimsPrincipal;

        private static readonly Claim[] EmptyClaimsArray = new Claim[0];

        /// <summary>
        /// 是否已认证
        /// </summary>
        public virtual bool IsAuthenticated => UserId.HasValue;

        /// <summary>
        /// 用户Id
        /// </summary>
        public virtual long? UserId => this.FindClaimValue<long>(NpsClaimTypes.UserId);

        /// <summary>
        /// 用户账户
        /// </summary>
        public virtual string UserName => this.FindClaimValue(NpsClaimTypes.UserName);

        /// <summary>
        /// 用户昵称
        /// </summary>
        public virtual string NikeName => this.FindClaimValue(NpsClaimTypes.NikeName);

        /// <summary>
        /// 手机号
        /// </summary>
        public virtual string Mobile => this.FindClaimValue(NpsClaimTypes.Mobile);

        /// <summary>
        /// 邮箱
        /// </summary>
        public virtual string Email => this.FindClaimValue(NpsClaimTypes.Email);

        /// <summary>
        /// 用户角色集合
        /// </summary>
        public virtual string[] Roles => FindClaims(NpsClaimTypes.Role).Select(c => c.Value).ToArray();

        /// <summary>
        /// 构造函数，初始化一个<see cref="CurrentUser"/>实例
        /// </summary>
        /// <param name="httpContextAccessor">IHttpContextAccessor</param>
        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _claimsPrincipal = httpContextAccessor.HttpContext?.User ?? Thread.CurrentPrincipal as ClaimsPrincipal;
        }

        /// <summary>
        /// 是否包含某个角色
        /// </summary>
        /// <param name="roleName">验证的角色名称</param>
        /// <returns>True/False</returns>
        public virtual bool IsInRole(string roleName)
        {
            return FindClaims(NpsClaimTypes.Role).Any(c => c.Value == roleName);
        }

        /// <summary>
        /// 根据ClaimType查找Claim
        /// </summary>
        public virtual Claim FindClaim(string claimType)
        {
            return _claimsPrincipal?.Claims.FirstOrDefault(c => c.Type == claimType);
        }

        /// <summary>
        /// 根据ClaimType查找Claim集合
        /// </summary>
        public virtual Claim[] FindClaims(string claimType)
        {
            return _claimsPrincipal?.Claims.Where(c => c.Type == claimType).ToArray() ?? EmptyClaimsArray;
        }

        /// <summary>
        /// 获取所有的Claim
        /// </summary>
        public virtual Claim[] GetAllClaims()
        {
            return _claimsPrincipal?.Claims.ToArray() ?? EmptyClaimsArray;
        }
    }
}