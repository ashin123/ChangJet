namespace H3.BizBus
{
    // ERROR, 这里需要注意下，看看能否根H3.DataModel.Math里的匹配符合并
    /// <summary>
    /// 比较用的通配符
    /// </summary>
    public enum ComparisonOperatorType
    {
        /// <summary>
        /// 大于
        /// </summary>
        Above = 0,
        /// <summary>
        /// 大于等于
        /// </summary>
        NotBelow = 1,
        /// <summary>
        /// 等于
        /// </summary>
        Equal = 2,
        /// <summary>
        /// 小于等于
        /// </summary>
        NotAbove = 3,
        /// <summary>
        /// 小于
        /// </summary>
        Below = 4,
        /// <summary>
        /// 不等于
        /// </summary>
        NotEqual = 5,

        /// <summary>
        /// 在某个范围内
        /// </summary>
        In = 6,
        /// <summary>
        /// 不在某个范围内
        /// </summary>
        NotIn = 7,

        /// <summary>
        /// 比较符左边的对象包含右边的对象，比如：{1, 2, 3} Contain {2} == true，如果是字符串的话，则调用string.Contains这样的逻辑
        /// </summary>
        Contains = 8,

        /// <summary>
        /// 以某个字符串为开始
        /// </summary>
        StartWith = 13,
        /// <summary>
        /// 以某个字符串为结束
        /// </summary>
        EndWith = 14,

        /// <summary>
        /// 是否是Null
        /// </summary>
        IsNull = 18,
        /// <summary>
        /// 是否不是Null
        /// </summary>
        NotNull = 19,
        /// <summary>
        /// 逻辑上是空，表示为Null、""、object[0]
        /// </summary>
        IsNone = 20,
        /// <summary>
        /// 逻辑上非空，表示不是Null、""、object[0]
        /// </summary>
        NotNone = 21
    }
}
