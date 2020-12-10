namespace Nps.Infrastructure
{
    /// <summary>
    /// 检查常量
    /// </summary>
    public class CheckConsts
    {
        /// <summary>
        /// 参数{0}的值必须在{1}与{2}之间
        /// </summary>
        public const string ParameterCheck_Between = "参数{0}的值必须在{1}与{2}之间。";

        /// <summary>
        /// 参数{0}的值必须在{1}与{2}之间，且不能等于{3}
        /// </summary>
        public const string ParameterCheck_BetweenNotEqual = "参数{0}的值必须在{1}与{2}之间，且不能等于{3}。";

        /// <summary>
        /// 指定的目录路径{0}不存在
        /// </summary>
        public const string ParameterCheck_DirectoryNotExists = "指定的目录路径{0}不存在。";

        /// <summary>
        /// 指定的文件路径{0}不存在
        /// </summary>
        public const string ParameterCheck_FileNotExists = "指定的文件路径{0}不存在。";

        /// <summary>
        /// 集合{0}中不能包含null的项
        /// </summary>
        public const string ParameterCheck_NotContainsNull_Collection = "集合{0}中不能包含null的项";

        /// <summary>
        /// 参数{0}的值不能为Guid.Empty
        /// </summary>
        public const string ParameterCheck_NotEmpty_Guid = "参数{0}的值不能为Guid.Empty";

        /// <summary>
        /// 参数{0}的值必须大于{1}
        /// </summary>
        public const string ParameterCheck_NotGreaterThan = "参数{0}的值必须大于{1}。";

        /// <summary>
        /// 参数{0}的值必须大于或等于{1}
        /// </summary>
        public const string ParameterCheck_NotGreaterThanOrEqual = "参数{0}的值必须大于或等于{1}。";

        /// <summary>
        /// 参数{0}的值必须小于{1}
        /// </summary>
        public const string ParameterCheck_NotLessThan = "参数{0}的值必须小于{1}。";

        /// <summary>
        /// 参数{0}的值必须小于或等于{1}
        /// </summary>
        public const string ParameterCheck_NotLessThanOrEqual = "参数{0}的值必须小于或等于{1}。";

        /// <summary>
        /// 参数{0}不能为空引用
        /// </summary>
        public const string ParameterCheck_NotNull = "参数{0}不能为空引用。";

        /// <summary>
        /// 参数{0}不能为空引用或空集合
        /// </summary>
        public const string ParameterCheck_NotNullOrEmpty_Collection = "参数{0}不能为空引用或空集合。";

        /// <summary>
        /// 参数{0}不能为空引用或空字符串
        /// </summary>
        public const string ParameterCheck_NotNullOrEmpty_String = "参数{0}不能为空引用或空字符串。";
    }
}