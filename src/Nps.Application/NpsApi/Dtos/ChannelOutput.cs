using Newtonsoft.Json;
using System.Collections.Generic;

namespace Nps.Application.NpsApi.Dtos
{
    /// <summary>
    /// 客户端隧道列表输出
    /// </summary>
    public class ChannelListOutput
    {
        /// <summary>
        /// 客户端隧道列表
        /// </summary>
        [JsonProperty("rows")]
        public List<ChannelDetail> Datas { get; set; }

        /// <summary>
        /// 客户端隧道总数
        /// </summary>
        [JsonProperty("total")]
        public string Total { get; set; }
    }

    /// <summary>
    /// 隧道详细信息
    /// </summary>
    public class ChannelDetail
    {
        /// <summary>
        /// 隧道id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 隧道端口
        /// </summary>
        [JsonProperty("Port")]
        public int ServerPort { get; set; }

        /// <summary>
        /// 隧道是否可用
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 隧道是否运行
        /// </summary>
        public bool RunStatus { get; set; }

        /// <summary>
        /// 隧道所属客户端详细信息
        /// </summary>
        public ClientDetail Client { get; set; }

        /// <summary>
        /// 隧道备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 隧道连接目标
        /// </summary>
        public ChannelTarget Target { get; set; }
    }

    /// <summary>
    /// 隧道连接目标
    /// </summary>
    public class ChannelTarget
    {
        /// <summary>
        /// 目标地址
        /// </summary>
        [JsonProperty("TargetStr")]
        public string TargetAddress { get; set; }

        /// <summary>
        /// 目标地址数组
        /// </summary>
        [JsonProperty("TargetArr")]
        public string[] TargetAddressArray { get; set; }

        /// <summary>
        /// 是否启用本地代理
        /// </summary>
        public bool LocalProxy { get; set; }
    }

    /// <summary>
    /// 单个隧道信息
    /// </summary>
    public class ChannelOutput
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
        public ChannelDetail Data { get; set; }
    }
}