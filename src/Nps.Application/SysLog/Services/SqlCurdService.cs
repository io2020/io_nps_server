using Nps.Application.SysLog.Dtos;
using Nps.Core.Security;
using Nps.Data.Entities;
using System;

namespace Nps.Application.SysLog.Services
{
    /// <summary>
    /// SQL语句写入服务
    /// </summary>
    public class SqlCurdService : ISqlCurdService
    {
        private readonly IFreeSql _freeSql;

        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 初始化一个<see cref="SqlCurdService"/>实例
        /// </summary>
        /// <param name="freeSql">IFreeSql</param>
        /// <param name="currentUser">ICurrentUser</param>
        public SqlCurdService(IFreeSql freeSql, ICurrentUser currentUser)
        {
            _freeSql = freeSql;
            _currentUser = currentUser;
        }

        /// <summary>
        /// 记录业务执行SQL执行语句
        /// </summary>
        /// <param name="input">写入参数</param>
        public void Create(SqlCurdAddInput input)
        {
            _freeSql.Insert(new SqlCurdLog
            {
                FullName = input.FullName,
                ExecuteMilliseconds = input.ExecuteMilliseconds,
                Sql = input.Sql,
                CreateTime = DateTime.Now,
                CreateUserId = _currentUser?.UserId ?? 0
            }).ExecuteAffrows();

            if (input.ExecuteMilliseconds > 1000)
            {//如果sql语句执行时间超过1m，执行以下操作
                //TODO
                //发送邮件/短信给负责人
            }
        }
    }
}