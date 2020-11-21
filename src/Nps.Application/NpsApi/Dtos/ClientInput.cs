using Newtonsoft.Json;

namespace Nps.Application.NpsApi.Dtos
{
    /// <summary>
    /// 客户端Id输入模型
    /// </summary>
    public class ClientIdInput : BaseAuthInput
    {
        /// <summary>
        /// 客户端Id
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }
    }

    /// <summary>
    /// 查询客户端列表
    /// </summary>
    public class ClientListInput : BaseAuthInput
    {
        /// <summary>
        /// 搜索
        /// </summary>
        [JsonProperty("search")]
        public string KeyWords { get; set; }

        /// <summary>
        /// 排序asc 正序 desc倒序
        /// </summary>
        [JsonProperty("order")]
        public string OrderType { get; set; }

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
    /// 添加客户端
    /// </summary>
    public class ClientAddInput : BaseAuthInput
    {
        /// <summary>
        /// 备注
        /// </summary>
        [JsonProperty("remark")]
        public string Remark { get; set; }

        /// <summary>
        /// 客户端验证密钥
        /// </summary>
        [JsonProperty("vkey")]
        public string AppSecret { get; set; }

        /// <summary>
        /// 是否允许客户端以配置文件模式连接 1允许 0不允许
        /// </summary>
        [JsonProperty("config_conn_allow")]
        public int IsConfigConnectAllow { get; set; } = 1;

        /// <summary>
        /// 是否压缩1允许 0不允许
        /// </summary>
        [JsonProperty("compress")]
        public int IsCompress { get; set; } = 0;

        /// <summary>
        /// 是否加密（1或者0）1允许 0不允许
        /// </summary>
        [JsonProperty("crypt")]
        public int IsCrypt { get; set; } = 1;
    }

    /// <summary>
    /// 根据客户端id编辑
    /// </summary>
    public class ClientEditInput : ClientAddInput
    {
        /// <summary>
        /// 客户端Id
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}