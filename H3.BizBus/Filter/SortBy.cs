using System;

namespace H3.BizBus
{
    /// <summary>
    /// 排序方式
    /// </summary>
    [Serializable]
    public class SortBy
    {
        /// <summary>
        /// 新建用的构造函数
        /// </summary>
        /// <param name="itemName">排序项目名称</param>
        /// <param name="direction">排序的方向</param>
        public SortBy(string itemName, SortDirection direction)
        {
            this._Direction = direction;
            this._ItemName = itemName;
        }

        private string _ItemName;
        /// <summary>
        /// 排序项目名称
        /// </summary>
        public string ItemName
        {
            get
            {
                return this._ItemName;
            }
        }

        private SortDirection _Direction;
        /// <summary>
        /// 排序的方向
        /// </summary>
        public SortDirection Direction
        {
            get
            {
                return this._Direction;
            }
        }

    }
}
