using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;

namespace H3.BizBus
{
    /// <summary>
    /// 过滤器，类似于SQL的SELECT语句，用于GetList方法获得满足条件的对象
    /// </summary>
    [System.Serializable]
    public class Filter
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Filter()
        {
        }

        #region 父的过滤条件

        private Filter _ParentFilter = null;
        /// <summary>
        /// 父对象的过滤条件，当我们需要过滤出子对象的时候，有的时候需要传入父对象的条件，比如查询订单明细的时候，可能需要订单头上的制单时间作为查询条件
        /// </summary>
        public Filter ParentFilter
        {
            get
            {
                return this._ParentFilter;
            }
            set
            {
                this._ParentFilter = value;
            }
        }

        #endregion

        #region Row Num值

        private int _FromRowNum = -1;
        /// <summary>
        /// 从第N条记录开始，基数是0，不含第N条
        /// </summary>
        public int FromRowNum
        {
            get
            {
                return this._FromRowNum;
            }
            set
            {
                this._FromRowNum = value;
            }
        }

        /// <summary>
        /// 一次请求，最大的返回记录的数量
        /// </summary>
        public const int DefaultMaxSize = 1000;

        private int _ToRowNum = DefaultMaxSize;
        /// <summary>
        /// 到第M条，基数是0，不包含第M条
        /// </summary>
        public int ToRowNum
        {
            get
            {
                return this._ToRowNum;
            }
            set
            {
                this._ToRowNum = value;
            }
        }

        #endregion

        #region 查询条件

        private Matcher _Matcher = null;
        /// <summary>
        /// 该对象的匹配条件
        /// </summary>
        public Matcher Matcher
        {
            get
            {
                return this._Matcher;
            }
            set
            {
                this._Matcher = value;
            }
        }

        #endregion

        #region 排序

        private List<SortBy> _SortByCollection = new List<SortBy>();
        /// <summary>
        /// 过滤器的排序方法
        /// </summary>
        public SortBy[] SortByCollection
        {
            get
            {
                return this._SortByCollection.ToArray();
            }
            set
            {
                this._SortByCollection = new List<SortBy>();
                for (int i = 0; i < value.Length; i++)
                {
                    this._SortByCollection.Add(value[i]);
                }
            }
        }

        /// <summary>
        /// 添加排序字段
        /// </summary>
        /// <param name="itemName">排序的字段的名称</param>
        /// <param name="direction">从大到小或者从小到大</param>
        /// <returns>如果添加成功，则返回true，否则返回false</returns>
        public bool AddSortBy(string itemName, SortDirection direction)
        {
            SortBy sortBy = new SortBy(itemName, direction);
            return this.AddSortBy(sortBy);
        }

        /// <summary>
        /// 添加排序字段
        /// </summary>
        /// <param name="sortBy">排序字段的信息</param>
        /// <returns>如果没有重复的排序，则返回true，否则返回false</returns>
        public bool AddSortBy(SortBy sortBy)
        {
            if (sortBy == null)
            {
                return false;
            }
            // 检查在集合中是否已经存在
            foreach (SortBy iterator in this._SortByCollection)
            {
                if (iterator.ItemName == sortBy.ItemName)
                {
                    return false;
                }
            }
            this._SortByCollection.Add(sortBy);
            return true;
        }

        /// <summary>
        /// 删除排序字段
        /// </summary>
        /// <param name="index">要删除的排序索引号</param>
        /// <returns>如果删除成功，则返回true，否则返回false</returns>
        public bool RemoveSortBy(int index)
        {
            if (index < 0 || index > this._SortByCollection.Count - 1)
            {
                return false;
            }
            this._SortByCollection.RemoveAt(index);
            return true;
        }

        #endregion

        /// <summary>
        /// 要返回的列的集合，如果不为空，则表示只需要返回其中的列；否则，则表示采用默认的返回方式。这个属性与ReturnColumns是相通的，差别仅仅是这个不显示别名
        /// </summary>
        public string[] ReturnItems
        {
            get
            {
                List<string> columns = new List<string>();
                if (this.ReturnColumns != null)
                {
                    foreach (ReturnColumn c in this.ReturnColumns)
                    {
                        columns.Add(c.ColumnName);
                    }
                }
                return columns.ToArray();
            }
            set
            {
                List<ReturnColumn> columns = new List<ReturnColumn>();
                if (value != null)
                {
                    foreach (string c in value)
                    {
                        ReturnColumn column = new ReturnColumn(c, null);
                        columns.Add(column);
                    }
                }
                this._ReturnColumns = columns.ToArray();
            }
        }

        private ReturnColumn[] _ReturnColumns;
        /// <summary>
        /// 要返回的列的集合，如果不为空，则表示只需要返回其中的列；否则，则表示采用默认的返回方式。这个属性与ReturnItems是相通的，差别仅仅是这个显示别名
        /// </summary>
        public ReturnColumn[] ReturnColumns
        {
            get
            {
                return this._ReturnColumns;
            }
            set
            {
                this._ReturnColumns = value;
            }
        }

        private bool _RequireCount = false;
        /// <summary>
        /// 是否需要返回符合条件的记录的总数，通常对于分页显示记录的时候，是需要获得总数的
        /// </summary>
        public bool RequireCount
        {
            get
            {
                return this._RequireCount;
            }
            set
            {
                this._RequireCount = value;
            }
        }

    }
}
