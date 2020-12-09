using System.Collections.Generic;

namespace Nps.Application.Nps.Dtos
{
    /// <summary>
    /// 开通客户端输入参数
    /// </summary>
    public class NpsClientOpenInput
    {
        /// <summary>
        /// 设备唯一标识，不同用户允许重复
        /// </summary>
        public string DeviceUniqueId { get; set; }

        /// <summary>
        /// 需要开通的端口列表
        /// </summary>
        public List<string> OpenPorts { get; set; }

        /// <summary>
        /// 是否允许客户端以配置文件模式连接
        /// </summary>
        public bool IsConfigConnectAllow { get; set; } = true;

        /// <summary>
        /// 是否压缩
        /// </summary>
        public bool IsCompress { get; set; } = false;

        /// <summary>
        /// 是否加密
        /// </summary>
        public bool IsCrypt { get; set; } = true;

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// 删除客户端输入参数
    /// </summary>
    public class NpsClientDeleteInput
    {
        /// <summary>
        /// 设备唯一标识，不同用户允许重复
        /// </summary>
        public string DeviceUniqueId { get; set; }

        /// <summary>
        /// 需要删除的客户端端口列表
        /// </summary>
        public List<string> DeletePorts { get; set; }
    }

    /// <summary>
    /// 查询客户端输入参数
    /// </summary>
    public class NpsClientSearchInput
    {
        /// <summary>
        /// 设备唯一标识，不同用户允许重复
        /// </summary>
        public string DeviceUniqueId { get; set; }

        /// <summary>
        /// 需要删除的客户端端口列表
        /// </summary>
        public List<string> SearchPorts { get; set; }
    }
}