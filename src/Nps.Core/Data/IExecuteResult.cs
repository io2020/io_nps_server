using Nps.Core.Infrastructure;

namespace Nps.Core.Data
{
    /// <summary>
    /// 统一Api响应结果输出
    /// </summary>
    public interface IExecuteResult
    {
        /// <summary>
        /// 是否请求成功
        /// </summary>
        bool IsSuccess { get; }

        /// <summary>
        /// 状态码
        /// </summary>
        StatusCode Status { get; }

        /// <summary>
        /// 执行消息
        /// </summary>
        string Message { get; }
    }

    /// <summary>
    /// 统一Api响应结果输出
    /// </summary>
    /// <typeparam name="TResult">响应结果</typeparam>
    public interface IExecuteResult<TResult> : IExecuteResult
    {
        /// <summary>
        /// 响应结果
        /// </summary>
        TResult Data { get; }
    }
}