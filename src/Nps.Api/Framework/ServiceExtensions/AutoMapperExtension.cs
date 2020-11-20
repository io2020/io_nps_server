using AutoMapper;
using Nps.Core.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Reflection;

namespace Nps.Api.Framework.ServiceExtensions
{
    /// <summary>
    /// IServiceCollection扩展-实体映射对象
    /// </summary>
    public static partial class AutoMapperExtension
    {
        /// <summary>
        /// 注入AutoMapper
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        public static void AddDefineAutoMapper(this IServiceCollection services)
        {
            Log.Logger.Information("Initialize AutoMapper Start;");

            Check.NotNull(services, nameof(services));

            var autoMapperAssemblys = Assembly.Load("Nps.Application");
            services.AddAutoMapper(autoMapperAssemblys);

            Log.Logger.Information("Initialize AutoMapper End;");
        }
    }
}