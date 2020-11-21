using System;

namespace Nps.Core.Infrastructure.Extensions
{
    /// <summary>
    /// 系统扩展--日期
    /// </summary>
    public static partial class Extension
    {
        /// <summary>
        /// 时间转时间戳-毫秒
        /// </summary>
        /// <param name="dateTime">需要转换的时间</param>
        /// <returns>返回时间戳</returns>
        public static string ToTimestamp(this DateTime dateTime)
        {
            var ts = dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }

        /// <summary>
        /// 时间转时间戳-秒
        /// </summary>
        /// <param name="dateTime">需要转换的时间</param>
        /// <returns>返回时间戳</returns>
        public static string ToTimestampSeconds(this DateTime dateTime)
        {
            var ts = dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
    }
}