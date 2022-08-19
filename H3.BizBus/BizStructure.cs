using System;
using System.Collections.Generic;
using System.Text;

namespace H3.BizBus
{
    /// <summary>
    /// 业务结构，只是用来传递可自解析的数据
    /// </summary>
    [Serializable]
    public class BizStructure
    {
        /// <summary>
        /// 新建业务结构对象的构造函数
        /// </summary>
        /// <param name="schema">业务结构模式</param>
        public BizStructure(BizStructureSchema schema)
        {
            this._BizStructure(schema);
        }

        private void _BizStructure(BizStructureSchema schema)
        {
            this._Schema = schema;

            string[] names = schema.GetNames();
            if (names != null)
            {
                foreach (string name in names)
                {
                    this.AddValue(name, schema.GetItem(name).DefaultValue);
                }
            }
        }

        /// <summary>
        /// (属性名称, 属性值)
        /// </summary>
        private Dictionary<string, object> Table = new Dictionary<string, object>();

        /// <summary>
        /// 向数据值表里添加值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        /// <returns>如果参数合法，并且原来的记录不存在，那么添加，并返回true；否则返回false</returns>
        private bool AddValue(string name, object value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }
            else if (this.Table.ContainsKey(name))
            {
                return false;
            }
            else
            {
                this.Table.Add(name, value);
                return true;
            }
        }

        /// <summary>
        /// 根据属性名称获得属性值，设置值的时候，检查类型
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <returns>属性值</returns>
        public object this[string name]
        {
            get
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentNullException();
                }
                else if (this._GetItemSchema(name) == null)
                {
                    throw new ArgumentOutOfRangeException(string.Format("属性\"{0}\"不存在", name));
                }
                else if (!this.Table.ContainsKey(name))
                {
                    return null;
                }
                else
                {
                    return this.Table[name];
                }
            }
            set
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentNullException();
                }
                NamedItemSchema item = this._GetItemSchema(name);
                if (item == null)
                {
                    throw new ArgumentOutOfRangeException(string.Format("属性\"{0}\"不存在", name));
                }

                // 检查类型是否匹配
                object convertedValue = null;
                if (!Utility.Convert(value, item.RealType, out convertedValue))
                {
                    // 转换失败，抛出异常
                    throw new InvalidCastException(string.Format("数据\"{0}\"无法转化为类型\"{1}\"",
                        value,
                        item.DataType.ToString(),
                        item.RealType));
                }

                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentNullException();
                }
                else if (!this.Table.ContainsKey(name))
                {
                    throw new ArgumentOutOfRangeException(string.Format("属性\"{0}\"不存在", name));
                }
                else
                {
                    this.Table[name] = convertedValue;
                }
            }
        }

        private BizStructureSchema _Schema;
        /// <summary>
        /// 业务结构模式
        /// </summary>
        public BizStructureSchema Schema
        {
            get
            {
                return this._Schema;
            }
        }

        /// <summary>
        /// 根据名称获得项模式
        /// </summary>
        /// <param name="Name">项名称</param>
        /// <returns>项模式</returns>
        private NamedItemSchema _GetItemSchema(string Name)
        {
            return this._Schema.GetItem(Name);
        }

    }
}
