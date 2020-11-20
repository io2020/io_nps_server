using FreeSql;

namespace Nps.Data.FreeSql
{
    /// <summary>
    /// FreeSql扩展--设置实体属性
    /// </summary>
    public static partial class FreeSqlConfigEntityExtension
    {
        /// <summary>
        /// 设置实体属性
        /// </summary>
        /// <param name="codeFirst">ICodeFirst对象</param>
        /// <returns>返回ICodeFirst对象</returns>
        public static ICodeFirst ConfigEntity(this ICodeFirst codeFirst)
        {
            return codeFirst;
        }
    }
}