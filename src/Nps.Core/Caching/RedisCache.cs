using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Nps.Core.Caching
{
    /// <summary>
    /// Redis缓存
    /// </summary>
    public class RedisCache : ICaching
    {
        #region Exist

        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="key">键</param>
        public bool Exists(string key)
        {
            return RedisHelper.Exists(key);
        }

        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="key">键</param>
        public async Task<bool> ExistsAsync(string key)
        {
            return await RedisHelper.ExistsAsync(key);
        }

        #endregion

        #region Get

        /// <summary>
        /// 获取指定 key 的值
        /// </summary>
        /// <param name="key">键</param>
        public string Get(string key)
        {
            return RedisHelper.Get(key);
        }

        /// <summary>
        /// 获取指定 key 的值
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">键</param>
        public T Get<T>(string key)
        {
            return RedisHelper.Get<T>(key);
        }

        /// <summary>
        /// 获取指定 key 的值
        /// </summary>
        /// <param name="key">键</param>
        public async Task<string> GetAsync(string key)
        {
            return await RedisHelper.GetAsync(key);
        }

        /// <summary>
        /// 获取指定 key 的值
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">键</param>
        public async Task<T> GetAsync<T>(string key)
        {
            return await RedisHelper.GetAsync<T>(key);
        }

        #endregion

        #region Set

        /// <summary>
        /// 设置指定 key 的值，所有写入参数object都支持string | byte[] | 数值 | 对象
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public bool Set(string key, object value)
        {
            return RedisHelper.Set(key, value);
        }

        /// <summary>
        /// 设置指定 key 的值，所有写入参数object都支持string | byte[] | 数值 | 对象
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expire">有效期</param>
        public bool Set(string key, object value, TimeSpan expire)
        {
            return RedisHelper.Set(key, value, expire);
        }

        /// <summary>
        /// 设置指定 key 的值，所有写入参数object都支持string | byte[] | 数值 | 对象
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public async Task<bool> SetAsync(string key, object value)
        {
            return await RedisHelper.SetAsync(key, value);
        }

        /// <summary>
        /// 设置指定 key 的值，所有写入参数object都支持string | byte[] | 数值 | 对象
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expire">有效期</param>
        public async Task<bool> SetAsync(string key, object value, TimeSpan expire)
        {
            return await RedisHelper.SetAsync(key, value, expire);
        }

        #endregion

        #region Remove

        /// <summary>
        /// 用于在 key 存在时删除 key
        /// </summary>
        /// <param name="key">键</param>
        public long Remove(params string[] key)
        {
            return RedisHelper.Del(key);
        }

        /// <summary>
        /// 用于在 key 存在时删除 key
        /// </summary>
        /// <param name="key">键</param>
        public async Task<long> RemoveAsync(params string[] key)
        {
            return await RedisHelper.DelAsync(key);
        }

        /// <summary>
        /// 用于在 key 模板存在时删除
        /// </summary>
        /// <param name="pattern">key模板</param>
        public async Task<long> RemoveByPatternAsync(string pattern)
        {
            if (pattern.IsNull())
                return default;

            pattern = Regex.Replace(pattern, @"\{.*\}", "*");

            var keys = (await RedisHelper.KeysAsync(pattern));
            if (keys != null && keys.Length > 0)
            {
                return await RedisHelper.DelAsync(keys);
            }

            return default;
        }

        #endregion
    }
}