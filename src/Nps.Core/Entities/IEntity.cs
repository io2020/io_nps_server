namespace Nps.Core.Entities
{
    /// <summary>
    /// 定义实体基类接口
    /// </summary>
    public interface IEntity
    {

    }

    /// <summary>
    /// 定义实体主键
    /// </summary>
    /// <typeparam name="TKey">主键类型</typeparam>
    public interface IEntity<TKey> : IEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        TKey Id { get; set; }
    }
}