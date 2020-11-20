namespace Nps.Core.Entities
{
    /// <summary>
    /// 更新审计属性
    /// </summary>
    public interface IUpdateAuditEntity : IHasUpdateTime
    {
        /// <summary>
        /// 最后修改人Id
        /// </summary>
        long? UpdateUserId { get; set; }
    }
}