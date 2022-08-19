using System;
using System.Collections.Generic;

namespace H3.BizBus
{
    /// <summary>
    /// 过滤条件匹配单元的集合
    /// </summary>
    [Serializable]
    public class MatcherCollection : Matcher
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MatcherCollection()
        {
        }

        private List<Matcher> _Matchers = new List<Matcher>();
        /// <summary>
        /// 过滤条件匹配单元的集合
        /// </summary>
        public Matcher[] Matchers
        {
            get
            {
                return this._Matchers.ToArray();
            }
        }

        /// <summary>
        /// 添加一个匹配单元
        /// </summary>
        /// <param name="matcher">匹配单元</param>
        public void Add(Matcher matcher)
        {
            if (matcher != null && !this._Matchers.Contains(matcher))
            {
                this._Matchers.Add(matcher);
            }
        }

        /// <summary>
        /// 删除一个匹配单元
        /// </summary>
        /// <param name="index">要删除的索引号</param>
        public void Remove(int index)
        {
            if (index < 0 || index > this._Matchers.Count - 1)
            {
                return;
            }
            this._Matchers.RemoveAt(index);
        }

    }
}
