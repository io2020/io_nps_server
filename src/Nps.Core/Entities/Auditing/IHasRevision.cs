namespace Nps.Core.Entities
{
    /// <summary>
    /// 乐观锁
    /// </summary>
    public interface IHasRevision
    {
        /// <summary>
        /// 乐观锁
        /// </summary>
        long Revision { get; set; }
    }
}