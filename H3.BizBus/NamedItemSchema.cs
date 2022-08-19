using System;
using System.Xml;
using System.Xml.Serialization;

namespace H3.BizBus
{
    /// <summary>
    /// 项目模式，这个是业务对象属性模式和业务结构项目模式的基类。
    /// </summary>
    [Serializable]
    public abstract class NamedItemSchema
    {
        /// <summary>
        /// 创建的时候使用
        /// </summary>
        /// <param name="name">项目名称</param>
        /// <param name="displayName">显示名称</param>
        /// <param name="bizDataType">类型</param>
        /// <param name="maxLength">最大长度</param>
        /// <param name="defaultValue">默认值</param>
        public NamedItemSchema(
            string name,
            string displayName,
            BizDataType bizDataType,
            int maxLength,
            object defaultValue)
        {
            this._Name = name;
            this._DisplayName = displayName;
            this._DataType = bizDataType;
            this._MaxLength = maxLength;
            // 检查默认值是否符合类型要求
            this._DefaultValue = defaultValue;
        }

        /// <summary>
        /// 创建的时候使用
        /// </summary>
        /// <param name="name">项目名称</param>
        /// <param name="displayName">显示名称</param>
        /// <param name="bizDataType">类型</param>
        public NamedItemSchema(
            string name,
            string displayName,
            BizDataType bizDataType)
        {
            this._Name = name;
            this._DisplayName = displayName;
            this._DataType = bizDataType;
        }

        /// <summary>
        /// 更新项目属性模式
        /// </summary>
        /// <param name="name">项目名称</param>
        /// <param name="displayName">显示名称</param>
        /// <param name="bizDataType">类型</param>
        /// <param name="maxLength">最大长度</param>
        /// <param name="defaultValue">默认值</param>
        public void Update(
            string name,
            string displayName,
            BizDataType bizDataType,
            int maxLength,
            object defaultValue)
        {
            this._Name = name;
            this._DisplayName = displayName;
            this._DataType = bizDataType;
            this._MaxLength = maxLength;
            // 检查默认值是否符合类型要求
            this._DefaultValue = defaultValue;
        }

        private string _Name = null;
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name
        {
            get
            {
                return this._Name;
            }
        }

        private string _DisplayName = null;
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName
        {
            get
            {
                return this._DisplayName;
            }
            set
            {
                if (this._DisplayName != value)
                {
                    this._DisplayName = value;
                }
            }
        }

        ///// <summary>
        ///// 项目全名称
        ///// </summary>
        //public string FullName
        //{
        //    get
        //    {
        //        return this.DisplayName + "[" + this.Name + "]";
        //    }
        //}

        private BizDataType _DataType = BizDataType.ShortString;
        /// <summary>
        /// 数据类型
        /// </summary>
        public BizDataType DataType
        {
            get
            {
                return this._DataType;
            }
        }

        /// <summary>
        /// 无长度限制的长度
        /// </summary>
        public const int UnlimitedMaxLength = 0;

        private int _MaxLength = UnlimitedMaxLength;
        /// <summary>
        /// 最大长度。0表示不限制长度
        /// </summary>
        public int MaxLength
        {
            get
            {
                return this._MaxLength;
            }
        }

        /// <summary>
        /// .Net数据类型
        /// </summary>
        public System.Type RealType
        {
            get
            {
                return Utility.ToRealType(this.DataType);
            }
        }

        private object _DefaultValue = null;
        /// <summary>
        /// 默认值
        /// </summary>
        public object DefaultValue
        {
            get
            {
                return this._DefaultValue;
            }
        }


        public override string ToString()
        {
            return this.Name + ", " + this.DataType;
        }
    }
}
