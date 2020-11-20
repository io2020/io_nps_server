using System;

namespace Nps.Core.Entities
{
    /// <summary>
    /// 删除时间
    /// </summary>
    public interface IHasDeleteTime
    {
        /// <summary>
        /// 删除时间
        /// </summary>
        DateTime? DeleteTime { get; set; }
    }
}