using Autofac.Extensions.DependencyInjection;
using Nps.Core.Infrastructure.Configs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace Nps.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(AppSettings.Load())
                .Enrich.FromLogContext()
                .CreateLogger();

            try
            {
                Log.Information("Initialize Main");

                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //自定义发布端口
                    webBuilder.UseUrls("http://*:7001");//http://*:7001;https://*:7002
                    webBuilder.UseStartup<Startup>();
                }).UseSerilog(dispose: true);
        }
    }
}