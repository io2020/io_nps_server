using Newtonsoft.Json;

namespace Nps.Application.NpsApi.Dtos
{
    /// <summary>
    /// 基础验证
    /// </summary>
    public class BaseAuthInput
    {
        /// <summary>
        /// auth_key的生成方式为：md5(配置文件中的auth_key+当前时间戳)
        /// md5 32位 小写
        /// </summary>
        [JsonProperty("auth_key")]
        public string AuthKey { get; set; }

        /// <summary>
        /// 当前时间戳（秒） 为保证安全，时间戳的有效范围为20秒内，所以每次提交请求必须重新生成。
        /// </summary>
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }
    }
}