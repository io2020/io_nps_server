﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Nps.Infrastructure
{
    /// <summary>
    /// 参数合法性检查类
    /// </summary>
    [DebuggerStepThrough]
    public static class Check
    {
        /// <summary>
        /// 验证指定值的断言<paramref name="assertion"/>是否为真，如果不为真，抛出指定消息<paramref name="message"/>的指定类型<typeparamref name="TException"/>异常
        /// </summary>
        /// <typeparam name="TException">异常类型</typeparam>
        /// <param name="assertion">要验证的断言。</param>
        /// <param name="message">异常消息。</param>
        public static void Required<TException>(bool assertion, string message)
            where TException : Exception
        {
            if (assertion)
            {
                return;
            }
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException(nameof(message));
            }
            TException exception = (TException)Activator.CreateInstance(typeof(TException), message);
            throw exception;
        }

        /// <summary>
        /// 验证指定值的断言表达式是否为真，不为值抛出<see cref="Exception"/>异常
        /// </summary>
        /// <param name="value"></param>
        /// <param name="assertionFunc">要验证的断言表达式</param>
        /// <param name="message">异常消息</param>
        public static void Required<T>(T value, Func<T, bool> assertionFunc, string message)
        {
            if (assertionFunc == null)
            {
                throw new ArgumentNullException(nameof(assertionFunc));
            }
            Required<Exception>(assertionFunc(value), message);
        }

        /// <summary>
        /// 验证指定值的断言表达式是否为真，不为真抛出<typeparamref name="TException"/>异常
        /// </summary>
        /// <typeparam name="T">要判断的值的类型</typeparam>
        /// <typeparam name="TException">抛出的异常类型</typeparam>
        /// <param name="value">要判断的值</param>
        /// <param name="assertionFunc">要验证的断言表达式</param>
        /// <param name="message">异常消息</param>
        public static void Required<T, TException>(T value, Func<T, bool> assertionFunc, string message) where TException : Exception
        {
            if (assertionFunc == null)
            {
                throw new ArgumentNullException(nameof(assertionFunc));
            }
            Required<TException>(assertionFunc(value), message);
        }

        /// <summary>
        /// 检查参数不能为空引用，否则抛出<see cref="ArgumentNullException"/>异常。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="paramName">参数名称</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void NotNull<T>(T value, string paramName)
        {
            Required<ArgumentNullException>(value != null, string.Format(CheckConsts.ParameterCheck_NotNull, paramName));
        }

        /// <summary>
        /// 检查字符串不能为空引用或空字符串，否则抛出<see cref="ArgumentNullException"/>异常或<see cref="ArgumentException"/>异常。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="paramName">参数名称。</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static void NotNullOrEmpty(string value, string paramName)
        {
            Required<ArgumentException>(!string.IsNullOrEmpty(value), string.Format(CheckConsts.ParameterCheck_NotNullOrEmpty_String, paramName));
        }

        /// <summary>
        /// 检查Guid值不能为Guid.Empty，否则抛出<see cref="ArgumentException"/>异常。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="paramName">参数名称。</param>
        /// <exception cref="ArgumentException"></exception>
        public static void NotEmpty(Guid value, string paramName)
        {
            Required<ArgumentException>(value != Guid.Empty, string.Format(CheckConsts.ParameterCheck_NotEmpty_Guid, paramName));
        }

        /// <summary>
        /// 检查集合不能为空引用或空集合，否则抛出<see cref="ArgumentNullException"/>异常或<see cref="ArgumentException"/>异常。
        /// </summary>
        /// <typeparam name="T">集合项的类型。</typeparam>
        /// <param name="list"></param>
        /// <param name="paramName">参数名称。</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static void NotNullOrEmpty<T>(IReadOnlyList<T> list, string paramName)
        {
            NotNull(list, paramName);
            Required<ArgumentException>(list.Any(), string.Format(CheckConsts.ParameterCheck_NotNullOrEmpty_Collection, paramName));
        }

        /// <summary>
        /// 检查集合中没有包含值为null的项
        /// </summary>
        public static void HasNoNulls<T>(IReadOnlyList<T> list, string paramName)
        {
            NotNull(list, paramName);
            Required<ArgumentException>(list.All(m => m != null), string.Format(CheckConsts.ParameterCheck_NotContainsNull_Collection, paramName));
        }

        /// <summary>
        /// 检查参数必须小于[或可等于，参数<paramref name="canEqual"/>]指定值，否则抛出<see cref="ArgumentOutOfRangeException"/>异常。
        /// </summary>
        /// <typeparam name="T">参数类型。</typeparam>
        /// <param name="value"></param>
        /// <param name="paramName">参数名称。</param>
        /// <param name="target">要比较的值。</param>
        /// <param name="canEqual">是否可等于。</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void LessThan<T>(T value, string paramName, T target, bool canEqual = false) where T : IComparable<T>
        {
            bool flag = canEqual ? value.CompareTo(target) <= 0 : value.CompareTo(target) < 0;
            string format = canEqual ? CheckConsts.ParameterCheck_NotLessThanOrEqual : CheckConsts.ParameterCheck_NotLessThan;
            Required<ArgumentOutOfRangeException>(flag, string.Format(format, paramName, target));
        }

        /// <summary>
        /// 检查参数必须大于[或可等于，参数<paramref name="canEqual"/>]指定值，否则抛出<see cref="ArgumentOutOfRangeException"/>异常。
        /// </summary>
        /// <typeparam name="T">参数类型。</typeparam>
        /// <param name="value"></param>
        /// <param name="paramName">参数名称。</param>
        /// <param name="target">要比较的值。</param>
        /// <param name="canEqual">是否可等于。</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void GreaterThan<T>(T value, string paramName, T target, bool canEqual = false) where T : IComparable<T>
        {
            bool flag = canEqual ? value.CompareTo(target) >= 0 : value.CompareTo(target) > 0;
            string format = canEqual ? CheckConsts.ParameterCheck_NotGreaterThanOrEqual : CheckConsts.ParameterCheck_NotGreaterThan;
            Required<ArgumentOutOfRangeException>(flag, string.Format(format, paramName, target));
        }

        /// <summary>
        /// 检查参数必须在指定范围之间，否则抛出<see cref="ArgumentOutOfRangeException"/>异常。
        /// </summary>
        /// <typeparam name="T">参数类型。</typeparam>
        /// <param name="value"></param>
        /// <param name="paramName">参数名称。</param>
        /// <param name="start">比较范围的起始值。</param>
        /// <param name="end">比较范围的结束值。</param>
        /// <param name="startEqual">是否可等于起始值</param>
        /// <param name="endEqual">是否可等于结束值</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void Between<T>(T value, string paramName, T start, T end, bool startEqual = false, bool endEqual = false)
            where T : IComparable<T>
        {
            bool flag = startEqual ? value.CompareTo(start) >= 0 : value.CompareTo(start) > 0;
            string message = startEqual
                ? string.Format(CheckConsts.ParameterCheck_Between, paramName, start, end)
                : string.Format(CheckConsts.ParameterCheck_BetweenNotEqual, paramName, start, end, start);
            Required<ArgumentOutOfRangeException>(flag, message);

            flag = endEqual ? value.CompareTo(end) <= 0 : value.CompareTo(end) < 0;
            message = endEqual
                ? string.Format(CheckConsts.ParameterCheck_Between, paramName, start, end)
                : string.Format(CheckConsts.ParameterCheck_BetweenNotEqual, paramName, start, end, end);
            Required<ArgumentOutOfRangeException>(flag, message);
        }

        /// <summary>
        /// 检查指定路径的文件夹必须存在，否则抛出<see cref="DirectoryNotFoundException"/>异常。
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="paramName">参数名称。</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public static void DirectoryExists(string directory, string paramName = null)
        {
            NotNull(directory, paramName);
            Required<DirectoryNotFoundException>(Directory.Exists(directory), string.Format(CheckConsts.ParameterCheck_DirectoryNotExists, directory));
        }

        /// <summary>
        /// 检查指定路径的文件必须存在，否则抛出<see cref="FileNotFoundException"/>异常。
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="paramName">参数名称。</param>
        /// <exception cref="ArgumentNullException">当文件路径为null时</exception>
        /// <exception cref="FileNotFoundException">当文件路径不存在时</exception>
        public static void FileExists(string filename, string paramName = null)
        {
            NotNull(filename, paramName);
            Required<FileNotFoundException>(File.Exists(filename), string.Format(CheckConsts.ParameterCheck_FileNotExists, filename));
        }
    }
}