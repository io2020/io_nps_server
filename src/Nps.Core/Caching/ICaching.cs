﻿using System;
using System.Threading.Tasks;

namespace Nps.Core.Caching
{
    /// <summary>
    /// 缓存接口
    /// </summary>
    public interface ICaching
    {
        #region Exists

        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="key">键</param>
        bool Exists(string key);

        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="key">键</param>
        Task<bool> ExistsAsync(string key);

        #endregion

        #region Get

        /// <summary>
        /// 获取指定 key 的值
        /// </summary>
        /// <param name="key">键</param>
        string Get(string key);

        /// <summary>
        /// 获取指定 key 的值
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">键</param>
        T Get<T>(string key);

        /// <summary>
        /// 获取指定 key 的值
        /// </summary>
        /// <param name="key">键</param>
        Task<string> GetAsync(string key);

        /// <summary>
        /// 获取指定 key 的值
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">键</param>
        Task<T> GetAsync<T>(string key);

        #endregion

        #region Set

        /// <summary>
        /// 设置指定 key 的值，所有写入参数object都支持string | byte[] | 数值 | 对象
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        bool Set(string key, object value);

        /// <summary>
        /// 设置指定 key 的值，所有写入参数object都支持string | byte[] | 数值 | 对象
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expire">有效期</param>
        bool Set(string key, object value, TimeSpan expire);

        /// <summary>
        /// 设置指定 key 的值，所有写入参数object都支持string | byte[] | 数值 | 对象
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        Task<bool> SetAsync(string key, object value);

        /// <summary>
        /// 设置指定 key 的值，所有写入参数object都支持string | byte[] | 数值 | 对象
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expire">有效期</param>
        Task<bool> SetAsync(string key, object value, TimeSpan expire);

        #endregion

        #region Remove

        /// <summary>
        /// 用于在 key 存在时删除 key
        /// </summary>
        /// <param name="key">键</param>
        long Remove(params string[] key);

        /// <summary>
        /// 用于在 key 存在时删除 key
        /// </summary>
        /// <param name="key">键</param>
        Task<long> RemoveAsync(params string[] key);

        /// <summary>
        /// 用于在 key 模板存在时删除
        /// </summary>
        /// <param name="pattern">key模板</param>
        Task<long> RemoveByPatternAsync(string pattern);

        #endregion
    }
}