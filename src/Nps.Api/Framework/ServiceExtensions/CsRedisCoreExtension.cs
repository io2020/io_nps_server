using CSRedis;
using Microsoft.Extensions.DependencyInjection;
using Nps.Core.Infrastructure;
using Serilog;

namespace Nps.Api.Framework.ServiceExtensions
{
    /// <summary>
    /// IServiceCollection扩展-Redis
    /// </summary>
    public static class CsRedisCoreExtension
    {
        /// <summary>
        /// 注入Redis
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        public static void AddDefineRedis(this IServiceCollection services)
        {
            Log.Logger.Information("Initialize CSRedisCore Start;");

            Check.NotNull(services, nameof(services));

            if (NpsEnvironment.NPS_DB_ISUSEDREDIS.ToBooleanOrDefault(false))
            {
                //初始化 CSRedisClient
                CSRedisClient csRedisClient = new CSRedisClient(NpsEnvironment.NPS_DB_REDISCONNECTSTRING);
                //初始化 RedisHelper
                RedisHelper.Initialization(csRedisClient);
            }

            Log.Logger.Information("Initialize CSRedisCore End;");
        }
    }
}