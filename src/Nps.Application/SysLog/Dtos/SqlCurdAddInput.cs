namespace Nps.Application.SysLog.Dtos
{
    public class SqlCurdAddInput
    {
        public string FullName { get; set; }

        public long ExecuteMilliseconds { get; set; }

        public string Sql { get; set; }
    }
}