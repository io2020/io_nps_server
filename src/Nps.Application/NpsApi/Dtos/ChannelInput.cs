using Newtonsoft.Json;

namespace Nps.Application.NpsApi.Dtos
{
    /// <summary>
    /// 客户端隧道输入模型
    /// </summary>
    public class ChannelIdInput : BaseAuthInput
    {
        /// <summary>
        /// 隧道的id
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }
    }

    /// <summary>
    /// 根据客户端id查询客户端隧道列表
    /// </summary>
    public class ChannelListInput : BaseAuthInput
    {
        /// <summary>
        /// 穿透隧道的客户端id
        /// </summary>
        [JsonProperty("client_id")]
        public int ClientId { get; set; }

        /// <summary>
        /// 协议类型
        /// </summary>
        [JsonProperty("type")]
        public string ProtocolType => "tcp";

        /// <summary>
        /// 搜索
        /// </summary>
        [JsonProperty("search")]
        public string KeyWords { get; set; }

        /// <summary>
        /// 分页(第几页)
        /// </summary>
        [JsonProperty("offset")]
        public string Offset { get; set; }

        /// <summary>
        /// 条数(分页显示的条数)
        /// </summary>
        [JsonProperty("limit")]
        public string Limit { get; set; }
    }

    /// <summary>
    /// 添加客户端隧道
    /// </summary>
    public class ChannelAddInput : BaseAuthInput
    {
        /// <summary>
        /// 协议类型
        /// </summary>
        [JsonProperty("type")]
        public string ProtocolType => "tcp";

        /// <summary>
        /// 备注
        /// </summary>
        [JsonProperty("remark")]
        public string Remark { get; set; }

        /// <summary>
        /// 服务端端口 服务端生成 设计从10001开始，最大值60000
        /// </summary>
        [JsonProperty("port")]
        public int ServerPort { get; set; }

        /// <summary>
        /// 目标(ip:端口)
        /// </summary>
        [JsonProperty("target")]
        public string TargetAddress { get; set; }

        /// <summary>
        /// 穿透隧道的客户端id
        /// </summary>
        [JsonProperty("client_id")]
        public int ClientId { get; set; }
    }

    /// <summary>
    /// 编辑客户端隧道
    /// </summary>
    public class ChannelEditInput : ChannelAddInput
    {
        /// <summary>
        /// 隧道的id
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}