using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace ChangJet.Controllers
{
    public class OAuthController : ApiController
    {
        private const string encryption_key = "5fa057f2734a418a";   // 密钥
        private const string app_ticket = "APP_TICKET"; // 消息类型为APP_TICKET

        /// <summary>
        /// 回调（OAuth回调地址）
        /// </summary>
        [HttpGet]
        public string GetCode(string code, string state)
        {
            try
            {
                MessageHelper.CreateErrLog($"授权Code：{code}");
                return code;
            }
            catch (Exception e)
            {
                MessageHelper.CreateErrLog(e.Message);
                return e.Message;
            }
        }

        /// <summary>
        /// 回调（消息接收地址）
        /// 每隔10分钟向三方应用推送一次，appTicket的过期时间为30分钟
        /// </summary>
        [HttpPost]
        public object GetAppTicket([FromBody] Dictionary<string, string> message)
        {
            try
            {
                Dictionary<string, string> result = new Dictionary<string, string>();

                if (message.ContainsKey("encryptMsg"))
                {
                    string encryptMsg = message["encryptMsg"] + string.Empty;
                    string sendData = AESDecrypt(encryptMsg, encryption_key);
                    DecryptDataModel dicSendData = JsonConvert.DeserializeObject<DecryptDataModel>(sendData);
                    //// 企业临时授权码消息
                    //if (dicSendData.msgType == "TEMP_AUTH_CODE")
                    //{
                    //    MessageHelper.CreateErrLog(string.Format("企业临时授权码消息:{0}\r\n密文：{1}", sendData, encryptMsg));
                    //}
                    // appTicket消息
                    if (dicSendData.msgType == app_ticket)
                    {
                        // Cache
                        string ticket = CacheHelper.CacheValue("app_ticket") + string.Empty; // appTicket 
                        string refreshTime = CacheHelper.CacheValue("refreshTime") + string.Empty; // 过期时间
                        // 首次进入时，不存在过期时间
                        if (string.IsNullOrWhiteSpace(refreshTime))
                        {
                            CacheHelper.CacheInsertAddMinutes("app_ticket", dicSendData.bizContent.appTicket, 1800);
                            CacheHelper.CacheInsertAddMinutes("refreshTime", DateTime.Now.AddSeconds(1800) + string.Empty, 1800);
                            result.Add("result", "success");
                            return result;
                        }
                        DateTime pauseT = Convert.ToDateTime(refreshTime); // 过期时间
                        DateTime resumeT = DateTime.Now;    // 当前时间
                        TimeSpan ts1 = new TimeSpan(pauseT.Ticks);
                        TimeSpan ts2 = new TimeSpan(resumeT.Ticks);
                        TimeSpan tsSub = ts1.Subtract(ts2).Duration();
                        if (string.IsNullOrWhiteSpace(ticket) || tsSub.Minutes < 10)
                        {
                            // 清除缓存
                            CacheHelper.CacheNull("app_ticket");
                            CacheHelper.CacheNull("refreshTime");
                            // 重新添加缓存
                            CacheHelper.CacheInsertAddMinutes("app_ticket", dicSendData.bizContent.appTicket, 1800);
                            CacheHelper.CacheInsertAddMinutes("refreshTime", resumeT.AddSeconds(1800) + string.Empty, 1800);
                        }
                        result.Add("result", "success");
                    }
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// AES解密(无向量)
        /// </summary>
        /// <param name="encryptedBytes">被加密的明文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public string AESDecrypt(string Data, string Key)
        {
            byte[] encryptedBytes = Convert.FromBase64String(Data);
            byte[] bKey = new byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(Key.PadRight(bKey.Length)), bKey, bKey.Length);

            MemoryStream mStream = new MemoryStream(encryptedBytes);
            //mStream.Write( encryptedBytes, 0, encryptedBytes.Length );
            //mStream.Seek( 0, SeekOrigin.Begin );
            RijndaelManaged aes = new RijndaelManaged();
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.KeySize = 128;
            aes.Key = bKey;
            //aes.IV = _iV;
            CryptoStream cryptoStream = new CryptoStream(mStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
            try
            {
                byte[] tmp = new byte[encryptedBytes.Length + 32];
                int len = cryptoStream.Read(tmp, 0, encryptedBytes.Length + 32);
                byte[] ret = new byte[len];
                Array.Copy(tmp, 0, ret, 0, len);
                return Encoding.UTF8.GetString(ret);
            }
            finally
            {
                cryptoStream.Close();
                mStream.Close();
                aes.Clear();
            }
        }

        /// <summary>
        /// appTicket消息(每隔10分钟发送的appTicket消息)
        /// </summary>
        public class DecryptDataModel
        {
            /// <summary>
            /// 消息Id
            /// </summary>
            public string id { get; set; }
            /// <summary>
            /// 开放平台的应用appKey
            /// </summary>
            public string appKey { get; set; }
            /// <summary>
            /// 消息类型
            /// </summary>
            public string msgType { get; set; }
            /// <summary>
            /// 时间戳
            /// </summary>
            public string time { get; set; }
            /// <summary>
            /// 具体推送的消息内容
            /// </summary>
            public AppTicketModel bizContent { get; set; }
        }
        public class AppTicketModel : DecryptDataModel
        {
            public string appTicket { get; set; }
        }
    }
}
