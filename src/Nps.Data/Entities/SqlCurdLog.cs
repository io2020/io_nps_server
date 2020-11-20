using FreeSql.DataAnnotations;
using Nps.Core.Aop.Attributes;
using Nps.Core.Entities;

namespace Nps.Data.Entities
{
    /// <summary>
    /// SQLCurd语句日志表
    /// </summary>
    [Table(Name = "Sys_Log_SqlCurd"), DisableSqlCurd]
    public class SqlCurdLog : CreateAuditEntity, IHasRevision
    {
        /// <summary>
        /// 实体类型全部名称
        /// </summary>
        [Column(StringLength = 100)]
        public string FullName { get; set; }

        /// <summary>
        /// SQL语句执行时间（毫秒）
        /// </summary>
        public long ExecuteMilliseconds { get; set; }

        /// <summary>
        /// SQL语句
        /// </summary>
        [Column(StringLength = -1)]
        public string Sql { get; set; }

        /// <summary>
        /// 乐观锁
        /// </summary>
        [Column(IsVersion = true, Position = -1)]
        public long Revision { get; set; }
    }
}