using Newtonsoft.Json;

namespace Nps.Application.NpsApi.Dtos
{
    /*
     * 注意： nps配置文件中auth_crypt_key需为16位
            解密密钥长度128
            偏移量与密钥相同
            补码方式pkcs5padding
            解密串编码方式 十六进制
     */

    /// <summary>
    /// AuthKey输出结果
    /// </summary>
    public class AuthKeyOutput
    {
        /// <summary>
        /// 执行结果 1=成功
        /// </summary>
        [JsonProperty("status")]
        public int Status { get; set; }

        /// <summary>
        /// 加密后的AuthKey
        /// </summary>
        [JsonProperty("crypt_auth_key")]
        public string CryptAuthKey { get; set; }

        /// <summary>
        /// 加密方式
        /// </summary>
        [JsonProperty("crypt_type")]
        public string CryptType { get; set; }
    }
}