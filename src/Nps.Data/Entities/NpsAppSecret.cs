using FreeSql.DataAnnotations;
using Nps.Core.Entities;

namespace Nps.Data.Entities
{
    /// <summary>
    /// Nps应用密钥表
    /// 一个设备对应一个密钥
    /// </summary>
    [Table(Name = "Nps_AppSecret")]
    public class NpsAppSecret : FullAuditEntity, IHasRevision
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
        /// 乐观锁
        /// </summary>
        [Column(IsVersion = true, Position = -1)]
        public long Revision { get; set; }
    }
}