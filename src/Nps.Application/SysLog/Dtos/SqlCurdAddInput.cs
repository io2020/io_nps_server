namespace Nps.Application.SysLog.Dtos
{
    /// <summary>
    /// SQL CURD语句写入参数
    /// </summary>
    public class SqlCurdAddInput
    {
        /// <summary>
        /// 实体完整名称
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// SQL语句执行耗时（毫秒）
        /// </summary>
        public long ExecuteMilliseconds { get; set; }

        /// <summary>
        /// 执行的SQL语句
        /// </summary>
        public string Sql { get; set; }
    }
}