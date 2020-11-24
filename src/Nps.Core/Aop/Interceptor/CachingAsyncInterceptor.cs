using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using Nps.Core.Aop.Attributes;
using Nps.Core.Caching;
using Nps.Core.Infrastructure;
using Nps.Core.Infrastructure.Exceptions;
using Nps.Core.Infrastructure.Extensions;
using Nps.Core.Infrastructure.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Nps.Core.Aop.Interceptor
{
    /// <summary>
    /// 同步/异步缓存拦截器
    /// </summary>
    public class CachingAsyncInterceptor : AsyncInterceptorBase
    {
        private readonly ILogger<CachingAsyncInterceptor> _logger;

        private readonly ICaching _caching;

        /// <summary>
        /// 初始化一个<see cref="CachingAsyncInterceptor"/>实例
        /// </summary>
        /// <param name="logger">日志对象</param>
        /// <param name="caching">缓存对象</param>
        public CachingAsyncInterceptor(
            ILogger<CachingAsyncInterceptor> logger,
            ICaching caching)
        {
            _logger = logger;
            _caching = caching;
        }

        /// <summary>
        /// 自定义缓存的key
        /// </summary>
        /// <param name="invocation">IInvocation</param>
        /// <returns>返回缓存主键</returns>
        private string CustomCacheKey(IInvocation invocation)
        {
            var typeName = invocation.TargetType.Name;
            var methodName = invocation.Method.Name;
            var methodArguments = invocation.Arguments;

            var defineKey = new
            {
                ServiceName= typeName,
                MethodName= methodName,
                Arguments= methodArguments
            };

            string key = defineKey.ToJson();
            return EncryptHelper.Md5By32(key);
        }

        /// <summary>
        /// 无返回值的 异步/同步 方法拦截
        /// </summary>
        /// <param name="invocation">IInvocation</param>
        /// <param name="proceed">Func<IInvocation, Task></param>
        protected override async Task InterceptAsync(IInvocation invocation, Func<IInvocation, Task> proceed)
        {
            await proceed(invocation).ConfigureAwait(false);
        }

        /// <summary>
        /// 有返回值的 异步/同步 方法拦截
        /// </summary>
        /// <param name="invocation">IInvocation</param>
        /// <param name="proceed">Func<IInvocation, Task></param>
        protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, Func<IInvocation, Task<TResult>> proceed)
        {
            var methodInfo = invocation.MethodInvocationTarget ?? invocation.Method;
            var cachingAttribute = methodInfo.GetCustomAttributes(typeof(CachingAttribute), false).FirstOrDefault();
            if (cachingAttribute is CachingAttribute attribute)
            {
                var methodName = $"开启缓存拦截：{methodInfo.Name}()->";
                var hashCode = invocation.GetHashCode();

                using (_logger.BeginScope("_cache_Result_Intercept：{hashCode}", hashCode))
                {
                    try
                    {
                        var cachingKey = CustomCacheKey(invocation);
                        var cacheValue = await _caching.GetAsync(cachingKey);
                        if (cacheValue != null)
                        {
                            return cacheValue.FromJson<TResult>();
                        }

                        var result = await proceed(invocation).ConfigureAwait(false);

                        if (cachingKey.IsNotNullOrWhiteSpace())
                        {
                            var timeSpan = attribute.ExpirationType switch
                            {
                                ExpirationType.Second => TimeSpan.FromSeconds(attribute.AbsoluteExpiration),
                                ExpirationType.Hour => TimeSpan.FromHours(attribute.AbsoluteExpiration),
                                ExpirationType.Day => TimeSpan.FromDays(attribute.AbsoluteExpiration),
                                _ => TimeSpan.FromMinutes(attribute.AbsoluteExpiration),
                            };
                            await _caching.SetAsync(cachingKey, result.ToJson(), timeSpan);
                        }

                        return result;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"{methodName}开启缓存出现异常，异常原因：{ex.Message + ex.InnerException}.");
                        throw new NpsException(ex.Message, StatusCode.Error);
                    }
                }
            }
            else
            {
                return await proceed(invocation).ConfigureAwait(false);
            }
        }
    }
}