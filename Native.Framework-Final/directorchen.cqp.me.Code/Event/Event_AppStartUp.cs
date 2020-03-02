using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using Native.Sdk.Cqp.Model;
using AngleSharp.Parser.Html;
using Newtonsoft.Json;
using System.IO;
using directorchen.cqp.me.Code.Enum;
using System.Net.Http;
using Native.Sdk.Cqp;

namespace directorchen.cqp.me.Code.Event
{
    public class Event_AppStartUp : ICQStartup
    {
        CQApi api = new CQApi(new AppInfo("com.director", 1, 1, "报更童子", "ss", 1, "director", "获取作品更新", 123456));

        System.Timers.Timer t = new System.Timers.Timer(10000); //设置时间间隔为10秒
        public void CQStartup(object sender, CQStartupEventArgs e)
        {
            t.Elapsed += new System.Timers.ElapsedEventHandler(getNewChapter);
            t.AutoReset = true; //每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）
            t.Enabled = true;
            t.Start();
        }

        private void getNewChapter(object sender, System.EventArgs e)
        {
            
            //url
            string url = "https://book.qidian.com/info/1015358161";

            //起点最新章节
            Chapter newChapter = getCatalog(url);

            //本地存储的最新章节
            Chapter latestChapter = readLatestChapter();

            //如果起点的最新章节发布时间要大于本地的最新章节发布时间，说明已更新
            if (DateTime.Parse(newChapter.ChapterTime) > DateTime.Parse(latestChapter.ChapterTime))
            {
                CQCode AtAll  = CQApi.CQCode_AtAll();

                //群消息提醒已更新
                //V群
                api.SendGroupMessage(615387042, AtAll + "\n最新章节：\"" + newChapter.ChapterName + "\" \n发布时间：" + newChapter.ChapterTime + " \n本章字数：" + newChapter.WordNumber);

                //普群
                api.SendGroupMessage(762873632, AtAll + "\n最新章节：\"" + newChapter.ChapterName + "\" \n发布时间：" + newChapter.ChapterTime + " \n本章字数：" + newChapter.WordNumber);

                //剧讨群
                api.SendGroupMessage(221827649, AtAll + "\n最新章节：\"" + newChapter.ChapterName + "\" \n发布时间：" + newChapter.ChapterTime + " \n本章字数：" + newChapter.WordNumber);

                //弟子群
                api.SendGroupMessage(526275426, AtAll + "\n最新章节：\"" + newChapter.ChapterName + "\" \n发布时间：" + newChapter.ChapterTime + " \n本章字数：" + newChapter.WordNumber);

                //把最新章节信息写入本地
                writeLatestChapter(newChapter);
            }

            //WriteMsgInLog("成功执行");
        }


        private Chapter getCatalog(string url)
        {
            //章节实体类
            Chapter chapter = new Chapter();

            using (HttpClient http = new HttpClient())
            {
                //获取指定网址的html
                var htmlString = http.GetStringAsync(url).Result;

                //开始格式化
                HtmlParser htmlParser = new HtmlParser();
                var data = htmlParser.Parse(htmlString)
                    .QuerySelectorAll("#j-catalogWrap .volume-wrap .cf li a")  //获取章节名称
                    .Select(t => t)
                    .ToList();

                //最新章节
                var latest = data[data.Count - 1];

                //章节名称
                chapter.ChapterName = latest.TextContent;

                //title获取，包含发布时间和字数
                string title = latest.Attributes["title"].Value;

                //title拆分，获取发布时间
                chapter.ChapterTime = title.Substring(5, 19);

                //title拆分，获取字数
                chapter.WordNumber = int.Parse(title.Substring(30));

            }

            //返回起点最新章节
            return chapter;
        }

        /// <summary>
        /// 读取存储的最新章节
        /// </summary>
        /// <returns></returns>
        private Chapter readLatestChapter()
        {

            //读取本地的最新章节json文件
            string json = File.ReadAllText("F://LatestChapter.json");

            //反序列化到章节实体类
            Chapter chapter = JsonConvert.DeserializeObject<Chapter>(json);

            //返回本地存储的最新章节
            return chapter;
        }

        /// <summary>
        /// 写入最新的章节信息
        /// </summary>
        /// <param name="chapter">章节信息</param>
        private void writeLatestChapter(Chapter chapter)
        {
            //序列化为json字符串
            string json = JsonConvert.SerializeObject(chapter);

            //写入文件
            File.WriteAllText("F://LatestChapter.json", json);
        }

        /// <summary>
        /// 日志记录
        /// </summary>
        /// <param name="msg">提示信息</param>
        private void WriteMsgInLog(string msg)
        {
            string filePath = "F:\\XHDZ_Log.txt";

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
