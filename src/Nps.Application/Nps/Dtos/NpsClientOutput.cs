using System.Collections.Generic;

namespace Nps.Application.Nps.Dtos
{
    /// <summary>
    /// 设备开通端口输出
    /// </summary>
    public class NpsClientOpenedOutput
    {
        /// <summary>
        /// 设备唯一识别编码
        /// </summary>
        public string DeviceUniqueId { get; set; }

        /// <summary>
        /// vKey
        /// </summary>
        public string VirtualKey { get; set; }

        ///// <summary>
        ///// 服务器域名
        ///// </summary>
        //[JsonProperty("serverIP")]
        //public string ServerDomain { get; set; }

        /// <summary>
        /// 服务器地址
        /// </summary>
        public string ServerIPAddress { get; set; }

        /// <summary>
        /// 客户端连接端口
        /// </summary>
        public string ClientConnectPort { get; set; }

        /// <summary>
        /// 已开通端口列表
        /// </summary>
        public List<NpsClientOpenedPortOutput> OpenPorts { get; set; }
    }

    /// <summary>
    /// 开通端口
    /// </summary>
    public class NpsClientOpenedPortOutput
    {
        /// <summary>
        /// 服务器端口
        /// </summary>
        public string ServerPort { get; set; }

        /// <summary>
        /// 客户端地址，ip+端口/端口
        /// </summary>
        public string DeviceAddress { get; set; }
    }

    /// <summary>
    /// 设备删除输出
    /// </summary>
    public class NpsClientDeletedOutput
    {
        /// <summary>
        /// 设备客户端地址
        /// </summary>
        public string DeviceAddress { get; set; }

        /// <summary>
        /// 远程删除执行状态
        /// </summary>
        public int RemoteStatus { get; set; }

        /// <summary>
        /// 远程删除执行消息
        /// </summary>
        public string RemoteMessage { get; set; }
    }
}