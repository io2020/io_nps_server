using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Nps.Core.Caching
{
    /// <summary>
    /// 内存缓存
    /// </summary>
    public class MemoryCache : ICaching
    {
        //内存缓存对象
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="memoryCache">IMemoryCache</param>
        public MemoryCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        //获取所有缓存Key
        private List<string> GetAllKeys()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var entries = _memoryCache.GetType().GetField("_entries", flags).GetValue(_memoryCache);
            var keys = new List<string>();
            if (!(entries is IDictionary cacheItems)) return keys;
            foreach (DictionaryEntry cacheItem in cacheItems)
            {
                keys.Add(cacheItem.Key.ToString());
            }
            return keys;
        }

        #region Exist

        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="key">键</param>
        public bool Exists(string key)
        {
            return _memoryCache.TryGetValue(key, out _);
        }

        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="key">键</param>
        public async Task<bool> ExistsAsync(string key)
        {
            return await Task.FromResult(_memoryCache.TryGetValue(key, out _));
        }

        #endregion

        #region Get

        /// <summary>
        /// 获取指定 key 的值
        /// </summary>
        /// <param name="key">键</param>
        public string Get(string key)
        {
            return _memoryCache.Get(key)?.ToString();
        }

        /// <summary>
        /// 获取指定 key 的值
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">键</param>
        public T Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        /// <summary>
        /// 获取指定 key 的值
        /// </summary>
        /// <param name="key">键</param>
        public async Task<string> GetAsync(string key)
        {
            return await Task.FromResult(Get(key));
        }

        /// <summary>
        /// 获取指定 key 的值
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">键</param>
        public async Task<T> GetAsync<T>(string key)
        {
            return await Task.FromResult(Get<T>(key));
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
            _memoryCache.Set(key, value);
            return true;
        }

        /// <summary>
        /// 设置指定 key 的值，所有写入参数object都支持string | byte[] | 数值 | 对象
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expire">有效期</param>
        public bool Set(string key, object value, TimeSpan expire)
        {
            _memoryCache.Set(key, value, expire);
            return true;
        }

        /// <summary>
        /// 设置指定 key 的值，所有写入参数object都支持string | byte[] | 数值 | 对象
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public async Task<bool> SetAsync(string key, object value)
        {
            Set(key, value);
            return await Task.FromResult(true);
        }

        /// <summary>
        /// 设置指定 key 的值，所有写入参数object都支持string | byte[] | 数值 | 对象
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expire">有效期</param>
        public async Task<bool> SetAsync(string key, object value, TimeSpan expire)
        {
            Set(key, value, expire);
            return await Task.FromResult(true);
        }

        #endregion

        #region Remove

        /// <summary>
        /// 用于在 key 存在时删除 key
        /// </summary>
        /// <param name="key">键</param>
        public long Remove(params string[] key)
        {
            foreach (var k in key)
            {
                _memoryCache.Remove(k);
            }
            return key.Length;
        }

        /// <summary>
        /// 用于在 key 存在时删除 key
        /// </summary>
        /// <param name="key">键</param>
        public async Task<long> RemoveAsync(params string[] key)
        {
            foreach (var k in key)
            {
                _memoryCache.Remove(k);
            }

            return await Task.FromResult(key.Length.ToLong());
        }

        /// <summary>
        /// 用于在 key 模板存在时删除
        /// </summary>
        /// <param name="pattern">key模板</param>
        public async Task<long> RemoveByPatternAsync(string pattern)
        {
            if (pattern.IsNull())
                return default;

            pattern = Regex.Replace(pattern, @"\{.*\}", "(.*)");

            var keys = GetAllKeys().Where(k => Regex.IsMatch(k, pattern));

            if (keys != null && keys.Count() > 0)
            {
                return await RemoveAsync(keys.ToArray());
            }

            return default;
        }

        #endregion
    }
}