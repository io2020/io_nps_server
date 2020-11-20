using FreeSql;
using Nps.Core.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nps.Data.FreeSql
{
    /// <summary>
    /// FreeSql扩展-集合列表，分页
    /// </summary>
    public static partial class FreeSqlCollectionExtension
    {
        /// <summary>
        /// 分页查询返回ISelect对象
        /// </summary>
        /// <typeparam name="TEntity">查询实体对象</typeparam>
        /// <param name="source">查询数据源</param>
        /// <param name="input">分页信息</param>
        /// <param name="count">查询数据总数</param>
        /// <returns>返回ISelect对象</returns>
        public static ISelect<TEntity> ToPaging<TEntity>(this ISelect<TEntity> source, PagingInput input, out long count) where TEntity : class
        {
            return source.Count(out count).Page(input.PageIndex, input.PageSize);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="TEntity">查询实体对象</typeparam>
        /// <param name="source">查询数据源</param>
        /// <param name="input">分页信息</param>
        /// <param name="count">查询数据总数</param>
        /// <returns>返回分页查询结果</returns>
        public static List<TEntity> ToPagingList<TEntity>(this ISelect<TEntity> source, PagingInput input, out long count) where TEntity : class
        {
            return source.Count(out count).Page(input.PageIndex, input.PageSize).ToList();
        }

        /// <summary>
        /// 异步分页查询
        /// </summary>
        /// <typeparam name="TEntity">查询实体对象</typeparam>
        /// <param name="source">查询数据源</param>
        /// <param name="input">分页信息</param>
        /// <param name="count">查询数据总数</param>
        /// <returns>返回分页查询结果</returns>
        public static Task<List<TEntity>> ToPagingListAsync<TEntity>(this ISelect<TEntity> source, PagingInput input, out long count) where TEntity : class
        {
            return source.Count(out count).Page(input.PageIndex, input.PageSize).ToListAsync();
        }

        /// <summary>
        /// 分页查询，返回输出结果数据集
        /// </summary>
        /// <typeparam name="TEntity">查询实体对象</typeparam>
        /// <typeparam name="TResult">输出结果对象</typeparam>
        /// <param name="source">查询数据源</param>
        /// <param name="input">分页信息</param>
        /// <param name="count">查询数据总数</param>
        /// <returns>分页查询，返回输出结果数据集</returns>
        public static List<TResult> ToPagingList<TEntity, TResult>(this ISelect<TEntity> source, PagingInput input, out long count) where TEntity : class
        {
            return source.Count(out count).Page(input.PageIndex, input.PageSize).ToList<TResult>();
        }

        /// <summary>
        /// 异步分页查询，返回输出结果数据集
        /// </summary>
        /// <typeparam name="TEntity">查询实体对象</typeparam>
        /// <typeparam name="TResult">输出结果对象</typeparam>
        /// <param name="source">查询数据源</param>
        /// <param name="input">分页信息</param>
        /// <param name="count">查询数据总数</param>
        /// <returns>分页查询，返回输出结果数据集</returns>
        public static Task<List<TResult>> ToPagingListAsync<TEntity, TResult>(this ISelect<TEntity> source, PagingInput input, out long count) where TEntity : class
        {
            return source.Count(out count).Page(input.PageIndex, input.PageSize).ToListAsync<TResult>();
        }

        /// <summary>
        /// 输出分页结果对象
        /// </summary>
        /// <typeparam name="TEntity">输出数据对象</typeparam>
        /// <param name="list">输出数据集</param>
        /// <param name="count">查询数据总数</param>
        /// <returns>输出分页结果对象</returns>
        public static PagingOutput<TEntity> ToPagingOutput<TEntity>(this List<TEntity> list, long count) where TEntity : class
        {
            return new PagingOutput<TEntity>(list, count);
        }
    }
}