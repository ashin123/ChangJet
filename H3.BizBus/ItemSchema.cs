using System;
using System.Xml;
using System.Xml.Serialization;

namespace H3.BizBus
{
    /// <summary>
    /// 每个属性的Schema
    /// </summary>
    [Serializable]
    public class ItemSchema : NamedItemSchema
    {
        /// <summary>
        /// 创建包含子结构的项的时候使用
        /// </summary>
        /// <param name="name">项名称</param>
        /// <param name="displayName">显示名称</param>
        /// <param name="bizDataType">类型</param>
        /// <param name="childSchema">如果类型是BizStructure或者BizStructureArray，那么该属性有效，表示子结构</param>
        public ItemSchema(
            string name,
            string displayName,
            BizDataType bizDataType,
            BizStructureSchema childSchema)
            : base(
                name,
                displayName,
                bizDataType,
                UnlimitedMaxLength,
                null)
        {
            this._ChildSchema = childSchema;
        }

        /// <summary>
        /// 创建不包含子结构的对象的项
        /// </summary>
        /// <param name="name">项名称</param>
        /// <param name="displayName">显示名称</param>
        /// <param name="bizDataType">类型</param>
        /// <param name="maxLength">最大长度，比如字符串类型，是可以设置长度限制的</param>
        /// <param name="DefaultValue">默认值</param>
        public ItemSchema(
            string name,
            string displayName,
            BizDataType bizDataType,
            int maxLength,
            object DefaultValue)
            : base(
                name,
                displayName,
                bizDataType,
                maxLength,
                DefaultValue)
        {
            this._ChildSchema = null;
        }

        /// <summary>
        /// 创建不包含子结构的对象的项
        /// </summary>
        /// <param name="name">项名称</param>
        /// <param name="displayName">显示名称</param>
        /// <param name="bizDataType">类型</param>
        public ItemSchema(
            string name,
            string displayName,
            BizDataType bizDataType)
            : base(
                name,
                displayName,
                bizDataType)
        {
            this._ChildSchema = null;
        }

        /// <summary>
        /// 更新包含子项的项模式
        /// </summary>
        /// <param name="name">项名称</param>
        /// <param name="displayName">显示名称</param>
        /// <param name="bizDataType">类型</param>
        /// <param name="childSchema">如果类型是BizStructure或者BizStructureArray，那么该属性有效，表示子结构</param>
        public void Update(
            string name,
            string displayName,
            BizDataType bizDataType,
            BizStructureSchema childSchema)
        {
            base.Update(name, displayName, bizDataType, UnlimitedMaxLength, null);
            this._ChildSchema = childSchema;
        }

        /// <summary>
        /// 更新不包含子结构的对象的项
        /// </summary>
        /// <param name="name">项名称</param>
        /// <param name="displayName">显示名称</param>
        /// <param name="bizDataType">类型</param>
        /// <param name="maxLength">最大长度，比如字符串类型，是可以设置长度限制的</param>
        /// <param name="DefaultValue">默认值</param>
        public void Update(
            string name,
            string displayName,
            BizDataType bizDataType,
            int maxLength,
            object defaultValue)
        {
            base.Update(name, displayName, bizDataType, maxLength, DefaultValue);
        }

        private BizStructureSchema _ChildSchema = null;
        /// <summary>
        /// 子对象模式
        /// </summary>
        public BizStructureSchema ChildSchema
        {
            get
            {
                return this._ChildSchema;
            }
        }

    }
}
