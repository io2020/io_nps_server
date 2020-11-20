namespace Nps.Core.Entities
{
    /// <summary>
    /// 创建审计属性
    /// </summary>
    public interface ICreateAuditEntity : IHasCreateTime
    {
        /// <summary>
        /// 创建者ID
        /// </summary>
        long CreateUserId { get; set; }
    }
}