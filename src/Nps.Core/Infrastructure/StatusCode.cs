using System.ComponentModel;

namespace Nps.Core.Infrastructure
{
    /// <summary>
    /// 资源操作返回码
    /// </summary>
    public enum StatusCode
    {
        /// <summary>
        /// 操作成功
        /// </summary>
        [Description("操作成功")]
        Success = 0,

        /// <summary>
        /// 未知错误
        /// </summary>
        [Description("未知错误")]
        UnknownError = 99,

        /// <summary>
        /// 服务器未知错误
        /// </summary>
        [Description("服务器未知错误")]
        ServerUnknownError = 999,

        /// <summary>
        /// 操作失败
        /// </summary>
        [Description("操作失败")]
        Error = 1000,

        /// <summary>
        /// 认证失败
        /// </summary>
        [Description("认证失败")]
        AuthenticationFailed = 10000,

        /// <summary>
        /// 无权限
        /// </summary>
        [Description("无权限")]
        NoPermission = 10001,

        /// <summary>
        /// 资源不存在
        /// </summary>
        [Description("资源不存在")]
        NotFound = 10010,

        /// <summary>
        /// 参数错误
        /// </summary>
        [Description("参数错误")]
        ParameterError = 10020,

        /// <summary>
        /// 令牌刷新异常
        /// </summary>
        [Description("令牌刷新异常")]
        RefreshTokenError = 10030,

        /// <summary>
        /// 令牌失效
        /// </summary>
        [Description("令牌失效")]
        TokenInvalidation = 10040,

        /// <summary>
        /// 令牌过期
        /// </summary>
        [Description("令牌过期")]
        TokenExpired = 10050,

        /// <summary>
        /// 字段重复
        /// </summary>
        [Description("字段重复")]
        RepeatField = 10060,

        /// <summary>
        /// 禁止操作
        /// </summary>
        [Description("禁止操作")]
        Forbidden = 10070,

        /// <summary>
        /// 请求过于频繁，请稍后重试
        /// </summary>
        [Description("请求过于频繁，请稍后重试")]
        ManyRequests = 10140
    }
}