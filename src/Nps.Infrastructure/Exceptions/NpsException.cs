using System;

namespace Nps.Infrastructure.Exceptions
{
    /// <summary>
    /// 异常
    /// </summary>
    [Serializable]
    public class NpsException : ApplicationException
    {
        /// <summary>
        /// 状态码
        /// </summary>
        private readonly int _code;

        /// <summary>
        /// 错误码，当为0时，代表正常
        /// </summary>
        private readonly StatusCode _errorCode;

        /// <summary>
        /// 构造函数
        /// </summary>
        public NpsException()
            : base("服务器繁忙，请稍后再试!")
        {
            _errorCode = StatusCode.Error;
            _code = 400;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">异常消息</param>
        /// <param name="errorCode">错误码，当为0时，代表正常</param>
        /// <param name="code">状态码</param>
        public NpsException(string message = "服务器繁忙，请稍后再试!", StatusCode errorCode = StatusCode.Error, int code = 400)
            : base(message)
        {
            _errorCode = errorCode;
            _code = code;
        }

        /// <summary>
        /// 获取状态码
        /// </summary>
        public int GetCode()
        {
            return _code;
        }

        /// <summary>
        /// 获取错误码，当为0时，代表正常
        /// </summary>
        public StatusCode GetErrorCode()
        {
            return _errorCode;
        }
    }
}