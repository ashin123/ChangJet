using System;

namespace H3.BizBus
{
    /// <summary>
    /// 要返回的列的设置
    /// </summary>
    [Serializable]
    public class ReturnColumn
    {
        public ReturnColumn(string columnName, string alias)
        {
            this._ColumnName = columnName;
            this._Alias = alias;
        }

        private string _ColumnName;
        /// <summary>
        /// 列名称，这里不包含表名
        /// </summary>
        public string ColumnName
        {
            get
            {
                return this._ColumnName;
            }
        }

        private string _Alias;
        /// <summary>
        /// 列别名
        /// </summary>
        public string Alias
        {
            get
            {
                return this._Alias;
            }
        }
    }
}
