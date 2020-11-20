using Newtonsoft.Json;
using Nps.Core.Infrastructure;

namespace Nps.Core.Data
{
    /// <summary>
    /// 统一Api响应结果输出
    /// </summary>
    /// <typeparam name="TResult">响应结果</typeparam>
    public class ExecuteResult<TResult> : IExecuteResult<TResult>
    {
        /// <summary>
        /// 是否成功标记
        /// </summary>
        [JsonIgnore]
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// 状态码
        /// </summary>
        public StatusCode Status { get; private set; }

        /// <summary>
        /// 执行消息
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public TResult Data { get; private set; }

        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="data">返回数据</param>
        /// <param name="status">状态码</param>
        /// <param name="message">执行消息</param>
        /// <returns>返回执行结果</returns>
        public ExecuteResult<TResult> Ok(TResult data, StatusCode status, string message)
        {
            IsSuccess = true;
            Status = status;
            Message = message;
            Data = data;

            return this;
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="message">执行消息</param>
        /// <param name="status">状态码</param>
        /// <param name="data">返回数据</param>
        /// <returns>返回执行结果</returns>
        public ExecuteResult<TResult> Error(string message, StatusCode status, TResult data)
        {
            IsSuccess = false;
            Status = status;
            Message = message;
            Data = data;

            return this;
        }
    }

    /// <summary>
    /// 统一Api响应结果输出
    /// </summary>
    public static partial class ExecuteResult
    {
        /// <summary>
        /// 成功
        /// </summary>
        /// <returns>返回执行结果</returns>
        public static IExecuteResult Ok()
        {
            return Ok("");
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="data">返回数据</param>
        /// <returns>返回执行结果</returns>
        public static IExecuteResult Ok<TResult>(TResult data)
        {
            return Ok(data, StatusCode.Success);
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="data">返回数据</param>
        /// <param name="status">状态码</param>
        /// <returns>返回执行结果</returns>
        public static IExecuteResult Ok<TResult>(TResult data, StatusCode status)
        {
            return Ok(data, status, status.ToDescriptionOrString());
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="data">返回数据</param>
        /// <param name="status">状态码</param>
        /// <param name="message">执行消息</param>
        /// <returns>返回执行结果</returns>
        public static IExecuteResult Ok<TResult>(TResult data = default, StatusCode status = StatusCode.Success, string message = "")
        {
            return new ExecuteResult<TResult>().Ok(data, status, message.IsNotNullOrWhiteSpace() ? message : status.ToDescriptionOrString());
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <returns>返回执行结果</returns>
        public static IExecuteResult Error()
        {
            return Error(StatusCode.Error.ToDescriptionOrString());
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="message">执行消息</param>
        /// <returns>返回执行结果</returns>
        public static IExecuteResult Error(string message)
        {
            return Error(message, StatusCode.Error);
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="message">执行消息</param>
        /// <param name="status">状态码</param>
        /// <returns>返回执行结果</returns>
        public static IExecuteResult Error(string message, StatusCode status)
        {
            return Error<string>(message, status, default);
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="message">执行消息</param>
        /// <param name="status">状态码</param>
        /// <param name="data">返回数据</param>
        /// <returns>返回执行结果</returns>
        public static IExecuteResult Error<TResult>(string message, StatusCode status = StatusCode.Error, TResult data = default)
        {
            return new ExecuteResult<TResult>().Error(message, status, data);
        }
    }
}