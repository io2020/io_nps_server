using Autofac;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Reflection;

namespace Nps.Api.Framework.DependencyRegister
{
    /// <summary>
    /// 基础注入
    /// </summary>
    public class DefaultRegisterModule : Autofac.Module
    {
        public IConfiguration _configuration;

        public DefaultRegisterModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            //注入配置文件
            builder.RegisterInstance(_configuration).AsImplementedInterfaces().PropertiesAutowired().SingleInstance();

            //获取所有控制器类型并使用属性注入
            //需要在Startup文件中添加 services.AddControllers().AddControllersAsServices();作为属性注入
            //在控制器中使用属性依赖注入，其中注入属性必须标注为public
            Assembly controllerAssemblys = Assembly.Load("Nps.Api");
            var controllerBaseType = typeof(ControllerBase);
            builder.RegisterAssemblyTypes(controllerAssemblys)
                .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
                .PropertiesAutowired();
        }
    }
}