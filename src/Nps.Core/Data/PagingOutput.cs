using System.Collections.Generic;

namespace Nps.Core.Data
{
    /// <summary>
    /// 分页数据输出
    /// </summary>
    /// <typeparam name="TData">数据集</typeparam>
    public class PagingOutput<TData>
    {
        /// <summary>
        /// 总数
        /// </summary>
        public long Total { get; set; }

        /// <summary>
        /// 分页查询数据列表
        /// </summary>
        public List<TData> List { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public PagingOutput()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="datas">分页数据</param>
        public PagingOutput(List<TData> datas)
        {
            Total = datas.Count;
            List = datas;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="datas">分页数据</param>
        /// <param name="total">数据总数</param>
        public PagingOutput(List<TData> datas, long total)
            : this(datas)
        {
            Total = total;
        }
    }
}