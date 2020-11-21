using System.Collections.Generic;

namespace Nps.Application.Nps.Dots
{
    /// <summary>
    /// 设备开通端口输出
    /// </summary>
    public class NpsOpenedOutput
    {
        /// <summary>
        /// 设备唯一识别编码
        /// </summary>
        public string DeviceUniqueId { get; set; }

        /// <summary>
        /// 已开通端口列表
        /// </summary>
        public List<NpsOpenedPortOutput> OpenPorts { get; set; }
    }

    /// <summary>
    /// 开通端口
    /// </summary>
    public class NpsOpenedPortOutput
    {
        /// <summary>
        /// 服务器地址
        /// </summary>
        public string ServerIPAddress { get; set; }

        /// <summary>
        /// 客户端连接端口
        /// </summary>
        public string ClientConnectPort { get; set; }

        /// <summary>
        /// 服务器端口
        /// </summary>
        public string ServerPort { get; set; }

        /// <summary>
        /// 客户端地址，ip+端口/端口
        /// </summary>
        public string DeviceAddress { get; set; }
    }
}