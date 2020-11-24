using System;

namespace Nps.Core.Aop.Attributes
{
    /// <summary>
    /// 禁用审计日志追踪
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class DisableAuditingAttribute : Attribute
    {

    }
}