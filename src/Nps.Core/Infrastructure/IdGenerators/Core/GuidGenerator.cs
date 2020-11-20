using Nps.Core.Infrastructure.IdGenerators.Ids;
using System;

namespace Nps.Core.Infrastructure.IdGenerators.Core
{
    /// <summary>
    /// 有序Guid生成器
    /// </summary>
    public class GuidGenerator : IGuidGenerator
    {
        private readonly SequentialGuid _sequentialGuid;

        /// <summary>
        /// 获取<see cref="GuidGenerator"/>类型的实例
        /// </summary>
        public GuidGenerator(SequentialGuid sequentialGuid)
        {
            Check.NotNull(sequentialGuid, nameof(SequentialGuid));
            _sequentialGuid = sequentialGuid;
        }

        /// <summary>
        /// 创建ID
        /// </summary>
        /// <returns></returns>
        public Guid Create()
        {
            return _sequentialGuid.Create();
        }
    }
}