using Nps.Core.Infrastructure;
using Nps.Core.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Nps.Api.Framework.ServiceExtensions
{
    /// <summary>
    /// IServiceCollection扩展-当前用户
    /// </summary>
    public static partial class CurrentUserExtension
    {
        /// <summary>
        /// 注入当前登录用户，提前注入
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        public static void AddCurrentUser(this IServiceCollection services)
        {
            Log.Logger.Information("Initialize CurrentUser Start;");

            Check.NotNull(services, nameof(services));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ICurrentUser, CurrentUser>();

            Log.Logger.Information("Initialize CurrentUser End;");
        }
    }
}