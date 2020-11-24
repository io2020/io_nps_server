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
            //ע�뵱ǰ�û�
            services.AddCurrentUser();
            //ע��ID������
            services.AddIdGenerator();
            //ע��FreeSql
            services.AddFreeSql();
            //ע��Cache
            services.AddDefineCache();
            //ע��Jwt��֤
            services.AddJwtBearer();
            //ע�����ӳ��
            services.AddDefineAutoMapper();
            //ע��Mvc
            services.AddDefineControllers();
            //Swagger
            services.AddDefineSwagger();
            //ע��HttpClient
            services.AddHttpClient();
            //ע��MiniProfiler
            services.AddDefineMiniProfiler();
            // ע���Զ���WebApi
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
            //ʹ��Swagger
            app.UseDefineSwagger(() => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("Nps.Api.index.html"));
            //ʹ�ÿ���
            app.UseDefineCors();
            // ʹ�þ�̬�ļ�
            app.UseStaticFiles();
            //ʹ��SerilogժҪ��־
            app.UseSerilogRequestLog();
            //����·��
            app.UseRouting();
            //1.�ȿ�����֤
            app.UseAuthentication();
            //2.�ٿ�����Ȩ
            app.UseAuthorization();
            //ʹ��MiniProfiler�����������app.UseMvc֮ǰ
            app.UseMiniProfiler();
            //Endpoint
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}