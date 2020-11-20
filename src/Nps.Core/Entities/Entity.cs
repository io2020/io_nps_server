using FreeSql.DataAnnotations;
using Nps.Core.Aop.Attributes;
using System;

namespace Nps.Core.Entities
{
    /// <summary>
    /// 定义实体基类
    /// </summary>
    /// <typeparam name="TKey">泛型主键</typeparam>
    [Serializable]
    public abstract class Entity<TKey> : IEntity<TKey>
    {
        /// <summary>
        /// 主键Id，id启用生成器标识
        /// </summary>
        [IdGenerator, Column(IsPrimary = true, Position = 1)]//IsIdentity = true //去掉自增属性，改用雪花算法Id
        public virtual TKey Id { get; set; }

        /// <summary>
        /// 重载
        /// </summary>
        /// <returns>ToString()</returns>
        public override string ToString()
        {
            return $"[ENTITY: {GetType().Name}] Id = {Id}";
        }
    }

    /// <summary>
    /// 定义主键为long类型实体基类
    /// </summary>
    [Serializable]
    public abstract class Entity : Entity<long>
    {

    }
}