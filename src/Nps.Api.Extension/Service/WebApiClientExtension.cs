using Microsoft.Extensions.DependencyInjection;
using Nps.Application.NpsApi;
using Nps.Infrastructure;
using Serilog;
using System;
using WebApiClient;

namespace Nps.Api.Extension.Service
{
    /// <summary>
    /// IServiceCollection扩展-WebApiClient文档展示
    /// </summary>
    public static class WebApiClientExtension
    {
        /// <summary>
        /// 注入自定义WebApi
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        public static void AddDefineHttpApi(this IServiceCollection services)
        {
            Log.Logger.Information("Initialize DefineHttpApi Start;");

            Check.NotNull(services, nameof(services));

            services.AddHttpApi<INpsApi>(option =>
            {
                option.HttpHost = new Uri(NpsEnvironment.NPS_REMOTEHOST);
                option.FormatOptions = new FormatOptions { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            });

            Log.Logger.Information("Initialize DefineHttpApi End;");
        }
    }
}