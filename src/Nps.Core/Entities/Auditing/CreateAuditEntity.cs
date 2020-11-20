using FreeSql.DataAnnotations;
using System;

namespace Nps.Core.Entities
{
    /// <summary>
    /// 新增审计属性
    /// </summary>
    [Serializable, Table(DisableSyncStructure = false)]
    public class CreateAuditEntity : CreateAuditEntity<long>
    {

    }

    /// <summary>
    /// 新增审计属性
    /// </summary>
    [Serializable, Table(DisableSyncStructure = false)]
    public class CreateAuditEntity<TKey> : Entity<TKey>, ICreateAuditEntity
    {
        /// <summary>
        /// 创建者ID
        /// </summary>
        [Column(Position = -2)]
        public long CreateUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column(Position = -1)]
        public DateTime CreateTime { get; set; }
    }
}