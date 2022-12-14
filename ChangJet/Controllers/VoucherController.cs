using ChangJet.Models;
using H3.BizBus;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace ChangJet.Controllers
{
    public class VoucherController : ApiController
    {
        // 请求地址
        private const string BASE_URL = "https://openapi.chanjet.com";
        private const string App_Key = "";
        private const string App_Secret = "";
        private static string openToken = ""; // 开放平台token

        /// <summary>
        /// 氚云对接
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage VoucherOperation(JObject param)
        {
            #region 返回结果参数
            List<BizStructure> listData = new List<BizStructure>();
            BizStructureSchema msgSchema1 = new BizStructureSchema();
            BizStructure returnBizStructure = null;
            string bResult = "false";
            #endregion

            try
            {
                BizStructureSchema msgSchema = new BizStructureSchema();
                msgSchema.Add(new ItemSchema("ObjectId", "氚云表单数据ID", BizDataType.String, 100, ""));
                msgSchema.Add(new ItemSchema("VoucherId", "生成的凭证ID", BizDataType.String, 100, ""));

                string jsonparam = JsonConvert.SerializeObject(param);
                //MessageHelper.CreateErrLog(jsonparam);
                if (!String.IsNullOrWhiteSpace(jsonparam))
                {

                    Dictionary<string, object> dic_schema_id = new Dictionary<string, object>();    // key:氚云表单数据ID  Value:生成的凭证ID
                    Dictionary<string, object> dicParam = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonparam);
                    openToken = dicParam["openToken"] + string.Empty;  // token

                    // 新增凭证
                    if (dicParam["methodName"] + string.Empty == "Add")
                    {

                        int i = 0;
                        string strResult = string.Empty;
                        List<VoucherModel> voucher = JsonConvert.DeserializeObject<List<VoucherModel>>(dicParam["VoucherCollection"] + string.Empty);
                        List<H3Yun> list_ObjectId = JsonConvert.DeserializeObject<List<H3Yun>>(dicParam["VoucherCollection"] + string.Empty);
                        foreach (VoucherModel item in voucher)
                        {
                            strResult = AddVoucher_glAccount("1870244219322369", JsonConvert.SerializeObject(item));
                            // 返回结果
                            Dictionary<string, object> dicJsonData = JsonConvert.DeserializeObject<Dictionary<string, object>>(strResult);
                            if (dicJsonData.ContainsKey("id"))
                            {
                                msgSchema1.Code = "0";
                                dic_schema_id.Add(list_ObjectId[i].ObjectId, dicJsonData["id"] + string.Empty);
                                i++;
                            }
                            else
                            {
                                throw new Exception($"{ dicJsonData["msg"] + string.Empty}");
                            }
                        }

                        if (dic_schema_id.Count > 0 && msgSchema1.Code == "0")
                        {
                            foreach (string ObjectId in dic_schema_id.Keys)
                            {
                                BizStructure Straaucture = new BizStructure(msgSchema);
                                Straaucture["ObjectId"] = ObjectId;
                                Straaucture["VoucherId"] = dic_schema_id[ObjectId] + string.Empty;
                                listData.Add(Straaucture);
                            }
                        }

                    }
                    //凭证作废
                    if (dicParam["methodName"] + string.Empty == "Del")
                    {
                        msgSchema1.Code = "0";
                        string strJsonData = DelVoucher(long.Parse(dicParam["acctgTransId"] + string.Empty), "1870244219322369");
                    }
                    bResult = "true";
                }

                #region 返回Schem
                msgSchema1.Add(new ItemSchema("Result", "返回值", BizDataType.String, 100, ""));
                msgSchema1.Add(new ItemSchema("Msg", "返回信息", BizDataType.String, 100, ""));
                msgSchema1.Add(new ItemSchema("ReturnData", "返回数据", BizDataType.BizStructureArray, int.MaxValue, msgSchema));
                returnBizStructure = new BizStructure(msgSchema1);

                returnBizStructure["Result"] = bResult;
                returnBizStructure["Msg"] = "服务器返回成功";
                returnBizStructure["ReturnData"] = listData.ToArray();
                InvokeResult Returnresult = new InvokeResult(0, "服务器返回成功", returnBizStructure);
                #endregion

                MessageHelper.CreateErrLog("成功参数：" + JsonConvert.SerializeObject(param));
                return new HttpResponseMessage
                {
                    Content = new StringContent(
                    BizStructureUtility.InvokeResultToJson(Returnresult),
                    System.Text.Encoding.UTF8,
                    "application/x-www-form-urlencoded")
                };
            }
            catch (Exception e)
            {
                MessageHelper.CreateErrLog(e.Message);
                MessageHelper.CreateErrLog("参数：" + JsonConvert.SerializeObject(param));
                #region 返回Schem
                msgSchema1.Code = "1";
                msgSchema1.Add(new ItemSchema("Result", "返回值", BizDataType.String, 100, ""));
                msgSchema1.Add(new ItemSchema("Msg", "返回信息", BizDataType.String, 100, ""));
                msgSchema1.Add(new ItemSchema("ReturnData", "返回数据", BizDataType.BizStructureArray, 100, ""));
                returnBizStructure = new BizStructure(msgSchema1);

                returnBizStructure["Result"] = false;
                returnBizStructure["Msg"] = e.Message;
                returnBizStructure["ReturnData"] = null;
                InvokeResult Returnresult = new InvokeResult(0, "", returnBizStructure);
                #endregion
                return new HttpResponseMessage
                {
                    Content = new StringContent(
                    BizStructureUtility.InvokeResultToJson(Returnresult),
                    System.Text.Encoding.UTF8,
                    "application/x-www-form-urlencoded")
                };
            }
        }

        /// <summary>
        /// 新增凭证（科目存在辅助核算场景）
        /// </summary>
        private string AddVoucher_glAccount(string bookid, string strJsonInfo)
        {
            StringBuilder strURL = new StringBuilder();
            strURL.AppendFormat("{0}/accounting/gl/AcctgTrans/", BASE_URL);
            strURL.AppendFormat("{0}", bookid);
            //strURL.AppendFormat("&isInsert=false");

            byte[] bytes = Encoding.UTF8.GetBytes(strJsonInfo);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strURL.ToString());
            request.Method = "POST";
            request.ContentLength = bytes.Length;
            request.ContentType = "application/json";
            request.Headers.Add("appKey", App_Key);
            request.Headers.Add("appSecret", App_Secret);
            request.Headers.Add("openToken", openToken);
            Stream reqstream = request.GetRequestStream();
            reqstream.Write(bytes, 0, bytes.Length);
            //声明一个HttpWebRequest请求
            request.Timeout = 90000;
            //设置连bai接超时时间
            request.Headers.Set("Pragma", "no-cache");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream streamReceive = response.GetResponseStream();
            Encoding encoding = Encoding.UTF8;
            StreamReader streamReader = new StreamReader(streamReceive, encoding);
            string strResult = streamReader.ReadToEnd();
            streamReceive.Dispose();
            streamReader.Dispose();

            Dictionary<string, object> returnDic = JsonConvert.DeserializeObject<Dictionary<string, object>>(strResult);
            if (returnDic.Count <= 0)
            {
                throw new Exception("获取企业凭证失败。");
            }
            return strResult;
        }

        /// <summary>
        /// 凭证删除
        /// </summary>
        /// <param name="acctgTransId">凭证id</param>
        /// <param name="bookid">创建的账套信息</param>
        /// <returns></returns>
        private string DelVoucher(long acctgTransId, string bookid)
        {
            string strResult = string.Empty;
            StringBuilder strURL = new StringBuilder();
            strURL.AppendFormat("{0}/accounting/gl/AcctgTrans/", BASE_URL);
            strURL.AppendFormat("{0}?", bookid);
            strURL.AppendFormat("&acctgTransId={0}", acctgTransId);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strURL.ToString());
            request.Method = "DELETE";
            request.ContentType = "application/json";
            request.Headers.Add("appKey", App_Key);
            request.Headers.Add("appSecret", App_Secret);
            request.Headers.Add("openToken", openToken);
            using (System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse())
            {
                using (System.IO.Stream s = response.GetResponseStream())
                {
                    string StrDate = string.Empty;
                    using (StreamReader Reader = new StreamReader(s, Encoding.UTF8))
                    {
                        while ((StrDate = Reader.ReadLine()) != null)
                        {
                            strResult += StrDate + "\r\n";
                        }
                    }
                }
            }
            // 返回结果
            if (strResult == "")
            {
                return "success";
            }
            else
            {
                throw new Exception("获取应用凭证失败。");
            }
        }

    }

}
