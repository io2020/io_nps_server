using Nps.Application.SysLog.Dtos;
using Nps.Core.Data;
using Nps.Core.Security;
using Nps.Data.Entities;
using System;

namespace Nps.Application.SysLog.Services
{
    public class SqlCurdService : ISqlCurdService
    {
        private readonly IFreeSql _freeSql;

        private readonly ICurrentUser _currentUser;

        public SqlCurdService(IFreeSql freeSql, ICurrentUser currentUser)
        {
            _freeSql = freeSql;
            _currentUser = currentUser;
        }

        public IExecuteResult AddLog(SqlCurdAddInput input)
        {
            _freeSql.Insert(new SqlCurdLog
            {
                FullName = input.FullName,
                ExecuteMilliseconds = input.ExecuteMilliseconds,
                Sql = input.Sql,
                CreateTime = DateTime.Now,
                CreateUserId = _currentUser?.UserId ?? 0
            }).ExecuteIdentity();

            if (input.ExecuteMilliseconds > 1000)
            {//如果sql语句执行时间超过1m，执行以下操作
                //TODO
                //发送邮件/短信给负责人
            }

            return ExecuteResult.Ok();
        }
    }
}