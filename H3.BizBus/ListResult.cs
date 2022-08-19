using System;

namespace H3.BizBus
{
    /// <summary>
    /// 请求列表的时候返回的列表结果
    /// </summary>
    [Serializable]
    public class ListResult : InvokeResultBase
    {
        public ListResult(int resultCode, string message, BizStructure[] data, int count)
        {
            this.Code = resultCode;
            this.Message = message;
            this.Data = data;
            this.Count = count;
        }

        /// <summary>
        /// 返回的多个数据的数组
        /// </summary>
        public BizStructure[] Data
        {
            set;
            get;
        }

        /// <summary>
        /// 存在记录的记录总数
        /// </summary>
        public int Count
        {
            set;
            get;
        }

        public override BizStructureSchema Schema
        {
            get
            {
                // 找到第一个不为空的对象
                if (this.Data == null)
                {
                    return null;
                }
                foreach (BizStructure b in this.Data)
                {
                    if (b != null)
                    {
                        return b.Schema;
                    }
                }
                return null;
            }
        }
    }
}
