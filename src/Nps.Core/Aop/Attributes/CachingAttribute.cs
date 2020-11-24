using System;

namespace Nps.Core.Aop.Attributes
{
    /// <summary>
    /// 缓存
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CachingAttribute : Attribute
    {
        /// <summary>
        /// 缓存绝对过期时间
        /// </summary>
        public int AbsoluteExpiration { get; set; } = 30;

        /// <summary>
        /// 缓存过期类型，默认为分钟
        /// </summary>
        public ExpirationType ExpirationType { get; set; } = ExpirationType.Minute;
    }

    /// <summary>
    /// 过期类型
    /// </summary>
    public enum ExpirationType
    {
        /// <summary>
        /// 秒
        /// </summary>
        Second = 0,

        /// <summary>
        /// 分钟
        /// </summary>
        Minute = 1,

        /// <summary>
        /// 小时
        /// </summary>
        Hour = 2,

        /// <summary>
        /// 天
        /// </summary>
        Day = 3
    }
}