using System;

namespace Nps.Core.Aop.Attributes
{
    /// <summary>
    /// 禁用SQL语句记录
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
    public class DisableSqlCurdAttribute : Attribute
    {

    }
}