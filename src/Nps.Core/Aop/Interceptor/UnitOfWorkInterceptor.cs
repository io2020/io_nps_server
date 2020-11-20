using Castle.DynamicProxy;

namespace Nps.Core.Aop.Interceptor
{
    /// <summary>
    /// 工作单元Aop
    /// </summary>
    public class UnitOfWorkInterceptor : IInterceptor
    {
        private readonly UnitOfWorkAsyncInterceptor asyncInterceptor;

        /// <summary>
        /// 初始化一个<see cref="UnitOfWorkInterceptor"/>实例
        /// </summary>
        /// <param name="interceptor">UnitOfWorkAsyncInterceptor</param>
        public UnitOfWorkInterceptor(UnitOfWorkAsyncInterceptor interceptor)
        {
            asyncInterceptor = interceptor;
        }

        /// <summary>
        /// 开启拦截
        /// </summary>
        /// <param name="invocation">IInvocation</param>
        public void Intercept(IInvocation invocation)
        {
            asyncInterceptor.ToInterceptor().Intercept(invocation);
        }
    }
}