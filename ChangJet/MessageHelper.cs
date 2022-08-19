using System;
using System.IO;
using System.Text;
using System.Web;

namespace ChangJet
{
    public class MessageHelper
    {
        /// <summary>
        /// 生成错误日志
        /// </summary>
        /// <param name="exc"></param>
        public static void CreateErrLog(string exc)
        {
            try
            {
                string url = HttpContext.Current.Server.MapPath("~") + "logs\\";
                exc = $"【{DateTime.Now}】ErrorMessage:\r\n{exc}\r\n";
                byte[] bytes = Encoding.UTF8.GetBytes(exc);
                if (!Directory.Exists(url))
                {
                    Directory.CreateDirectory(url);
                }
                string fileName = url + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                using (FileStream fs = new FileStream(fileName, FileMode.Append))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}