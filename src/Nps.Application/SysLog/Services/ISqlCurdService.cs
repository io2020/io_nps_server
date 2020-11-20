using Nps.Application.SysLog.Dtos;
using Nps.Core.Data;

namespace Nps.Application.SysLog.Services
{
    public interface ISqlCurdService
    {
        IExecuteResult AddLog(SqlCurdAddInput input);
    }
}