using Newtonsoft.Json;

namespace Nps.Application.NpsApi.Dtos
{
    /// <summary>
    /// 获取服务器当前时间戳
    /// </summary>
    public class ServerTimeOutput
    {
        /// <summary>
        /// 时间戳（秒）
        /// </summary>
        [JsonProperty("time")]
        public string Timestamp { get; set; }
    }
}