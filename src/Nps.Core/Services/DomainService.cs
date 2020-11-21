using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Nps.Core.Security;
using System;

namespace Nps.Core.Services
{
    /// <summary>
    /// 实现领域服务
    /// </summary>
    public abstract class DomainService : IDomainService
    {
        /// <summary>
        /// IServiceProvider
        /// </summary>
        public IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// 线程锁
        /// </summary>
        protected readonly object ServiceProviderLock = new object();

        /// <summary>
        /// 从DI容器中获取服务引用
        /// </summary>
        /// <typeparam name="TService">需要获取的实例对象</typeparam>
        /// <param name="reference">返回实例</param>
        /// <returns>返回实例</returns>
        protected TService LazyGetRequiredService<TService>(ref TService reference) => LazyGetRequiredService(typeof(TService), ref reference);

        /// <summary>
        /// 从DI容器中获取服务引用
        /// </summary>
        /// <typeparam name="TRef">服务引用</typeparam>
        /// <param name="serviceType">对象类型</param>
        /// <param name="reference">服务引用</param>
        /// <returns>返回服务引用</returns>
        protected TRef LazyGetRequiredService<TRef>(Type serviceType, ref TRef reference)
        {
            if (reference == null)
            {
                lock (ServiceProviderLock)
                {
                    if (reference == null)
                    {
                        reference = (TRef)ServiceProvider.GetRequiredService(serviceType);
                    }
                }
            }

            return reference;
        }

        private ICurrentUser _currentUser;
        /// <summary>
        /// 获取当前用户对象
        /// </summary>
        public ICurrentUser CurrentUser => LazyGetRequiredService(ref _currentUser);

        private IMapper _mapper;
        /// <summary>
        /// 获取AutoMapper对象
        /// </summary>
        public IMapper Mapper => LazyGetRequiredService(ref _mapper);
    }
}