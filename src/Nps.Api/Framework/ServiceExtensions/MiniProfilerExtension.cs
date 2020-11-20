using Nps.Core.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Nps.Api.Framework.ServiceExtensions
{
    /// <summary>
    /// IServiceCollection扩展-MiniProfiler接口性能监控
    /// https://www.cnblogs.com/lwqlun/p/10222505.html
    /// </summary>
    public static partial class MiniProfilerExtension
    {
        /// <summary>
        /// 注入MiniProfiler
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        public static void AddDefineMiniProfiler(this IServiceCollection services)
        {
            Log.Logger.Information("Initialize MiniProfiler Start;");

            Check.NotNull(services, nameof(services));

            services.AddMiniProfiler(options =>
            {
                options.RouteBasePath = "/profiler";
            });

            Log.Logger.Information("Initialize MiniProfiler End;");
        }
    }
}