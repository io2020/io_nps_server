using Nps.Infrastructure;
using Nps.Core.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;

namespace Nps.Api.Extension.Application
{
    /// <summary>
    /// IApplicationBuilder扩展-Serilog高级用法
    /// </summary>
    public static partial class SerilogExtension
    {
        /// <summary>
        /// 定义Serilog摘要日志
        /// </summary>
        /// <param name="diagnosticContext">IDiagnosticContext</param>
        /// <param name="httpContext">HttpContext</param>
        public static void EnrichFromRequest(IDiagnosticContext diagnosticContext, HttpContext httpContext)
        {
            var request = httpContext.Request;
            ICurrentUser currentUser = (ICurrentUser)httpContext.RequestServices.GetService(typeof(ICurrentUser));
            if (currentUser != null)
            {
                diagnosticContext.Set("UserName", currentUser.UserName);
                diagnosticContext.Set("UserId", currentUser.UserId);
            }
            // et all the common properties available for every request
            diagnosticContext.Set("Host", request.Host);
            diagnosticContext.Set("Protocol", request.Protocol);
            diagnosticContext.Set("Scheme", request.Scheme);

            // Only set it if available. You're not sending sensitive data in a querystring right?!
            if (request.QueryString.HasValue)
            {
                diagnosticContext.Set("QueryString", request.QueryString.Value);
            }

            // Set the content-type of the Response at this point
            diagnosticContext.Set("ContentType", httpContext.Response.ContentType);

            // Retrieve the IEndpointFeature selected for the request
            var endpoint = httpContext.GetEndpoint();
            if (endpoint != null)
            {
                diagnosticContext.Set("EndpointName", endpoint.DisplayName);
            }
        }

        /// <summary>
        /// 使用Serilog摘要日志
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        public static void UseSerilogRequestLog(this IApplicationBuilder app)
        {
            Check.NotNull(app, nameof(app));

            //使用Serilog摘要日志
            app.UseSerilogRequestLogging(opts =>
            {
                opts.EnrichDiagnosticContext = EnrichFromRequest;
                opts.GetLevel = (ctx, _, ex) =>
                {
                    return ex != null || ctx.Response.StatusCode > 499 ? LogEventLevel.Error : LogEventLevel.Information;
                };
            });
        }
    }
}