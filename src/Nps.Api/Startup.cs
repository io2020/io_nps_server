using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nps.Api.Framework.ApplicationExtensions;
using Nps.Api.Framework.DependencyRegister;
using Nps.Api.Framework.ServiceExtensions;
using Nps.Core.Infrastructure.Configs;
using System.Reflection;

namespace Nps.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; } = AppSettings.Load();

        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //注入当前用户
            services.AddCurrentUser();
            //注入ID生成器
            services.AddIdGenerator();
            //注入FreeSql
            services.AddFreeSql();
            //注入Cache
            services.AddDefineCache();
            //注入Jwt认证
            services.AddJwtBearer();
            //注入对象映射
            services.AddDefineAutoMapper();
            //注入Mvc
            services.AddDefineControllers();
            //Swagger
            services.AddDefineSwagger();
            //注入HttpClient
            services.AddHttpClient();
            //注入MiniProfiler
            services.AddDefineMiniProfiler();
            // 注入自定义WebApi
            services.AddDefineHttpApi();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new DefaultRegisterModule(Configuration));
            builder.RegisterModule(new RepositoryRegisterModule());
            builder.RegisterModule(new ServiceRegisterModule());
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //使用Swagger
            app.UseDefineSwagger(() => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("Nps.Api.index.html"));
            //使用跨域
            app.UseDefineCors();
            // 使用静态文件
            app.UseStaticFiles();
            //使用Serilog摘要日志
            app.UseSerilogRequestLog();
            //开启路由
            app.UseRouting();
            //1.先开启认证
            app.UseAuthentication();
            //2.再开启授权
            app.UseAuthorization();
            //使用MiniProfiler，必须放置在app.UseMvc之前
            app.UseMiniProfiler();
            //Endpoint
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}