using Newtonsoft.Json;
using System.Collections.Generic;

namespace Nps.Application.NpsApi.Dtos
{
    /// <summary>
    /// 客户端列表输出
    /// </summary>
    public class ClientListOutput
    {
        /// <summary>
        /// nps客户端连接端口
        /// </summary>
        [JsonProperty("bridgePort")]
        public string ClientConnectPort { get; set; }

        /// <summary>
        /// nps服务器IP地址
        /// </summary>
        [JsonProperty("ip")]
        public string ServerIPAddress { get; set; }

        /// <summary>
        /// 客户端列表
        /// </summary>
        [JsonProperty("rows")]
        public List<ClientDetail> Datas { get; set; }

        /// <summary>
        /// 客户端总数
        /// </summary>
        [JsonProperty("total")]
        public string Total { get; set; }
    }

    /// <summary>
    /// 客户端详细信息
    /// </summary>
    public class ClientDetail
    {
        /// <summary>
        /// 客户端id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 客户端验证密钥
        /// </summary>
        [JsonProperty("VerifyKey")]
        public string AppSecret { get; set; }

        /// <summary>
        /// 最后一次连接客户端IP地址
        /// </summary>
        [JsonProperty("Addr")]
        public string LastConnectAddress { get; set; }

        /// <summary>
        /// 客户端备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 客户端是否可用
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 客户端是否连接
        /// </summary>
        public bool IsConnect { get; set; }
    }

    /// <summary>
    /// 单个客户端信息
    /// </summary>
    public class ClientOutput
    {
        /// <summary>
        /// 执行结果 1=成功
        /// </summary>
        [JsonProperty("code")]
        public int Status { get; set; }

        /// <summary>
        /// 客户端详细信息
        /// </summary>
        [JsonProperty("data")]
        public ClientDetail Data { get; set; }
    }
}