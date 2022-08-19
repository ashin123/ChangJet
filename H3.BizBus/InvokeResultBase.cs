using System;

namespace H3.BizBus
{
    /// <summary>
    /// 调用接口的时候返回值的基类
    /// </summary>
    [Serializable]
    public abstract class InvokeResultBase
    {
        /// <summary>
        /// g构造函数
        /// </summary>
        public InvokeResultBase()
        {
        }

        private int _Code = (int)ErrorCode.Success;
        /// <summary>
        /// 获取或设置执行的结果的代码，0：成功；1：未命名的失败；其他：其他命名的错误代码。如果是外部系统调用H3的接口，那么这里返回的值跟H3.ErrorCode对应
        /// </summary>
        public int Code
        {
            get
            {
                return this._Code;
            }
            set
            {
                this._Code = value;
            }
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Message
        {
            set;
            get;
        }

        public abstract BizStructureSchema Schema
        {
            get;
        }
    }
}
