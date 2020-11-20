using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;

namespace Nps.Core.Infrastructure.Configs
{
    /// <summary>
    /// 配置帮助类
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// 输出内容
        /// </summary>
        public static Action<string> WriteLine { get; set; } = Console.WriteLine;

        /// <summary>
        /// 应用程序配置属性
        /// </summary>
        public static IConfiguration Configuration { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="configuration">应用程序配置属性</param>
        public AppSettings(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="configFileName">配置文件名。默认：appsettings.json</param>
        /// <param name="basePath">基路径</param>
        /// <returns>返回配置文件信息</returns>
        public static IConfiguration Load(string configFileName = "appsettings.json", string basePath = "")
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            basePath = basePath.IsNullOrWhiteSpace() ? AppContext.BaseDirectory : Path.Combine(AppContext.BaseDirectory, basePath);

            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)//设置基础路径
                .AddJsonFile(configFileName, optional: false, reloadOnChange: true);//添加配置文件

            if (environmentName.IsNotNullOrWhiteSpace())
            {
                builder.AddJsonFile($"appsettings.{environmentName}.json", optional: true);//添加配置文件
            }

            return builder
                .AddEnvironmentVariables()//支持环境变量
                .Build();
        }

        /// <summary>
        /// 获取节点值
        /// </summary>
        /// <param name="sections">节点参数配置</param>
        /// <returns>返回节点值</returns>
        public static string Get(params string[] sections)
        {
            try
            {
                if (sections.Any())
                {
                    return Configuration[string.Join(":", sections)];
                }
            }
            catch (Exception ex)
            {
                WriteLine($"获取配置文件配置项错误，参数：{sections.SerializeJson()}。[Exception]:{ex}");
            }

            return "";
        }

        /// <summary>
        /// 获取节点值
        /// </summary>
        /// <typeparam name="T">泛型，值类型</typeparam>
        /// <param name="sections">节点参数配置</param>
        /// <returns>返回节点值</returns>
        public static T Get<T>(params string[] sections)
        {
            try
            {
                if (sections.Any())
                {
                    return Configuration.GetValue<T>(string.Join(":", sections));
                }
            }
            catch (Exception ex)
            {
                WriteLine($"获取配置文件配置项错误，参数：{sections.SerializeJson()}。[Exception]:{ex}");
            }

            return default;
        }

        /// <summary>
        /// 获取节点值
        /// </summary>
        /// <param name="sections">节点参数配置</param>
        /// <returns>返回节点值</returns>
        public static string Get(string sections)
        {
            try
            {
                if (sections.IsNotNullOrWhiteSpace())
                {
                    return Configuration[sections];
                }
            }
            catch (Exception ex)
            {
                WriteLine($"获取配置文件配置项错误，参数：{sections.SerializeJson()}。[Exception]:{ex}");
            }

            return "";
        }

        /// <summary>
        /// 获取节点值
        /// </summary>
        /// <typeparam name="T">泛型，值类型</typeparam>
        /// <param name="sections">节点参数配置</param>
        /// <returns>返回节点值</returns>
        public static T Get<T>(string sections)
        {
            try
            {
                if (sections.IsNotNullOrWhiteSpace())
                {
                    return Configuration.GetValue<T>(sections);
                }
            }
            catch (Exception ex)
            {
                WriteLine($"获取配置文件配置项错误，参数：{sections.SerializeJson()}。[Exception]:{ex}");
            }

            return default;
        }
    }
}