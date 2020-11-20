using FreeSql.DataAnnotations;
using Nps.Core.Entities;
using System;

namespace Nps.Data.Entities
{
    /// <summary>
    /// 用户表
    /// </summary>
    [Table(Name = "Sys_User")]
    public class User : FullAuditEntity
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        [Column(StringLength = 24)]
        public string UserName { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        [Column(StringLength = 24)]
        public string NikeName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Column(StringLength = 100)]
        public string Email { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Column(StringLength = 15)]
        public string Mobile { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [Column(StringLength = 100)]
        public string Password { get; set; }

        /// <summary>
        /// 最后一次登录的时间
        /// </summary>
        public DateTime LastLoginTime { get; set; }

        /// <summary>
        /// JWT 登录，保存生成的随机token值。
        /// </summary>
        [Column(StringLength = 200)]
        public string RefreshToken { get; set; }
    }
}