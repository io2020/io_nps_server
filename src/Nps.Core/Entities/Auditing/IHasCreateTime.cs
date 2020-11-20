using System;

namespace Nps.Core.Entities
{
    /// <summary>
    /// 创建时间
    /// </summary>
    public interface IHasCreateTime
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreateTime { get; set; }
    }
}