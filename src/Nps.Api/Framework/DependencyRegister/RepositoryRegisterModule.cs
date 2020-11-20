using Autofac;
using Nps.Core.Repositories;
using System.Reflection;

namespace Nps.Api.Framework.DependencyRegister
{
    /// <summary>
    /// 注入仓储
    /// </summary>
    public class RepositoryRegisterModule : Autofac.Module
    {
        //注册Repository
        protected override void Load(ContainerBuilder builder)
        {
            Assembly repositoryAssemblys = Assembly.Load("Nps.Data");
            builder.RegisterAssemblyTypes(repositoryAssemblys)
                    .Where(a => a.Name.EndsWith("Repository"))
                    .AsImplementedInterfaces()//自动以其实现的所有接口类型暴露，以接口的方式注册不包括IDisposable接口
                    .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(FreeSqlRepository<>)).As(typeof(IFreeSqlRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(FreeSqlRepository<,>)).As(typeof(IFreeSqlRepository<,>)).InstancePerLifetimeScope();
        }
    }
}