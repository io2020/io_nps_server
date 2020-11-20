using Nps.Core.Infrastructure.IdGenerators.Ids;

namespace Nps.Core.Infrastructure.IdGenerators.Core
{
    /// <summary>
    /// 雪花ID生成器
    /// </summary>
    public class LongIdGenerator : ILongIdGenerator
    {
        //雪花算法
        private readonly SnowflakeId _snowflakeId;

        /// <summary>
        /// 构造函数，初始化一个<see cref="LongIdGenerator"/>实例
        /// </summary>
        /// <param name="snowflakeId">雪花算法</param>
        public LongIdGenerator(SnowflakeId snowflakeId)
        {
            Check.NotNull(snowflakeId, nameof(SnowflakeId));
            _snowflakeId = snowflakeId;
        }

        /// <summary>
        /// 生成雪花Id
        /// </summary>
        /// <returns></returns>
        public long Create()
        {
            return _snowflakeId.NextId();
        }
    }
}