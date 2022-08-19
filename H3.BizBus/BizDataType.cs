using System;

namespace H3.BizBus
{
    /// <summary>
    /// 数据项的逻辑类型
    /// </summary>
    public enum BizDataType
    {
        /// <summary>
        /// 空值
        /// </summary>
        Unspecified = -1,

        /// <summary>
        /// 逻辑型
        /// </summary>
        Bool = 1,

        /// <summary>
        /// 日期型
        /// </summary>
        DateTime = 5,

        /// <summary>
        /// 双精度数值型
        /// </summary>
        Double = 7,

        /// <summary>
        /// 整数
        /// </summary>
        Int = 9,

        /// <summary>
        /// 长整数
        /// </summary>
        Long = 11,

        /// <summary>
        /// 长文本
        /// </summary>
        String = 13,
        /// <summary>
        /// 短文本
        /// </summary>
        ShortString = 14,

        /// <summary>
        /// 二进制流
        /// </summary>
        ByteArray = 20,

        ///// <summary>
        ///// 未指定类型的附件
        ///// </summary>
        //File = 24,

        /// <summary>
        /// 时间段型
        /// </summary>
        TimeSpan = 25,

        /// <summary>
        /// 参与者（单人）
        /// </summary>
        Unit = 26,
        /// <summary>
        /// 参与者（多人）
        /// </summary>
        UnitArray = 27,

        /// <summary>
        /// Html
        /// </summary>
        Html = 30,

        /// <summary>
        /// Xml
        /// </summary>
        Xml = 32,

        ///// <summary>
        ///// 业务对象
        ///// </summary>
        //BizObject = 40,
        ///// <summary>
        ///// 业务对象数组
        ///// </summary>
        //BizObjectArray = 41,
        /// <summary>
        /// 业务结构
        /// </summary>
        BizStructure = 42,
        /// <summary>
        /// 业务结构数组
        /// </summary>
        BizStructureArray = 43,

        /// <summary>
        /// 关联到其他的对象，这种字段在表单上通常是以开窗查询的形式出现
        /// </summary>
        Association = 50,

        /// <summary>
        /// 地图类型,存json格式：{Address:"",Point:{lat:32323.43,lng:323.232}}
        /// </summary>
        Map = 55,
        /// <summary>
        /// 地址类型,存json格式:{"Province":"福建省","City":"泉州市","Town":"惠安县","Detail":"32323"}
        /// </summary>
        Address
    }
}
