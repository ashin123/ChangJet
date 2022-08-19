using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChangJet.Models
{
    public class VoucherModel
    {
        #region 一级节点
        /// <summary>
        /// 凭证分类，默认是100001 
        /// 100001 记 100009 银 100007 银付 100006 银收 100008 现 100005 现付 100004 现收 100003 付 100002 收 100010 转
        /// </summary>
        public int acctgTransCategoryId { get; set; }

        /// <summary>
        /// 凭证期间
        /// </summary>
        public string acctgPeriod { get; set; }

        /// <summary>
        /// 业务事务类型，100501为凭证，写死即可
        /// </summary>
        public int bizTypeId { get; set; }

        /// <summary>
        /// 凭证编号(默认三位，不要超过五位)
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 数据来源：写死AcctgTrans即可
        /// </summary>
        public string boName { get; set; }

        /// <summary>
        /// 凭证日期，时间戳格式13位
        /// </summary>
        public long bizDate { get; set; }

        /// <summary>
        /// 科目识别，只为外部接口服务，固定写true即可
        /// </summary>
        public bool isCodeType { get; set; }

        /// <summary>
        /// 凭证明细
        /// </summary>
        public List<Details> details { get; set; }

        /// <summary>
        /// 只为外部接口服务，固定写true即可
        /// </summary>
        public bool categoryCodeExist { get; set; }

        /// <summary>
        /// false
        /// </summary>
        public bool isFinal { get; set; }

        /// <summary>
        /// 经办人
        /// </summary>
        public string origCreatedUserName { get; set; }
        #endregion

        #region 二级节点
        public class Details
        {
            /// <summary>
            /// 摘要
            /// </summary>
            public string comments { get; set; }
            /// <summary>
            /// 借方数量
            /// </summary>
            public int postedDrQty { get; set; }
            /// <summary>
            /// 贷方数量
            /// </summary>
            public int postedCrQty { get; set; }
            /// <summary>
            /// 顺序号
            /// </summary>
            public int sequenceNum { get; set; }
            /// <summary>
            /// 借方外币
            /// </summary>
            public decimal postedDr { get; set; }
            /// <summary>
            /// 贷方外币
            /// </summary>
            public decimal postedCr { get; set; }
            /// <summary>
            /// 借方金额
            /// </summary>
            public decimal basePostedDr { get; set; }
            /// <summary>
            /// 贷方金额
            /// </summary>
            public decimal basePostedCr { get; set; }
            /// <summary>
            /// 单价
            /// </summary>
            public decimal price { get; set; }
            /// <summary>
            /// 辅助核算信息
            /// </summary>
            public GlSubAccount glSubAccount { get; set; }
            /// <summary>
            /// 科目信息
            /// </summary>
            public GlAccount glAccount { get; set; }
        }

        #endregion

        #region 三级节点_glAccount
        public class GlAccount
        {
            /// <summary>
            /// 科目编号
            /// </summary>
            public string code { get; set; }
            /// <summary>
            /// 借贷方向：标示科目借贷类型，借方为1，贷方为-1
            /// </summary>
            public int drCrDirection { get; set; }
            /// <summary>
            /// 是否是末级，是写true,否写false
            /// </summary>
            public bool hasSubsidiaryAccounting { get; set; }
            /// <summary>
            /// 是否是辅助核算，是写true,否写false
            /// </summary>
            public bool isLeafNode { get; set; }
        }
        #endregion

        #region 三级节点_GlSubAccount
        public class GlSubAccount
        {
            /// <summary>
            /// 存货辅助核算编码
            /// </summary>
            public string productCode { get; set; }
            /// <summary>
            /// 项目编号
            /// </summary>
            public string projectCode { get; set; }
            /// <summary>
            /// 部门辅助核算
            /// </summary>
            public string departmentCode { get; set; }
            /// <summary>
            /// 客户辅助核算
            /// </summary>
            public string customerCode { get; set; }
            /// <summary>
            /// 员工辅助核算
            /// </summary>
            public string employeeCode { get; set; }
            /// <summary>
            /// 供应商辅助核算
            /// </summary>
            public string vendorCode { get; set; }
        }
        #endregion
    }

    public class H3Yun
    {
        public string ObjectId { get; set; }
    }
}