using System;

namespace Nps.Core.Entities
{
    /// <summary>
    /// 更新时间
    /// </summary>
    public interface IHasUpdateTime
    {
        /// <summary>
        /// 更新时间
        /// </summary>
        DateTime? UpdateTime { get; set; }
    }
}