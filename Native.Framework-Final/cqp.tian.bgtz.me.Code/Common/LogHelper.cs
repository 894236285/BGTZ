using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace cqp.tian.bgtz.me.Code.Common
{
    public class LogHelper
    {
        /// <summary>
        /// 日志记录
        /// </summary>
        /// <param name="msg">提示信息</param>
        /// <param name="code">文件名</param>
        public static void WriteMsgInLog(string msg,string code)
        {
            string filePath = CommonApi.LogPath + code + "_Log.txt";

            if (!Directory.Exists(CommonApi.LogPath))
            {
                Directory.CreateDirectory(CommonApi.LogPath);
            }

            //标记是否是新建文件的标量   
            //如果文件不存在，就新建该文件  
            if (!File.Exists(filePath))
            {
                StreamWriter sr = File.CreateText(filePath);
                sr.Close();
            }

            //向文件写入内容
            StreamWriter sw = new StreamWriter(filePath, true, Encoding.UTF8);

            sw.WriteLine("==================" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "====================");
            sw.WriteLine(msg);
            sw.WriteLine("=========================================================");
            sw.WriteLine("");
            sw.WriteLine("");
            sw.Flush();
            sw.Close();

        }
    }
}
