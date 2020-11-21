using Castle.DynamicProxy;

namespace Nps.Core.Aop.Interceptor
{
    /// <summary>
    /// 事务同步拦截器
    /// </summary>
    public class TransactionalInterceptor : IInterceptor
    {
        private readonly TransactionalAsyncInterceptor _asyncInterceptor;

        /// <summary>
        /// 初始化一个<see cref="TransactionalInterceptor"/>实例
        /// </summary>
        /// <param name="asyncInterceptor">TransactionalAsyncInterceptor</param>
        public TransactionalInterceptor(TransactionalAsyncInterceptor asyncInterceptor)
        {
            _asyncInterceptor = asyncInterceptor;
        }

        /// <summary>
        /// 开启拦截
        /// </summary>
        /// <param name="invocation">IInvocation</param>
        public void Intercept(IInvocation invocation)
        {
            _asyncInterceptor.ToInterceptor().Intercept(invocation);
        }
    }
}
