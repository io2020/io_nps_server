using FreeSql.DataAnnotations;
using Nps.Core.Aop.Attributes;
using Nps.Core.Entities;

namespace Nps.Data.Entities
{
    /// <summary>
    /// 接口调用日志表
    /// </summary>
    [Table(Name = "Sys_Log_Action"), DisableSqlCurd]
    public class ActionTrackLog : CreateAuditEntity, IHasRevision
    {
        /// <summary>
        /// 请求域名
        /// </summary>
        [Column(StringLength = 200)]
        public string HostDomain { get; set; }

        /// <summary>
        /// 请求接口提交方法
        /// </summary>
        [Column(StringLength = 25)]
        public string ApiMethod { get; set; }

        /// <summary>
        /// 请求接口地址
        /// </summary>
        [Column(StringLength = 100)]
        public string ApiPath { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        [Column(StringLength = -1)]
        public string ApiParams { get; set; }

        /// <summary>
        /// 用户代理
        /// </summary>
        [Column(StringLength = 500)]
        public string UserAgent { get; set; }

        /// <summary>
        /// 设备IP
        /// </summary>
        [Column(StringLength = 30)]
        public string IP { get; set; }

        /// <summary>
        /// 接口响应耗时（毫秒）
        /// </summary>
        public long? ExecuteMilliseconds { get; set; }

        /// <summary>
        /// 接口响应状态码
        /// </summary>
        public int? StatusCode { get; set; }

        /// <summary>
        /// 接口响应结果
        /// </summary>
        [Column(StringLength = -1)]
        public string ExecuteResult { get; set; }

        /// <summary>
        /// 接口响应消息
        /// </summary>
        [Column(StringLength = 500)]
        public string ExecuteMessage { get; set; }

        /// <summary>
        /// 乐观锁
        /// </summary>
        [Column(IsVersion = true, Position = -1)]
        public long Revision { get; set; }
    }
}