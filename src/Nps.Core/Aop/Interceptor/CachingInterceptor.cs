using Castle.DynamicProxy;

namespace Nps.Core.Aop.Interceptor
{
    /// <summary>
    /// 同步缓存拦截器
    /// </summary>
    public class CachingInterceptor : IInterceptor
    {
        private readonly CachingAsyncInterceptor _asyncInterceptor;

        /// <summary>
        /// 初始化一个<see cref="CachingAsyncInterceptor"/>实例
        /// </summary>
        public CachingInterceptor(CachingAsyncInterceptor asyncInterceptor)
        {
            _asyncInterceptor = asyncInterceptor;
        }

        /// <summary>
        /// 同步拦截方法
        /// </summary>
        /// <param name="invocation">IInvocation</param>
        public void Intercept(IInvocation invocation)
        {
            _asyncInterceptor.ToInterceptor().Intercept(invocation);
        }
    }
}