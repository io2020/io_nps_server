namespace Nps.Application.Nps.Dtos
{
    /// <summary>
    /// Nps服务器输出结果
    /// </summary>
    public class NpsServerSearchOutput
    {
        /// <summary>
        /// 服务地址
        /// </summary>
        public string ServerIPAddress { get; set; }

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