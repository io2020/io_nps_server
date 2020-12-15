using FreeSql.DataAnnotations;
using Nps.Core.Entities;
using System.Collections.Generic;

namespace Nps.Data.Entities
{
    /// <summary>
    /// Nps服务器表
    /// </summary>
    [Table(Name = "Nps_Server")]
    public class NpsServer : CreateAuditEntity
    {
        /// <summary>
        /// 服务器序列号
        /// </summary>
        [Column(StringLength = 5)]
        public string SerialNumber { get; set; }

        /// <summary>
        /// 服务IP地址
        /// </summary>
        [Column(StringLength = 100)]
        public string ServerIPAddress { get; set; }

        /// <summary>
        /// 客户端连接端口
        /// </summary>
        public int ClientConnectPort { get; set; }

        /// <summary>
        /// 连接协议类型
        /// </summary>
        [Column(StringLength = 20)]
        public string ProtocolType { get; set; }

        /// <summary>
        /// 服务器所有设备密钥列表
        /// </summary>
        [Navigate(nameof(NpsAppSecret.NpsServerId))]
        public List<NpsAppSecret> NpsAppSecrets { get; set; }
    }
}