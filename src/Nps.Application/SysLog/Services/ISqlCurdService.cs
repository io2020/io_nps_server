using Nps.Application.SysLog.Dtos;

namespace Nps.Application.SysLog.Services
{
    /// <summary>
    /// SQL语句写入服务接口
    /// </summary>
    public interface ISqlCurdService
    {
        /// <summary>
        /// 记录业务执行SQL执行语句
        /// </summary>
        /// <param name="input">写入参数</param>
        void Create(SqlCurdAddInput input);
    }
}