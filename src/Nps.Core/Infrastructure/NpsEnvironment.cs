using System;

namespace Nps.Core.Infrastructure
{
    /// <summary>
    /// Nps环境变量常量
    /// </summary>
    public class NpsEnvironment
    {
        /// <summary>
        /// 数据库是否同步表结构
        /// </summary>
        public static string NPS_DB_SYNCSTRUCTURE => Get("NPS_DB_SYNCSTRUCTURE", "true");

        /// <summary>
        /// 数据库是否同步数据
        /// </summary>
        public static string NPS_DB_SYNCDATA => Get("NPS_DB_SYNCDATA", "true");

        /// <summary>
        /// 数据库类型 MySql = 0, SqlServer = 1
        /// </summary>
        public static string NPS_DB_DATETYPE => Get("NPS_DB_DATETYPE", "0");

        /// <summary>
        /// 数据库主库连接字符串
        /// </summary>
        public static string NPS_DB_MASTERCONNECTSTRING => Get("NPS_DB_MASTERCONNECTSTRING", "Data Source=rm-2zel8sk911c1l1g6a8o.mysql.rds.aliyuncs.com;Port=3306;User ID=ionps;Password=ioNPS2020; Initial Catalog=ionps;Charset=utf8mb4; SslMode=none;Min pool size=1");

        /// <summary>
        /// 数据库从库连接字符串
        /// </summary>
        public static string NPS_DB_SLAVECONNECTSTRING => Get("NPS_DB_SLAVECONNECTSTRING", "Data Source=cpftu7fes6yg2ar7rn2j-rw4rm.rwlb.rds.aliyuncs.com;Port=3306;User ID=ionps;Password=ioNPS2020; Initial Catalog=ionps;Charset=utf8mb4; SslMode=none;Min pool size=1");

        /// <summary>
        /// 是否启用Redis
        /// </summary>
        public static string NPS_DB_ISUSEDREDIS => Get("NPS_DB_ISUSEDREDIS", "true");

        /// <summary>
        /// Redis连接字符串
        /// </summary>
        public static string NPS_DB_REDISCONNECTSTRING => Get("NPS_DB_REDISCONNECTSTRING", "r-m5ep2cn21tb7lvk0hxpd.redis.rds.aliyuncs.com:6379,password=soonSmart_Redis,defaultDatabase=11");

        /// <summary>
        /// JwtBearer-SecurityKey
        /// </summary>
        public static string NPS_AUTH_JWT_SECURITYKEY => Get("NPS_AUTH_JWT_SECURITYKEY", "nps-dotnetfive-SecurityKey");

        /// <summary>
        /// JwtBearer-发行人
        /// </summary>
        public static string NPS_AUTH_JWT_ISSUER => Get("NPS_AUTH_JWT_ISSUER", "nps-dotnetfive-Issuer");

        /// <summary>
        /// JwtBearer-受众人
        /// </summary>
        public static string NPS_AUTH_JWT_AUDIENCE => Get("NPS_AUTH_JWT_AUDIENCE", "Nps.Api");

        /// <summary>
        /// JwtBearer-CRYPTOGRAPHY
        /// </summary>
        public static string NPS_AUTH_JWT_CRYPTOGRAPHY => Get("NPS_AUTH_JWT_CRYPTOGRAPHY", "nps-dotnetfive-cryptography");

        /// <summary>
        /// 雪花ID生成器数据中心ID，取值0-31
        /// </summary>
        public static string NPS_IDGENERATOR_DATACENTERID => Get("NPS_IDGENERATOR_DATACENTERID", "11");

        /// <summary>
        /// 雪花ID生成器工作机器ID，取值0-31
        /// </summary>
        public static string NPS_IDGENERATOR_WORKEID => Get("NPS_IDGENERATOR_WORKEID", "11");

        /// <summary>
        /// NPS远程服务器地址
        /// </summary>
        public static string NPS_REMOTEHOST => Get("NPS_REMOTEHOST", "http://8.131.77.125:7501/");

        /// <summary>
        /// 获取系统环境变量值
        /// </summary>
        /// <param name="environmentName">环境变量名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>返回系统环境变量值</returns>
        public static string Get(string environmentName, string defaultValue)
        {
            var environmentValue = "";
            if (environmentName.IsNotNullOrWhiteSpace())
            {
                environmentValue = Environment.GetEnvironmentVariable(environmentName, EnvironmentVariableTarget.Machine);
            }
            return environmentValue.IsNotNullOrWhiteSpace() ? environmentValue : defaultValue;
        }
    }
}