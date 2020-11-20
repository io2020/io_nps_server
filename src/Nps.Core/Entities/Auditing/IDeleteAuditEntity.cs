namespace Nps.Core.Entities
{
    /// <summary>
    /// 删除审计属性
    /// </summary>
    public interface IDeleteAuditEntity : IHasDeleteTime, ISoftDelete
    {
        /// <summary>
        /// 删除人id
        /// </summary>
        long? DeleteUserId { get; set; }
    }
}