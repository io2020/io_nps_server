using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Nps.Core.Aop.Attributes;
using Nps.Core.Data;
using Nps.Infrastructure.Extensions;
using Nps.Core.Repositories;
using Nps.Data.Entities;
using Serilog;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Nps.Api.Extension.Aop
{
    public class ActionTrackFilterAttribute : ActionFilterAttribute
    {
        private readonly IDiagnosticContext _diagnosticContext;

        private readonly IFreeSqlRepository<ActionTrackLog> _actionTrackLogRepository;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public ActionTrackFilterAttribute(
            IDiagnosticContext diagnosticContext,
            IFreeSqlRepository<ActionTrackLog> actionTrackLogRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _diagnosticContext = diagnosticContext;
            _actionTrackLogRepository = actionTrackLogRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (SkipLogging(context))
            {
                return base.OnActionExecutionAsync(context, next);
            }

            #region Serilog摘要日志

            _diagnosticContext.Set("ActionArguments", context.ActionArguments.ToJson());
            _diagnosticContext.Set("RouteData", context.ActionDescriptor.RouteValues);
            _diagnosticContext.Set("ActionName", context.ActionDescriptor.DisplayName);
            _diagnosticContext.Set("ActionId", context.ActionDescriptor.Id);
            _diagnosticContext.Set("ValidationState", context.ModelState.IsValid);

            #endregion

            #region 接口审计日志

            var input = new ActionTrackLog
            {
                HostDomain = $"{context.HttpContext.Request.Scheme}://{context.HttpContext.Request.Host.Value}",
                ApiMethod = context.HttpContext.Request.Method,
                ApiPath = context.HttpContext.Request.Path.Value,
                ApiParams = context.ActionArguments.ToJson(),
                UserAgent = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"],
                IP = GetIp(_httpContextAccessor?.HttpContext?.Request)
            };

            //接口执行耗时
            var watch = new Stopwatch();
            watch.Start();
            var actionExecutedContext = next();
            watch.Stop();
            input.ExecuteMilliseconds = watch.ElapsedMilliseconds;
            input.StatusCode = context.HttpContext.Response.StatusCode;

            if (actionExecutedContext.Result.Exception != null)
            {
                input.ExecuteMessage = actionExecutedContext.Result.Exception.Message;
            }
            else
            {
                if (actionExecutedContext.Result.Result is ObjectResult result && result.Value is IExecuteResult res)
                {
                    input.ExecuteResult = res.ToJson();
                    input.ExecuteMessage = res.Message;
                }
            }

            _actionTrackLogRepository.InsertAsync(input);

            #endregion

            return Task.CompletedTask;
        }

        /// <summary>
        /// 当方法或控制器上存在DisableAuditingAttribute特性标签时，不记录日志 
        /// </summary>
        private static bool SkipLogging(ActionExecutingContext context)
        {
            return context.ActionDescriptor is ControllerActionDescriptor d && d.MethodInfo.IsDefined(typeof(DisableAuditingAttribute), true)
                || context.Controller.GetType().IsDefined(typeof(DisableAuditingAttribute), true);
        }

        private static string GetIp(HttpRequest request)
        {
            if (request == null)
            {
                return "";
            }

            string ip = request.Headers["X-Real-IP"].FirstOrDefault();
            if (ip.IsNull())
            {
                ip = request.Headers["X-Forwarded-For"].FirstOrDefault();
            }
            if (ip.IsNull())
            {
                ip = request.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            }
            if (ip.IsNull() || !ip.Split(":")[0].IsValidIP())
            {
                ip = "127.0.0.1";
            }

            return ip;
        }
    }
}