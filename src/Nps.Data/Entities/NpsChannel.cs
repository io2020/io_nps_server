using FreeSql.DataAnnotations;
using Nps.Core.Entities;

namespace Nps.Data.Entities
{
    /// <summary>
    /// Nps隧道
    /// </summary>
    [Table(Name = "Nps_Channel")]
    public class NpsChannel : FullAuditEntity, IHasRevision
    {
        /// <summary>
        /// 所属客户端Id
        /// </summary>
        public long NpsClientId { get; set; }

        /// <summary>
        /// 所属客户端
        /// </summary>
        [Navigate(nameof(NpsClientId))]
        public NpsClient NpsClient { get; set; }

        /// <summary>
        /// Nps服务器隧道Id
        /// </summary>
        public int RemoteChannelId { get; set; }

        /// <summary>
        /// 服务端口 服务端生成 设计从10001开始，最大值60000
        /// </summary>
        public int ServerPort { get; set; }

        /// <summary>
        /// 目标地址(ip:端口)，支持只输入端口号
        /// </summary>
        public string DeviceAddress { get; set; }

        /// <summary>
        /// 隧道是否可用
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 隧道是否运行
        /// </summary>
        public bool RunStatus { get; set; }

        /// <summary>
        /// 隧道备注信息
        /// </summary>
        [Column(StringLength = 200)]
        public string Remark { get; set; }

        /// <summary>
        /// 乐观锁
        /// </summary>
        [Column(IsVersion = true, Position = -1)]
        public long Revision { get; set; }
    }
}