namespace Nps.Application.Account.Dtos
{
    /// <summary>
    /// 用户登录Dto
    /// </summary>
    public class LoginInput
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}