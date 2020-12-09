using Microsoft.AspNetCore.Builder;
using Nps.Api.Extension.Middleware;
using Nps.Infrastructure;

namespace Nps.Api.Extension.Application
{
    /// <summary>
    /// IApplicationBuilder扩展-跨域
    /// </summary>
    public static partial class CorsExtension
    {
        /// <summary>
        /// 使用跨域
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        public static void UseDefineCors(this IApplicationBuilder app)
        {
            Check.NotNull(app, nameof(app));

            app.UseMiddleware<CorsMiddleware>();
        }
    }
}