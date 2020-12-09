using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Nps.Api.Extension.Aop;
using Nps.Core.Data;
using Nps.Infrastructure;
using Serilog;
using System.Reflection;

namespace Nps.Api.Extension.Service
{
    /// <summary>
    /// IServiceCollection扩展-MvcControllers、FluentValidation
    /// </summary>
    public static partial class MvcControllersExtension
    {
        /// <summary>
        /// 注入MvcControllers、FluentValidation
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        public static void AddDefineControllers(this IServiceCollection services)
        {
            Log.Logger.Information("Initialize MvcControllers Start;");

            Check.NotNull(services, nameof(services));

            services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
                options.Filters.Add<ActionTrackFilterAttribute>();
                //禁止去除ActionAsync后缀
                options.SuppressAsyncSuffixInActionNames = false;
            })
            //添加FluentValidation模型验证组件
            .AddFluentValidation(config =>
            {
                //只使用FluentValidation模型参数验证规则
                config.RunDefaultMvcValidationAfterFluentValidationExecutes = false;

                //从dll文件中，注入所有模型验证实现类
                var validationAssembly = Assembly.Load("Nps.Application");
                config.RegisterValidatorsFromAssembly(validationAssembly);
            })
            .AddNewtonsoftJson(options =>
            {
                //忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //使用驼峰 首字母小写
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                //设置时间格式
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            })
            .ConfigureApiBehaviorOptions(options =>
            {//统一返回模型验证的信息 ExecuteResult为全局统一Api返回结果
                options.InvalidModelStateResponseFactory = context =>
                {//自定义 BadRequest 响应
                    var problemDetails = new ValidationProblemDetails(context.ModelState);
                    var executeResult = ExecuteResult.Error("参数错误", StatusCode.ParameterError, problemDetails.Errors);

                    return new BadRequestObjectResult(executeResult)
                    {
                        ContentTypes = { "application/json" }
                    };
                };
            })
            //在Controller中使用属性注入
            .AddControllersAsServices();

            Log.Logger.Information("Initialize MvcControllers End;");
        }
    }
}