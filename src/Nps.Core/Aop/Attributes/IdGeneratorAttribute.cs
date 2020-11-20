using System;

namespace Nps.Core.Aop.Attributes
{
    /// <summary>
    /// 实体属性启用Id生成器标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IdGeneratorAttribute : Attribute
    {

    }
}