using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Nps.Infrastructure;
using Serilog;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Nps.Api.Extension.Service
{
    /// <summary>
    /// IServiceCollection扩展-Swagger文档展示
    /// </summary>
    public static class SwaggerExtension
    {
        /// <summary>
        /// 注入Swagger，使用jwt授权方案
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        public static void AddDefineSwagger(this IServiceCollection services)
        {
            Log.Logger.Information("Initialize Swagger Start;");

            Check.NotNull(services, nameof(services));

            string ApiName = "Nps.WebApi";
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("nps", new OpenApiInfo
                {
                    Version = "nps",
                    Title = $"{ApiName}--接口文档--{ RuntimeInformation.FrameworkDescription}",//标题
                    Description = $"{ApiName} 接口文档"//简介
                });

                //controller注释;必须放最后,否则后面的会覆盖前面的
                string xmlPath = Path.Combine(AppContext.BaseDirectory, "Nps.Api.xml");
                options.IncludeXmlComments(xmlPath, true);
                //实体层的xml文件名
                string xmlEntityPath = Path.Combine(AppContext.BaseDirectory, "Nps.Data.xml");
                options.IncludeXmlComments(xmlEntityPath);
                //Dto所在类库
                string applicationPath = Path.Combine(AppContext.BaseDirectory, "Nps.Application.xml");
                options.IncludeXmlComments(applicationPath);
                //输出所在类库
                string corePath = Path.Combine(AppContext.BaseDirectory, "Nps.Core.xml");
                options.IncludeXmlComments(corePath);
                string infrastructurePath = Path.Combine(AppContext.BaseDirectory, "Nps.Infrastructure.xml");
                options.IncludeXmlComments(infrastructurePath);

                //添加一个必须的全局安全信息，和AddSecurityDefinition方法指定的方案名称要一致，这里是Bearer。
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference=new OpenApiReference
                            {
                                Id="Bearer",
                                Type= ReferenceType.SecurityScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                });
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 参数结构: \"Bearer {token}\"",
                    Name = "Authorization", //jwt默认的参数名称
                    In = ParameterLocation.Header, //jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey
                });

                //重新赋值OperationId
                options.CustomOperationIds(apiDesc =>
                {
                    var controllerAction = apiDesc.ActionDescriptor as ControllerActionDescriptor;
                    return $"{controllerAction.ControllerName}-{controllerAction.ActionName}";
                });
            });

            Log.Logger.Information("Initialize Swagger End;");
        }
    }
}