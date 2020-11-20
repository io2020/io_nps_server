using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nps.Core.Data;
using Nps.Core.Infrastructure;
using Nps.Core.Infrastructure.Exceptions;
using Nps.Core.Infrastructure.Extensions;
using System;
using System.Threading.Tasks;

namespace Nps.Api.Framework.Aop
{
    public class GlobalExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger _logger;

        private readonly IWebHostEnvironment _environment;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.Exception is NpsException exception)
            {
                var warnResponse = ExecuteResult.Error("", exception.GetErrorCode(), exception.Message);
                _logger.LogWarning(warnResponse.ToJson());
                HandlerException(context, warnResponse, exception.GetCode());
                return Task.CompletedTask;
            }

            string error = "异常信息：";

            void ReadException(Exception ex)
            {
                error += $"{ex.Message} | {ex.StackTrace} | {ex.InnerException}";
                if (ex.InnerException != null)
                {
                    ReadException(ex.InnerException);
                }
            }

            ReadException(context.Exception);

            _logger.LogError(error);

            var apiResponse = ExecuteResult.Error(_environment.IsDevelopment() ? error : "服务器正忙，请稍后再试.", StatusCode.UnknownError);

            HandlerException(context, apiResponse, StatusCodes.Status500InternalServerError);

            return Task.CompletedTask;
        }

        private static void HandlerException(ExceptionContext context, IExecuteResult apiResponse, int statusCode)
        {
            context.Result = new JsonResult(apiResponse)
            {
                StatusCode = statusCode,
                ContentType = "application/json",
            };
            context.ExceptionHandled = true;
        }
    }
}