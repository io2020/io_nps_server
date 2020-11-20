namespace Nps.Core.Data
{
    /// <summary>
    /// 分页信息输入
    /// </summary>
    public class PagingInput
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { set; get; } = 20;
    }

    /// <summary>
    /// 分页查询条件
    /// </summary>
    public class PagingInput<TFilter> : PagingInput
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        public TFilter Filter { get; set; }
    }
}