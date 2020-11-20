using FreeSql.DataAnnotations;
using System;

namespace Nps.Core.Entities
{
    /*
     * 字段位置(Position) 属性值描述
     * 适用场景：当实体类继承时，CodeFirst创建表的字段顺序可能不是想要的，通过该特性可以设置顺序。
     * 创建表时指定字段位置，如：[Column(Position = 1]，可为负数即反方向位置；
     */

    /// <summary>
    /// 新增、更新、删除全量审计属性实体模型
    /// </summary>
    [Serializable, Table(DisableSyncStructure = false)]
    public class FullAuditEntity : FullAuditEntity<long>
    {
    }

    /// <summary>
    /// 新增、更新、删除全量审计属性实体模型
    /// </summary>
    /// <typeparam name="TKey">主键泛型</typeparam>
    [Serializable, Table(DisableSyncStructure = false)]
    public class FullAuditEntity<TKey> : Entity<TKey>, ICreateAuditEntity, IUpdateAuditEntity, IDeleteAuditEntity
    {
        /// <summary>
        /// 创建者ID
        /// </summary>
        [Column(Position = -7)]
        public long CreateUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column(Position = -6)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最后修改人Id
        /// </summary>
        [Column(Position = -5)]
        public long? UpdateUserId { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Column(Position = -4)]
        public DateTime? UpdateTime { get; set; }

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