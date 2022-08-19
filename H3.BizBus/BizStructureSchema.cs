using System;
using System.Xml;
using System.Xml.Serialization;

namespace H3.BizBus
{
    /// <summary>
    /// 业务结构的Schema
    /// </summary>
    [Serializable]
    public class BizStructureSchema : NamedItemSchemaCollection<ItemSchema>
    {
        /// <summary>
        /// 构造函数，创建一个业务结构模式的使用
        /// </summary>
        public BizStructureSchema()
        {
        }

        private string _Code;
        /// <summary>
        /// 标记这个业务结构是来自.Net/Java/SAP/其他系统的哪个类，这个类名称会被用来生成H3中对应的Schema的SchemaCode
        /// </summary>
        public string Code
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
        /// 为业务结构添加一个项目
        /// </summary>
        /// <param name="item">业务结构的项目</param>
        /// <returns>如果添加成功，则返回true；否则返回false</returns>
        public override bool Add(ItemSchema item)
        {
            return base.Add(item);
        }
    }
}
