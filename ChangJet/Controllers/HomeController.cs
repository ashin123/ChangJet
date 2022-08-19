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
    public class HomeController : ApiController
    {
        // 请求地址
        private const string BASE_URL = "https://openapi.chanjet.com";
        private const string App_Key = "P6oQEJAt";
        private const string App_Secret = "C30897064C171395270979EB2FB8DF82";
        private const string user_auth_permanent_code = "up-bc698bb078984eb3aad656b83b14c926"; // 用户永久授权码
        private const string permanentAuthCode = "op-5db38824adb94a0c854f2b9908f4815e"; // 企业永久授权码

        /// <summary>
        /// 使用用户永久授权码获取token
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        public HttpResponseMessage GetToken()
        {
            BizStructure returnBizStructure = null;
            // 定义数据结构
            BizStructureSchema msgSchema1 = new BizStructureSchema();
            msgSchema1.Add(new ItemSchema("Result", "返回值", BizDataType.String, 100, ""));
            msgSchema1.Add(new ItemSchema("Msg", "返回信息", BizDataType.String, 100, ""));
            msgSchema1.Add(new ItemSchema("ReturnData", "返回数据", BizDataType.String, 100, ""));

            try
            {
                int expires_in = 0; // 访问token的过期时间
                string access_token = string.Empty; // 访问token，请求业务接口的openToken字段

                string accesstoken_from_cache = CacheHelper.CacheValue("access_token") + string.Empty;
                if (string.IsNullOrWhiteSpace(accesstoken_from_cache))
                {
                    // 获取应用凭证
                    string appAccessToken = GetAppAccessToken();
                    // 获取企业凭证
                    string orgAccessToken = GetEnterpriseVoucher(appAccessToken);
                    // 获取token
                    access_token = GetAccessToken(orgAccessToken, ref expires_in);
                    CacheHelper.CacheInsertAddMinutes("access_token", access_token, expires_in);
                }
                else
                {
                    access_token = accesstoken_from_cache;
                }

                msgSchema1.Code = "0";
                returnBizStructure = new BizStructure(msgSchema1);
                returnBizStructure["Result"] = true.ToString();
                returnBizStructure["Msg"] = "服务器返回成功";
                returnBizStructure["ReturnData"] = access_token;
                InvokeResult Returnresult = new InvokeResult(0, "服务器返回成功", returnBizStructure);
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
                msgSchema1.Code = "1";  // 返回状态
                returnBizStructure = new BizStructure(msgSchema1);
                returnBizStructure["Result"] = true.ToString();
                returnBizStructure["Msg"] = "服务器返回成功";
                returnBizStructure["ReturnData"] = null;
                InvokeResult Returnresult = new InvokeResult(0, e.Message, returnBizStructure);
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
        /// 应用凭证
        /// </summary>
        private string GetAppAccessToken()
        {
            string appAccessToken = string.Empty;

            string appTicket = CacheHelper.CacheValue("app_ticket") + string.Empty;

            Dictionary<string, object> jsonDic = new Dictionary<string, object>();
            jsonDic.Add("appTicket", appTicket);
            string strJson = JsonConvert.SerializeObject(jsonDic);

            byte[] bytes = Encoding.UTF8.GetBytes(strJson);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BASE_URL + "/auth/appAuth/getAppAccessToken");
            request.Method = "POST";
            request.ContentLength = bytes.Length;
            //request.ContentType = "application/json";
            request.ContentType = "application/json";
            request.Headers.Add("appKey", App_Key);
            request.Headers.Add("appSecret", App_Secret);
            Stream reqstream = request.GetRequestStream();
            reqstream.Write(bytes, 0, bytes.Length);
            //声明一个HttpWebRequest请求
            request.Timeout = 30000;
            //设置连bai接超时时间
            request.Headers.Set("Pragma", "no-cache");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream streamReceive = response.GetResponseStream();
            Encoding encoding = Encoding.UTF8;
            StreamReader streamReader = new StreamReader(streamReceive, encoding);
            string strResult = streamReader.ReadToEnd();
            streamReceive.Dispose();
            streamReader.Dispose();
            // 返回结果
            Dictionary<string, object> appAccessTokenDic = JsonConvert.DeserializeObject<Dictionary<string, object>>(strResult);
            if (appAccessTokenDic.Count > 0)
            {
                if (appAccessTokenDic["code"] + string.Empty != "200")
                {
                    throw new Exception($"{appAccessTokenDic["message"] + string.Empty}；app_ticket：{ CacheHelper.CacheValue("app_ticket") + string.Empty}");
                }
                if (appAccessTokenDic.ContainsKey("result"))
                {
                    Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(appAccessTokenDic["result"] + string.Empty);
                    appAccessToken = dic.ContainsKey("appAccessToken") ? dic["appAccessToken"] + string.Empty : "";
                }
            }
            else
            {
                throw new Exception("获取应用凭证失败。");
            }
            return appAccessToken;
        }

        /// <summary>
        /// 获取企业凭证
        /// </summary>
        /// <param name="appAccessToken">应用凭证</param>
        /// <returns></returns>
        private string GetEnterpriseVoucher(string appAccessToken)
        {
            string orgAccessToken = string.Empty;

            Dictionary<string, object> jsonDic = new Dictionary<string, object>();
            jsonDic.Add("appAccessToken", appAccessToken);
            jsonDic.Add("permanentAuthCode", permanentAuthCode);
            string strJson = JsonConvert.SerializeObject(jsonDic);

            byte[] bytes = Encoding.UTF8.GetBytes(strJson);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BASE_URL + "/auth/orgAuth/getOrgAccessToken");
            request.Method = "POST";
            request.ContentLength = bytes.Length;
            request.ContentType = "application/json";
            request.Headers.Add("appKey", App_Key);
            request.Headers.Add("appSecret", App_Secret);
            Stream reqstream = request.GetRequestStream();
            reqstream.Write(bytes, 0, bytes.Length);
            //声明一个HttpWebRequest请求
            request.Timeout = 30000;
            //设置连bai接超时时间
            request.Headers.Set("Pragma", "no-cache");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream streamReceive = response.GetResponseStream();
            Encoding encoding = Encoding.UTF8;
            StreamReader streamReader = new StreamReader(streamReceive, encoding);
            string strResult = streamReader.ReadToEnd();
            streamReceive.Dispose();
            streamReader.Dispose();

            Dictionary<string, object> orgAccessTokenDic = JsonConvert.DeserializeObject<Dictionary<string, object>>(strResult);
            if (orgAccessTokenDic.Count > 0)
            {
                if (orgAccessTokenDic["code"] + string.Empty != "200")
                {
                    throw new Exception(orgAccessTokenDic["message"] + string.Empty);
                }
                if (orgAccessTokenDic.ContainsKey("result"))
                {
                    Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(orgAccessTokenDic["result"] + string.Empty);
                    orgAccessToken = dic.ContainsKey("orgAccessToken") ? dic["orgAccessToken"] + string.Empty : "";
                }
            }
            else
            {
                throw new Exception("获取企业凭证失败。");
            }

            return orgAccessToken;
        }

        /// <summary>
        /// 使用用户永久授权码获取token
        /// </summary>
        /// <param name="orgAccessToken">企业凭证</param>
        /// <returns></returns>
        private string GetAccessToken(string orgAccessToken, ref int expires_in)
        {
            string access_token = string.Empty;

            Dictionary<string, object> jsonDic = new Dictionary<string, object>();
            jsonDic.Add("orgAccessToken", orgAccessToken);
            jsonDic.Add("userAuthPermanentCode", user_auth_permanent_code);
            string strJson = JsonConvert.SerializeObject(jsonDic);

            byte[] bytes = Encoding.UTF8.GetBytes(strJson);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BASE_URL + "/auth/token/getTokenByPermanentCode");
            request.Method = "POST";
            request.ContentLength = bytes.Length;
            request.ContentType = "application/json";
            request.Headers.Add("appKey", App_Key);
            request.Headers.Add("appSecret", App_Secret);
            Stream reqstream = request.GetRequestStream();
            reqstream.Write(bytes, 0, bytes.Length);
            //声明一个HttpWebRequest请求
            request.Timeout = 30000;
            //设置连bai接超时时间
            request.Headers.Set("Pragma", "no-cache");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream streamReceive = response.GetResponseStream();
            Encoding encoding = Encoding.UTF8;
            StreamReader streamReader = new StreamReader(streamReceive, encoding);
            string strResult = streamReader.ReadToEnd();
            streamReceive.Dispose();
            streamReader.Dispose();

            Dictionary<string, object> AccessTokenDic = JsonConvert.DeserializeObject<Dictionary<string, object>>(strResult);
            if (AccessTokenDic.Count > 0)
            {
                if (AccessTokenDic["code"] + string.Empty != "200")
                {
                    throw new Exception(AccessTokenDic["message"] + string.Empty);
                }
                if (AccessTokenDic.ContainsKey("result"))
                {
                    Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(AccessTokenDic["result"] + string.Empty);
                    access_token = dic.ContainsKey("access_token") ? dic["access_token"] + string.Empty : "";   // 访问token，请求业务接口的openToken字段
                    expires_in = Convert.ToInt32(!dic.ContainsKey("expires_in") ? "1800" : dic["expires_in"] + string.Empty); // 访问token的过期时间
                }
            }
            else
            {
                throw new Exception("获取token失败。");
            }

            return access_token;
        }

    }
}
