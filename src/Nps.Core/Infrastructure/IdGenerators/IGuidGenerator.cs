using System;

namespace Nps.Core.Infrastructure.IdGenerators
{
    /// <summary>
    /// 定义有序GUID生成器
    /// </summary>
    public interface IGuidGenerator : IIdGenerator<Guid>
    {

    }
}