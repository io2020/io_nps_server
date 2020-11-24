using Castle.DynamicProxy;
using FreeSql;
using Microsoft.Extensions.Logging;
using Nps.Core.Aop.Attributes;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Nps.Core.Aop.Interceptor
{
    /// <summary>
    /// 同步/异步事务拦截器
    /// </summary>
    public class TransactionalAsyncInterceptor : AsyncInterceptorBase
    {
        private readonly UnitOfWorkManager _unitOfWorkManager;

        private readonly ILogger<TransactionalAsyncInterceptor> _logger;

        private IUnitOfWork _unitOfWork;

        /// <summary>
        /// 初始化一个<see cref="TransactionalAsyncInterceptor"/>对象
        /// </summary>
        /// <param name="unitOfWorkManager">UnitOfWorkManager</param>
        /// <param name="logger">ILogger<TransactionInterceptor></param>
        public TransactionalAsyncInterceptor(
            UnitOfWorkManager unitOfWorkManager,
            ILogger<TransactionalAsyncInterceptor> logger)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _logger = logger;
        }

        /// <summary>
        /// 是否需要启动事务
        /// </summary>
        /// <param name="invocation">IInvocation</param>
        /// <returns>True/False</returns>
        private bool TryBegin(IInvocation invocation)
        {
            var methodInfo = invocation.MethodInvocationTarget ?? invocation.Method;
            var attribute = methodInfo.GetCustomAttributes(typeof(TransactionalAttribute), false).FirstOrDefault();
            if (attribute is TransactionalAttribute transaction)
            {
                _unitOfWork = _unitOfWorkManager.Begin(transaction.Propagation, transaction.IsolationLevel);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 无返回值的 异步/同步 方法拦截
        /// </summary>
        /// <param name="invocation">IInvocation</param>
        /// <param name="proceed">Func<IInvocation, Task></param>
        protected override async Task InterceptAsync(IInvocation invocation, Func<IInvocation, Task> proceed)
        {
            if (TryBegin(invocation))
            {
                var methodName = $"{invocation.MethodInvocationTarget.DeclaringType?.FullName}.{invocation.Method.Name}()->";
                var hashCode = _unitOfWork.GetHashCode();

                using (_logger.BeginScope("_unitOfWork_Transactional_Intercept：{hashCode}", hashCode))
                {
                    _logger.LogInformation($"------ async Task Intercept 已开启事务拦截：工作单元编号：{hashCode}，方法全称：{methodName} ------");
                    try
                    {
                        await proceed(invocation).ConfigureAwait(false);
                        _unitOfWork.Commit();
                        _logger.LogInformation($"------ async Task Intercept 执行完成，已提交事务：工作单元编号：{hashCode} ------");
                    }
                    catch (Exception ex)
                    {
                        _unitOfWork.Rollback();
                        _logger.LogError($"------ async Task Intercept 执行失败，失败原因：{ex.Message}；已回滚事务：工作单元编号：{hashCode} ------");
                        throw;
                    }
                    finally
                    {
                        _unitOfWork.Dispose();
                    }
                }
            }
            else
            {
                await proceed(invocation).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// 有返回值的 异步/同步 方法拦截
        /// </summary>
        /// <param name="invocation">IInvocation</param>
        /// <param name="proceed">Func<IInvocation, Task></param>
        protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, Func<IInvocation, Task<TResult>> proceed)
        {
            if (TryBegin(invocation))
            {
                var methodName = $"{invocation.MethodInvocationTarget.DeclaringType?.FullName}.{invocation.Method.Name}()->";
                var hashCode = _unitOfWork.GetHashCode();

                using (_logger.BeginScope("_unitOfWork_Transactional_Intercept：{hashCode}", hashCode))
                {
                    _logger.LogInformation($"------ async Task<TResult> Intercept 已开启事务拦截：工作单元编号：{hashCode}，方法全称：{methodName} ------");
                    try
                    {
                        var result = await proceed(invocation).ConfigureAwait(false);
                        _unitOfWork.Commit();
                        _logger.LogInformation($"------ async Task<TResult> Intercept 执行完成，已提交事务：工作单元编号：{hashCode} ------");

                        return result;
                    }
                    catch (Exception ex)
                    {
                        _unitOfWork.Rollback();
                        _logger.LogError($"------ async Task<TResult> Intercept 执行失败，失败原因：{ex.Message}；已回滚事务：工作单元编号：{hashCode} ------");
                        throw;
                    }
                    finally
                    {
                        _unitOfWork.Dispose();
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