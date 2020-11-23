using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace Nps.Core.Infrastructure.Configs
{
    /// <summary>
    /// 配置帮助类
    /// </summary>
    public class AppSettings
    {
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
        /// <returns>返回IConfiguration</returns>
        public static IConfiguration Load()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)//设置基础路径
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);//添加配置文件

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
            if (sections.Any())
            {
                return Configuration[string.Join(":", sections)] ?? "";
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
            if (sections.Any())
            {
                return Configuration.GetValue<T>(string.Join(":", sections)) ?? default;
            }
            return default;
        }

        /// <summary>
        /// 获取系统环境变量值
        /// </summary>
        /// <param name="environmentName">环境变量名称</param>
        /// <returns>返回系统环境变量值</returns>
        public static string Get(string environmentName)
        {
            if (environmentName.IsNotNullOrWhiteSpace())
            {
                return Environment.GetEnvironmentVariable(environmentName, EnvironmentVariableTarget.Machine) ?? "";
            }
            return "";
        }
    }
}