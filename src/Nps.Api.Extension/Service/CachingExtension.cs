using CSRedis;
using Microsoft.Extensions.DependencyInjection;
using Nps.Core.Caching;
using Nps.Infrastructure;
using Serilog;

namespace Nps.Api.Extension.Service
{
    /// <summary>
    /// IServiceCollection扩展-缓存
    /// </summary>
    public static class CachingExtension
    {
        /// <summary>
        /// 注入自定义缓存
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        public static void AddDefineCache(this IServiceCollection services)
        {
            Log.Logger.Information("Initialize Caching Start;");

            Check.NotNull(services, nameof(services));

            if (NpsEnvironment.NPS_DB_ISUSEDREDIS.ToBooleanOrDefault(false))
            {
                //初始化 CSRedisClient
                CSRedisClient csRedisClient = new CSRedisClient(NpsEnvironment.NPS_DB_REDISCONNECTSTRING);
                //初始化 RedisHelper
                RedisHelper.Initialization(csRedisClient);

                services.AddSingleton<ICaching, RedisCache>();
            }
            else
            {
                services.AddSingleton<ICaching, MemoryCache>();
            }

            Log.Logger.Information("Initialize Caching End;");
        }
    }
}