namespace Nps.Core.Entities
{
    /// <summary>
    /// 数据归属组
    /// </summary>
    public interface IHasGroup
    {
        /// <summary>
        /// 数据归属组 为数据做数据权限提供方便
        /// </summary>
        string GroupId { get; set; }
    }
}