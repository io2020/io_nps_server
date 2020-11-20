using System.Security.Claims;

namespace Nps.Core.Security
{
    /// <summary>
    /// Define ClaimTypes
    /// </summary>
    public static class NpsClaimTypes
    {
        /// <summary>
        /// Default: <see cref="ClaimTypes.NameIdentifier"/>
        /// </summary>
        public static string UserId { get; set; } = ClaimTypes.NameIdentifier;

        /// <summary>
        /// Default: <see cref="ClaimTypes.Name"/>
        /// </summary>
        public static string UserName { get; set; } = ClaimTypes.Name;

        /// <summary>
        /// Default: <see cref="ClaimTypes.GivenName"/>
        /// </summary>
        public static string NikeName { get; set; } = ClaimTypes.GivenName;

        /// <summary>
        /// Default: "MobilePhone".
        /// </summary>
        public static string Mobile { get; set; } = ClaimTypes.MobilePhone;

        /// <summary>
        /// Default: <see cref="ClaimTypes.Email"/>
        /// </summary>
        public static string Email { get; set; } = ClaimTypes.Email;

        /// <summary>
        /// Default: <see cref="ClaimTypes.Role"/>
        /// </summary>
        public static string Role { get; set; } = ClaimTypes.Role;
    }
}