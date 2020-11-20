using FreeSql;

namespace Nps.Core.Repositories
{
    /// <summary>
    /// 定义FreeSql ORM仓储
    /// </summary>
    public interface IFreeSqlRepository<TEntity> : IFreeSqlRepository<TEntity, long>
        where TEntity : class
    {

    }

    /// <summary>
    /// 定义FreeSql ORM仓储
    /// 当需要给公共仓储增加方法时，在此方法中增加
    /// </summary>
    public interface IFreeSqlRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey>
        where TEntity : class
    {

    }
}