using Autofac;
using Autofac.Extras.DynamicProxy;
using Nps.Application.SysLog.Services;
using Nps.Core.Aop.Interceptor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nps.Api.Framework.DependencyRegister
{
    /// <summary>
    /// 注入服务
    /// </summary>
    public class ServiceRegisterModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWorkInterceptor>();
            builder.RegisterType<UnitOfWorkAsyncInterceptor>();

            List<Type> interceptorServiceTypes = new List<Type>()
            {
                typeof(UnitOfWorkInterceptor)
            };

            //跳过的服务列表
            string[] notIncludes = new string[]
            {
                typeof(SqlCurdService).Name,
            };

            Assembly serviceAssemblys = Assembly.Load("Nps.Application");
            builder.RegisterAssemblyTypes(serviceAssemblys)
                .Where(a => a.Name.EndsWith("Service") && !notIncludes.Where(r => r == a.Name).Any())
                .PropertiesAutowired()//开始属性注入
                .AsImplementedInterfaces()//自动以其实现的所有接口类型暴露，以接口的方式注册不包括IDisposable接口
                .InstancePerLifetimeScope()//即为每一个依赖或调用创建一个单一的共享的实例
                .InterceptedBy(interceptorServiceTypes.ToArray())
                .EnableInterfaceInterceptors();//引用Autofac.Extras.DynamicProxy,使用接口的拦截器，在使用特性 [Attribute] 注册时，注册拦截器可注册到接口(Interface)上或其实现类(Implement)上。使用注册到接口上方式，所有的实现类都能应用到拦截器。
        }
    }
}