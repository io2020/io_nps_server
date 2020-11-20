using DotNetCore.Security;
using Nps.Core.Data;
using Nps.Core.Infrastructure;
using Nps.Core.Infrastructure.Configs;
using Nps.Core.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Nps.Api.Framework.ServiceExtensions
{
    /// <summary>
    /// IServiceCollection扩展-JWT认证
    /// </summary>
    public static class JwtTokenExtension
    {
        /// <summary>
        /// 根据配置文件创建JsonWebTokenSettings
        /// 默认过期时间为1天
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <returns>JsonWebTokenSettings</returns>
        public static JsonWebTokenSettings AddSecurity(this IServiceCollection services)
        {
            var jsonWebTokenSettings = new JsonWebTokenSettings(
                            AppSettings.Get(new string[] { "Authentication", "JwtBearer", "SecurityKey" }),
                            new TimeSpan(1, 0, 0, 0),
                            AppSettings.Get(new string[] { "Authentication", "JwtBearer", "Audience" }),
                            AppSettings.Get(new string[] { "Authentication", "JwtBearer", "Issuer" })
                       );
            services.AddHash();
            services.AddCryptography(AppSettings.Get(new string[] { "Authentication", "JwtBearer", "Cryptography" }));
            services.AddJsonWebToken(jsonWebTokenSettings);
            return jsonWebTokenSettings;
        }

        /// <summary>
        /// 注入JWT
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        public static void AddJwtBearer(this IServiceCollection services)
        {
            Log.Logger.Information("Initialize JwtBearer Start;");

            Check.NotNull(services, nameof(services));

            JsonWebTokenSettings jsonWebTokenSettings = services.AddSecurity();
            // 开启Bearer认证
            services.AddAuthentication(opts =>
            {
                opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            // 添加JwtBearer服务
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                //令牌验证参数
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    //是否验证密钥
                    ValidateIssuerSigningKey = true,
                    //密钥
                    IssuerSigningKey = jsonWebTokenSettings.SecurityKey,

                    //是否验证发行人
                    ValidateIssuer = true,
                    ValidIssuer = jsonWebTokenSettings.Issuer,

                    //是否验证受众人
                    ValidateAudience = true,
                    ValidAudience = jsonWebTokenSettings.Audience,

                    //验证生命周期
                    ValidateLifetime = true,

                    //过期时间
                    RequireExpirationTime = true,

                    // 允许服务器时间偏移量300秒，即我们配置的过期时间加上这个允许偏移的时间值，才是真正过期的时间(过期时间 +偏移值)你也可以设置为0
                    //ClockSkew = TimeSpan.Zero
                };

                //使用Authorize设置为需要登录时，返回json格式数据。
                options.Events = new JwtBearerEvents()
                {
                    //授权失败
                    OnAuthenticationFailed = context =>
                    {
                        //Token expired
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }

                        return Task.CompletedTask;
                    },
                    //未授权
                    OnChallenge = async context =>
                    {
                        //此处代码为终止.Net Core默认的返回类型和数据结果，这个很重要哦
                        context.HandleResponse();

                        string message;
                        StatusCode status;
                        int statusCode = StatusCodes.Status401Unauthorized;

                        if (context.Error == "invalid_token" && context.ErrorDescription == "The token is expired")
                        {
                            message = "令牌过期";
                            status = StatusCode.TokenExpired;
                            statusCode = StatusCodes.Status422UnprocessableEntity;
                        }
                        else if (context.Error == "invalid_token" && string.IsNullOrWhiteSpace(context.ErrorDescription))
                        {
                            message = "令牌失效";
                            status = StatusCode.TokenInvalidation;
                        }
                        else
                        {
                            message = "请先登录 " + context.ErrorDescription; //""认证失败，请检查请求头或者重新登录";
                            status = StatusCode.AuthenticationFailed;
                        }

                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = statusCode;
                        await context.Response.WriteAsync(ExecuteResult.Error(message, status).ToJson());
                    }
                };
            });

            Log.Logger.Information("Initialize JwtBearer End;");
        }
    }
}