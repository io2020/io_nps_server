using Nps.Application.NpsApi.Dtos;
using WebApiClient;
using WebApiClient.Attributes;

namespace Nps.Application.NpsApi
{
    /*
     * web api
        需要开启请先去掉nps.conf中auth_key的注释并配置一个合适的密钥

        webAPI验证说明
        采用auth_key的验证方式
        在提交的每个请求后面附带两个参数，auth_key 和timestamp
        auth_key的生成方式为：md5(配置文件中的auth_key+当前时间戳)
        timestamp为当前时间戳
        注意： 为保证安全，时间戳的有效范围为20秒内，所以每次提交请求必须重新生成。
     */

    /// <summary>
    /// Nps服务端接口列表
    /// </summary>
    public interface INpsApi : IHttpApi
    {
        #region Service

        /// <summary>
        /// 获取服务端authKey
        /// </summary>
        /// <returns>将返回加密后的authKey，采用aes cbc加密，请使用与服务端配置文件中cryptKey相同的密钥进行解密</returns>
        [HttpPost("auth/getauthkey")]
        ITask<AuthKeyOutput> AuthKeyAsync();

        /// <summary>
        /// 获取服务端时间
        /// </summary>
        [HttpPost("auth/gettime")]
        ITask<ServerTimeOutput> ServerTimeAsync();

        #endregion

        #region Client

        /// <summary>
        /// 获取客户端列表
        /// </summary>
        [HttpPost("client/list")]
        ITask<ClientListOutput> ClientListAsync(ClientListInput input);

        /// <summary>
        /// 根据Id获取指定的客户端
        /// </summary>
        [HttpPost("client/getclient")]
        ITask<ClientOutput> ClientAsync(ClientIdInput input);

        /// <summary>
        /// 添加客户端
        /// </summary>
        [HttpPost("client/add")]
        ITask<BaseApiResult> AddClientAsync(ClientAddInput input);

        /// <summary>
        /// 编辑客户端
        /// </summary>
        [HttpPost("client/edit")]
        ITask<BaseApiResult> EditClientAsync(ClientEditInput input);

        /// <summary>
        /// 删除客户端
        /// </summary>
        [HttpPost("client/del")]
        ITask<BaseApiResult> DeleteClientAsync(ClientIdInput input);

        #endregion

        #region ClientChannel

        /// <summary>
        /// 获取客户端隧道列表
        /// </summary>
        [HttpPost("index/gettunnel")]
        ITask<ChannelListOutput> ChannelListAsync(ChannelListInput input);

        /// <summary>
        /// 获取单个隧道
        /// </summary>
        [HttpPost("index/getonetunnel")]
        ITask<ChannelOutput> ChannelAsync(ChannelIdInput input);

        /// <summary>
        /// 添加隧道
        /// </summary>
        [HttpPost("index/add")]
        ITask<BaseApiResult> AddChannelAsync(ChannelAddInput input);

        /// <summary>
        /// 编辑隧道
        /// </summary>
        [HttpPost("index/edit")]
        ITask<BaseApiResult> EditChannelAsync(ChannelEditInput input);

        /// <summary>
        /// 删除隧道
        /// </summary>
        [HttpPost("index/del")]
        ITask<BaseApiResult> DeleteChannelAsync(ChannelIdInput input);

        /// <summary>
        /// 启动隧道
        /// </summary>
        [HttpPost("index/start")]
        ITask<string> StartChannelAsync(ChannelIdInput input);

        /// <summary>
        /// 停止隧道
        /// </summary>
        [HttpPost("index/stop")]
        ITask<string> StopChannelAsync(ChannelIdInput input);

        #endregion
    }
}