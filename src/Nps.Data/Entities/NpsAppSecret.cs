using FreeSql.DataAnnotations;
using Nps.Core.Entities;
using System;

namespace Nps.Data.Entities
{
    /// <summary>
    /// Nps应用密钥表
    /// 一个设备对应一个密钥
    /// </summary>
    [Table(Name = "Nps_AppSecret")]
    public class NpsAppSecret : Entity, ICreateAuditEntity, IDeleteAuditEntity
    {
        /// <summary>
        /// 所属Nps服务器Id
        /// </summary>
        public long NpsServerId { get; set; }

        /// <summary>
        /// 所属Nps服务器
        /// </summary>
        [Navigate(nameof(NpsServerId))]
        public NpsServer NpsServer { get; set; }

        /// <summary>
        /// 设备唯一标识，不同用户允许重复
        /// </summary>
        [Column(StringLength = 50)]
        public string DeviceUniqueId { get; set; }

        /// <summary>
        /// 设备对应的唯一密钥，所有均不重复
        /// </summary>
        [Column(StringLength = 50)]
        public string AppSecret { get; set; }

        /// <summary>
        /// Nps客户端
        /// </summary>
        [Navigate(nameof(Id))]
        public NpsClient NpsClient { get; set; }

        /// <summary>
        /// 创建者ID
        /// </summary>
        [Column(Position = -5)]
        public long CreateUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column(Position = -4)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        [Column(Position = -3)]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 删除人id
        /// </summary>
        [Column(Position = -2)]
        public long? DeleteUserId { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        [Column(Position = -1)]
        public DateTime? DeleteTime { get; set; }
    }
}