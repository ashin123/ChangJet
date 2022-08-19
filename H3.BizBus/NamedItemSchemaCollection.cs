using System;
using System.Collections.Generic;

namespace H3.BizBus
{
    /// <summary>
    /// 命名项模式集合
    /// </summary>
    [Serializable]
    public class NamedItemSchemaCollection<T>
    {
        public NamedItemSchemaCollection()
        {
        }

        /// <summary>
        /// (Name.ToLower(), Item)
        /// </summary>
        private Dictionary<string, T> Table = new Dictionary<string, T>();
        private List<string> Names = new List<string>();

        private T[] _Items = null;
        /// <summary>
        /// 所有的命名项模式
        /// </summary>
        public virtual T[] Items
        {
            get
            {
                if (this._Items == null)
                {
                    T[] itemArray = new T[this.Table.Count];
                    int count = 0;
                    foreach (string key in this.Table.Keys)
                    {
                        T item = this.Table[key];
                        itemArray[count] = item;
                        count++;
                    }
                    this._Items = itemArray;
                }
                return this._Items;
            }
        }

        /// <summary>
        /// 集合中包含的项的数量
        /// </summary>
        public int Count
        {
            get
            {
                return this.Table.Count;
            }
        }

        /// <summary>
        /// 是否存在某个项
        /// </summary>
        /// <param name="itemName">项名称</param>
        /// <returns>如果存在，则返回true，否则返回false</returns>
        public bool Exists(string itemName)
        {
            return this.GetItem(itemName) != null;
        }

        /// <summary>
        /// 添加一个项模式
        /// </summary>
        /// <param name="item">项模式</param>
        /// <returns>如果项模式的名称合法，并且名称不重复，则返回true，否则返回false</returns>
        public virtual bool Add(T item)
        {
            NamedItemSchema i = (NamedItemSchema)(object)item;
            if (item == null || string.IsNullOrEmpty(i.Name))
            {
                return false;
            }
            else if (this.Table.ContainsKey(i.Name.ToLower()))
            {
                return false;
            }
            else
            {
                this.Table.Add(i.Name.ToLower(), item);
                this.Names.Add(i.Name);

                this._Items = null;

                return true;
            }
        }

        /// <summary>
        /// 根据名称删除一个项模式
        /// </summary>
        /// <param name="itemName">项名称</param>
        public virtual void Remove(string itemName)
        {
            if (!string.IsNullOrEmpty(itemName) && this.Table.ContainsKey(itemName.ToLower()))
            {
                this.Table.Remove(itemName.ToLower());
                this.Names.Remove(itemName);

                this._Items = null;
            }
        }

        /// <summary>
        /// 清除所有项模式
        /// </summary>
        public virtual void Clear()
        {
            this.Table.Clear();
            this.Names.Clear();

            this._Items = null;
        }

        /// <summary>
        /// 根据项名称获得项模式
        /// </summary>
        /// <param name="itemName">项名称</param>
        /// <returns>项模式</returns>
        public virtual T GetItem(string itemName)
        {
            if (string.IsNullOrEmpty(itemName))
            {
                return default(T);
            }
            else if (this.Table.ContainsKey(itemName.ToLower()))
            {
                return this.Table[itemName.ToLower()];
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// 根据名称获得项目模式
        /// </summary>
        /// <param name="Name">名称</param>
        /// <returns>项目模式</returns>
        public NamedItemSchema GetNamedItem(string itemName)
        {
            return (NamedItemSchema)(object)this.GetItem(itemName);
        }

        /// <summary>
        /// 获得所有的项名称
        /// </summary>
        /// <returns>所有的项名称</returns>
        public virtual string[] GetNames()
        {
            return this.Names.ToArray();
        }

        /// <summary>
        /// 获得所有数据表支持的项
        /// </summary>
        /// <returns>所有数据表支持的项</returns>
        public T[] GetDataTableSupportedItems()
        {
            T[] items = this.Items;
            List<T> list = new List<T>();
            if (items != null)
            {
                foreach (T item in items)
                {
                    NamedItemSchema i = (NamedItemSchema)(object)item;
                    list.Add(item);
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// 根据项名称获得项类型
        /// </summary>
        /// <param name="itemName">项名称</param>
        /// <returns>项类型</returns>
        public BizDataType GetDataType(string itemName)
        {
            NamedItemSchema item = (NamedItemSchema)(object)this.GetItem(itemName);
            if (item == null)
            {
                throw new ArgumentOutOfRangeException();
            }
            return item.DataType;
        }

        /// <summary>
        /// 根据项名称获得项显示名称
        /// </summary>
        /// <param name="itemName">项名称</param>
        /// <returns>显示名称</returns>
        public string GetDisplayName(string itemName)
        {
            NamedItemSchema item = (NamedItemSchema)(object)this.GetItem(itemName);
            if (item == null)
            {
                throw new ArgumentOutOfRangeException();
            }
            return item.DisplayName;
        }

        /// <summary>
        /// 更新一个项模式
        /// </summary>
        /// <param name="oldItemName">项的旧名称</param>
        /// <param name="newItemSchema">新的项模式</param>
        /// <returns>如果项不存在，或者存在重名的话，则返回false，否则返回true</returns>
        public bool Update(string oldItemName, T newItemSchema)
        {
            NamedItemSchema i = (NamedItemSchema)(object)newItemSchema;
            if (newItemSchema == null || string.IsNullOrEmpty(i.Name) ||
                // 修改了名称，并且新名称已经存在
                (string.Compare(oldItemName, i.Name, true) != 0 && this.Table.ContainsKey(i.Name.ToLower())))
            {
                return false;
            }
            this.Remove(oldItemName);
            this.Add(newItemSchema);
            return true;
        }
    }
}
