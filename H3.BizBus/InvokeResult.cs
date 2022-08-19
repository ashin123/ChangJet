using System;

namespace H3.BizBus
{
    /// <summary>
    /// 调用返回结果
    /// </summary>
    [Serializable]
    public class InvokeResult : InvokeResultBase
    {
        public InvokeResult(int resultCode, string message, BizStructure data)
        {
            this.Code = resultCode;
            this.Message = message;
            this.Data = data;
        }

        /// <summary>
        /// 调用接口后，返回的数据
        /// </summary>
        public BizStructure Data
        {
            set;
            get;
        }

        public override BizStructureSchema Schema
        {
            get
            {
                if (this.Data != null)
                {
                    return this.Data.Schema;
                }
                return null;
            }
        }
    }
}
