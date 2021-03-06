﻿using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using Nps.Infrastructure;
using Nps.Infrastructure.Exceptions;
using StackExchange.Profiling;
using System;
using System.Threading.Tasks;

namespace Nps.Core.Aop.Interceptor
{
    /// <summary>
    /// 同步/异步服务层异常/性能拦截器
    /// </summary>
    public class ServiceAsyncInterceptor : AsyncInterceptorBase
    {
        private readonly ILogger<ServiceAsyncInterceptor> _logger;

        /// <summary>
        /// 初始化一个<see cref="TransactionalAsyncInterceptor"/>实例
        /// </summary>
        /// <param name="logger">日志对象</param>
        public ServiceAsyncInterceptor(ILogger<ServiceAsyncInterceptor> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 无返回值的 异步/同步 方法拦截
        /// </summary>
        /// <param name="invocation">IInvocation</param>
        /// <param name="proceed">Func<IInvocation, Task></param>
        protected override async Task InterceptAsync(IInvocation invocation, Func<IInvocation, Task> proceed)
        {
            var methodName = $"执行{invocation.MethodInvocationTarget.DeclaringType?.FullName}.{invocation.Method.Name}()->";
            var hashCode = invocation.GetHashCode();

            using (_logger.BeginScope("_service_Intercept：{hashCode}", hashCode))
            {
                try
                {
                    MiniProfiler.Current.Step(methodName);
                    await proceed(invocation).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"执行{methodName}出现异常，异常原因：{ex.Message + ex.InnerException}.");
                    throw new NpsException(ex.Message, StatusCode.Error);
                }
            }
        }

        /// <summary>
        /// 有返回值的 异步/同步 方法拦截
        /// </summary>
        /// <param name="invocation">IInvocation</param>
        /// <param name="proceed">Func<IInvocation, Task></param>
        protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, Func<IInvocation, Task<TResult>> proceed)
        {
            var methodName = $"执行{invocation.MethodInvocationTarget.DeclaringType?.FullName}.{invocation.Method.Name}()->";
            var hashCode = invocation.GetHashCode();

            using (_logger.BeginScope("_service_Result_Intercept：{hashCode}", hashCode))
            {
                try
                {
                    MiniProfiler.Current.Step(methodName);
                    var result = await proceed(invocation).ConfigureAwait(false);
                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"执行{methodName}出现异常，异常原因：{ex.Message + ex.InnerException}.");
                    throw new NpsException(ex.Message, StatusCode.Error);
                }
            }
        }
    }
}