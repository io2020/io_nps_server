using System;
using System.Security.Cryptography;
using System.Text;

namespace Nps.Core.Infrastructure.Helpers
{
    /// <summary>
    /// 加密操作
    /// </summary>
    public static class EncryptHelper
    {
        #region Md5加密

        /// <summary>
        /// Md5加密，返回16位结果
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static string Md5By16(string value)
        {
            return Md5By16(value, Encoding.UTF8);
        }

        /// <summary>
        /// Md5加密，返回16位结果
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string Md5By16(string value, Encoding encoding)
        {
            return Md5(value, encoding, 4, 8);
        }

        /// <summary>
        /// Md5加密，返回32位结果
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static string Md5By32(string value)
        {
            return Md5By32(value, Encoding.UTF8);
        }

        /// <summary>
        /// Md5加密，返回32位结果
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string Md5By32(string value, Encoding encoding)
        {
            return Md5(value, encoding, null, null);
        }

        /// <summary>
        /// Md5加密
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="startIndex">开始索引</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        private static string Md5(string value, Encoding encoding, int? startIndex, int? length)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }
            var md5 = new MD5CryptoServiceProvider();
            string result;
            try
            {
                var hash = md5.ComputeHash(encoding.GetBytes(value));
                result = startIndex == null
                    ? BitConverter.ToString(hash)
                    : BitConverter.ToString(hash, startIndex.ToInt32OrDefault(0), length.ToInt32OrDefault(0));
            }
            finally
            {
                md5.Clear();
            }
            return result.Replace("-", "");
        }

        #endregion

        #region Base64加密

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static string Base64Encrypt(string value)
        {
            return Base64Encrypt(value, Encoding.UTF8);
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string Base64Encrypt(string value, Encoding encoding)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : Convert.ToBase64String(encoding.GetBytes(value));
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static string Base64Decrypt(string value)
        {
            return Base64Decrypt(value, Encoding.UTF8);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string Base64Decrypt(string value, Encoding encoding)
        {
            return string.IsNullOrWhiteSpace(value)
                ? string.Empty
                : encoding.GetString(Convert.FromBase64String(value));
        }

        #endregion
    }
}