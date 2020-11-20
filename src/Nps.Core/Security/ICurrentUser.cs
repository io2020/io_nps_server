using System.Security.Claims;

namespace Nps.Core.Security
{
    /*
     * https://andrewlock.net/introduction-to-authentication-with-asp-net-core/
     * ASP.NET Core 的验证模型是 claims-based authentication 。
     Claim 证件单元，存储信息最小单位
     * Claim是对被验证主体特征的一种表述，比如：登录用户名是...，email是...，用户Id是...，其中的"登录用户名"，"email"，"用户Id"就是ClaimType。
     * You can think of claims as being a statement about...That statement consists of a name and a value.
     * 对应现实中的事物，比如驾照，驾照中的"身份证号码：xxx"是一个claim，"姓名：xxx"是另一个claim。
     
     ClaimsIdentity 相当于一个证件
     * 一组claims构成了一个identity，具有这些claims的identity就是ClaimsIdentity
     * 驾照就是一种ClaimsIdentity，可以把ClaimsIdentity理解为"证件"，驾照是一种证件，护照也是一种证件。
     
     ClaimsPrincipal 则是证件的持有者
     * ClaimsIdentity的持有者就是ClaimsPrincipal，一个ClaimsPrincipal可以持有多个ClaimsIdentity，而一个ClaimsIdentity中可以有多个Claim。就比如一个人既持有驾照，又持有护照，驾照中包含姓名、身份证号码等信息。
     */

    /// <summary>
    /// 当前登录用户信息
    /// Reference Volo.Abp.Security
    /// </summary>
    public interface ICurrentUser
    {
        /// <summary>
        /// 是否已认证
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// 用户Id
        /// </summary>
        long? UserId { get; }

        /// <summary>
        /// 用户账户
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        string NikeName { get; }

        /// <summary>
        /// 手机号
        /// </summary>
        string Mobile { get; }

        /// <summary>
        /// 邮箱
        /// </summary>
        string Email { get; }

        /// <summary>
        /// 用户角色集合
        /// </summary>
        string[] Roles { get; }

        /// <summary>
        /// 是否包含某个角色
        /// </summary>
        /// <param name="roleName">验证的角色名称</param>
        /// <returns>True/False</returns>
        bool IsInRole(string roleName);

        /// <summary>
        /// 根据ClaimType查找Claim
        /// </summary>
        Claim FindClaim(string claimType);

        /// <summary>
        /// 根据ClaimType查找Claim集合
        /// </summary>
        Claim[] FindClaims(string claimType);

        /// <summary>
        /// 获取所有的Claim
        /// </summary>
        Claim[] GetAllClaims();
    }
}