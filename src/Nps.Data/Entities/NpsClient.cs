using FreeSql.DataAnnotations;
using Nps.Core.Entities;
using System.Collections.Generic;

namespace Nps.Data.Entities
{
    /// <summary>
    /// Nps客户端
    /// </summary>
    [Table(Name = "Nps_Client")]
    public class NpsClient : FullAuditEntity, IHasRevision
    {
        /// <summary>
        /// 设备应用密钥id 1:1导航属性
        /// </summary>
        public long NpsAppSecretId { get; set; }

        /// <summary>
        /// 设备应用密钥
        /// </summary>
        [Navigate(nameof(NpsAppSecretId))]
        public NpsAppSecret NpsAppSecret { get; set; }

        /// <summary>
        /// Nps服务器客户端Id
        /// </summary>
        public int RemoteClientId { get; set; }

        /// <summary>
        /// 是否允许客户端以配置文件模式连接 1允许 0不允许
        /// </summary>
        public bool IsConfigConnAllow { get; set; }

        /// <summary>
        /// 是否压缩1允许 0不允许
        /// </summary>
        public bool IsCompress { get; set; }

        /// <summary>
        /// 是否加密（1或者0）1允许 0不允许
        /// </summary>
        public bool IsCrypt { get; set; }

        /// <summary>
        /// 客户端是否可用
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 客户端是否连接
        /// </summary>
        public bool IsConnect { get; set; }

        /// <summary>
        /// 最后一次连接客户端IP地址
        /// </summary>
        [Column(StringLength = 50)]
        public string LastConnectAddress { get; set; }

        /// <summary>
        /// 客户端备注信息
        /// </summary>
        [Column(StringLength = 200)]
        public string Remark { get; set; }

        /// <summary>
        /// 乐观锁
        /// </summary>
        [Column(IsVersion = true, Position = -1)]
        public long Revision { get; set; }

        /// <summary>
        /// 客户端所有隧道列表
        /// </summary>
        [Navigate(nameof(NpsChannel.NpsClientId))]
        public List<NpsChannel> NpsChannels { get; set; }
    }
}