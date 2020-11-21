namespace Nps.Core.Infrastructure.Helpers
{
    /// <summary>
    /// 验证 操作
    /// </summary>
    public static class RegexHelper
    {
        /// <summary>
        /// 是否IP地址
        /// </summary>
        /// <param name="value">ip地址</param>
        /// <returns>结果</returns>
        public static bool IsIpAddress(string value)
        {
            if (value.IsEmpty())
            {
                return false;
            }
            return value.IsMatch(@"((25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))");
        }
    }
}