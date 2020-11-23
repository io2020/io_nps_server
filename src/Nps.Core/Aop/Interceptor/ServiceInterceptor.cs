using Castle.DynamicProxy;

namespace Nps.Core.Aop.Interceptor
{
    /// <summary>
    /// 同步服务层异常/性能拦截器
    /// </summary>
    public class ServiceInterceptor : IInterceptor
    {
        private readonly ServiceAsyncInterceptor _asyncInterceptor;

        /// <summary>
        /// 初始化一个<see cref="ServiceInterceptor"/>实例
        /// </summary>
        public ServiceInterceptor(ServiceAsyncInterceptor asyncInterceptor)
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