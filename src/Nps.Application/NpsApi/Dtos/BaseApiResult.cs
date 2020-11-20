using Newtonsoft.Json;

namespace Nps.Application.NpsApi.Dtos
{
    /// <summary>
    /// NpsApi执行结果
    /// </summary>
    public class BaseApiResult
    {
        /// <summary>
        /// 执行结果 1=成功
        /// </summary>
        [JsonProperty("status")]
        public int Status { get; set; }

        /// <summary>
        /// 执行消息
        /// </summary>
        [JsonProperty("msg")]
        public string Message { get; set; }
    }
}