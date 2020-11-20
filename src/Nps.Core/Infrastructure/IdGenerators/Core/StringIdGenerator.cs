using Nps.Core.Infrastructure.IdGenerators.Ids;

namespace Nps.Core.Infrastructure.IdGenerators.Core
{
    /// <summary>
    /// ObjectId 生成器
    /// </summary>
    public class StringIdGenerator : IStringIdGenerator
    {
        /// <summary>
        /// 获取<see cref="StringIdGenerator"/>类型的实例
        /// </summary>
        public StringIdGenerator()
        {

        }

        /// <summary>
        /// 创建ID
        /// </summary>
        /// <returns></returns>
        public string Create()
        {
            return ObjectId.GenerateNewStringId();
        }
    }
}