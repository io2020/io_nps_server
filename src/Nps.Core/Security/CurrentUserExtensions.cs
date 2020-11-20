namespace Nps.Core.Security
{
    /// <summary>
    /// CurrentUser扩展
    /// </summary>
    public static class CurrentUserExtensions
    {
        /// <summary>
        /// 根据ClaimType查询ClaimValue
        /// </summary>
        /// <param name="currentUser">ICurrentUser</param>
        /// <param name="claimType">ClaimType</param>
        /// <returns>返回ClaimValue</returns>
        public static string FindClaimValue(this ICurrentUser currentUser, string claimType)
        {
            return currentUser.FindClaim(claimType)?.Value;
        }

        /// <summary>
        /// 根据ClaimType查询ClaimValue
        /// </summary>
        /// <typeparam name="T">泛型对象</typeparam>
        /// <param name="currentUser">ICurrentUser</param>
        /// <param name="claimType">ClaimType</param>
        /// <returns>返回ClaimValue</returns>
        public static T FindClaimValue<T>(this ICurrentUser currentUser, string claimType)
            where T : struct
        {
            var value = currentUser.FindClaimValue(claimType);
            if (value == null)
            {
                return default;
            }

            return value.To<T>();
        }
    }
}