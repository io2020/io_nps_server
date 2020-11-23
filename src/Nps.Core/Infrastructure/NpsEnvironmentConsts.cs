namespace Nps.Core.Infrastructure
{
    /// <summary>
    /// Nps环境变量常量
    /// </summary>
    public class NpsEnvironmentConsts
    {
        /// <summary>
        /// 数据库是否同步表结构
        /// </summary>
        public const string NPS_DB_SYNCSTRUCTURE = "NPS_DB_SYNCSTRUCTURE";

        /// <summary>
        /// 数据库是否同步数据
        /// </summary>
        public const string NPS_DB_SYNCDATA = "NPS_DB_SYNCDATA";

        /// <summary>
        /// 数据库类型 MySql = 0, SqlServer = 1
        /// </summary>
        public const string NPS_DB_DATETYPE = "NPS_DB_DATETYPE";

        /// <summary>
        /// 数据库主库连接字符串
        /// </summary>
        public const string NPS_DB_MASTERCONNECTSTRING = "NPS_DB_MASTERCONNECTSTRING";

        /// <summary>
        /// 数据库从库连接字符串
        /// </summary>
        public const string NPS_DB_SLAVECONNECTSTRING = "NPS_DB_SLAVECONNECTSTRING";

        /// <summary>
        /// 是否启用Redis
        /// </summary>
        public const string NPS_DB_ISUSEDREDIS = "NPS_DB_ISUSEDREDIS";

        /// <summary>
        /// Redis连接字符串
        /// </summary>
        public const string NPS_DB_REDISCONNECTSTRING = "NPS_DB_REDISCONNECTSTRING";

        /// <summary>
        /// JwtBearer-SecurityKey
        /// </summary>
        public const string NPS_AUTH_JWT_SECURITYKEY = "NPS_AUTH_JWT_SECURITYKEY";

        /// <summary>
        /// JwtBearer-发行人
        /// </summary>
        public const string NPS_AUTH_JWT_ISSUER = "NPS_AUTH_JWT_ISSUER";

        /// <summary>
        /// JwtBearer-受众人
        /// </summary>
        public const string NPS_AUTH_JWT_AUDIENCE = "NPS_AUTH_JWT_AUDIENCE";

        /// <summary>
        /// JwtBearer-CRYPTOGRAPHY
        /// </summary>
        public const string NPS_AUTH_JWT_CRYPTOGRAPHY = "NPS_AUTH_JWT_CRYPTOGRAPHY";

        /// <summary>
        /// 雪花ID生成器数据中心ID，取值0-31
        /// </summary>
        public const string NPS_IDGENERATOR_DATACENTERID = "NPS_IDGENERATOR_DATACENTERID";

        /// <summary>
        /// 雪花ID生成器工作机器ID，取值0-31
        /// </summary>
        public const string NPS_IDGENERATOR_WORKEID = "NPS_IDGENERATOR_WORKEID";

        /// <summary>
        /// NPS远程服务器地址
        /// </summary>
        public const string NPS_REMOTEHOST = "NPS_REMOTEHOST";
    }
}