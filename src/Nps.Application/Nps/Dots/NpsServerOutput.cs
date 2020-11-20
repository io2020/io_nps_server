namespace Nps.Application.Nps.Dots
{
    /// <summary>
    /// Nps服务器输出结果
    /// </summary>
    public class NpsServerOutput
    {
        /// <summary>
        /// 服务器Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 服务地址
        /// </summary>
        public string HostDomain { get; set; }

        /// <summary>
        /// 服务IP地址
        /// </summary>
        public string HostIP { get; set; }

        /// <summary>
        /// 服务端口
        /// </summary>
        public int HostPort { get; set; }

        /// <summary>
        /// 客户端连接端口
        /// </summary>
        public int ClientConnectPort { get; set; }

        /// <summary>
        /// 连接协议类型
        /// </summary>
        public string ProtocolType { get; set; }
    }
}