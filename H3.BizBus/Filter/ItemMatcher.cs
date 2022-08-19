using System;

namespace H3.BizBus
{
    /// <summary>
    /// 属性匹配器
    /// </summary>
    [Serializable]
    public class ItemMatcher : Matcher
    {
        /// <summary>
        /// 创建属性匹配器
        /// </summary>
        /// <param name="itemName">要匹配的项目的名称</param>
        /// <param name="comparisonOperator">匹配的比较符号</param>
        /// <param name="value">目标值</param>
        public ItemMatcher(
            string itemName,
            ComparisonOperatorType comparisonOperator,
            object value,
            bool isColumn = false)
        {
            this._ItemName = itemName;
            this._ComparisonOperator = comparisonOperator;
            if (value == null)
            {
                this._Value = null;
            }
            else if (value is Enum)
            {
                this._Value = (int)value;
            }
            else if (value is bool)
            {
                this._Value = ((bool)value) ? 1 : 0;
            }
            else
            {
                this._Value = value;
            }
            this._IsColumn = isColumn;
        }

        private string _ItemName;
        /// <summary>
        /// 要匹配的项目的名称
        /// </summary>
        public string ItemName
        {
            get
            {
                return this._ItemName;
            }
        }

        private ComparisonOperatorType _ComparisonOperator;
        /// <summary>
        /// 匹配的比较符号
        /// </summary>
        public ComparisonOperatorType ComparisonOperator
        {
            get
            {
                return this._ComparisonOperator;
            }
        }

        private object _Value;
        /// <summary>
        /// 目标值
        /// </summary>
        public object Value
        {
            get
            {
                return this._Value;
            }
        }
        private bool _IsColumn;
        /// <summary>
        /// ItemValue是否是列
        /// </summary>
        public bool IsColumn
        {
            get
            {
                return this._IsColumn;
            }
        }

    }
}
