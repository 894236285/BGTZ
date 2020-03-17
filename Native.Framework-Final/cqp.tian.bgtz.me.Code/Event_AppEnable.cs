using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AngleSharp.Parser.Html;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using cqp.tian.bgtz.me.Code.Enum;
using Native.Sdk.Cqp;
using cqp.tian.bgtz.me.Code.Common;
using System.Net;
using Newtonsoft.Json.Linq;

namespace cqp.tian.bgtz.me.Code
{
    public class Event_AppEnable : IAppEnable
    {
        public static CQApi CQApi = null;
        System.Timers.Timer t = new System.Timers.Timer(10000); //设置时间间隔为10秒
        private static object lockObj = new object();
        public void AppEnable(object sender, CQAppEnableEventArgs e)
        {
            if (CQApi == null)
            {
                CQApi = e.CQApi;
            }

            t.Elapsed += new System.Timers.ElapsedEventHandler(getNewChapter);
            t.AutoReset = true; //每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）
            t.Enabled = true;
            t.Start();
        }

        private void getNewChapter(object sender, System.EventArgs e)
        {
            lock (lockObj) {
                //获取更新的书籍信息
                List<Book> bookList = ReadXml.GetBooksData();

                //遍历每一本书籍
                foreach (Book book in bookList)
                {
                    //起点最新章节
                    Chapter newChapter = getCatalog(book.Id);

                    //本地存储的最新章节
                    Chapter latestChapter = readLatestChapter(book.Code);

                    //如果起点的最新章节发布时间要大于本地的最新章节发布时间，说明已更新
                    if (latestChapter == null || DateTime.Parse(newChapter.ChapterTime) > DateTime.Parse(latestChapter.ChapterTime))
                    {
                        string atAll = CQApi.CQCode_AtAll().ToSendString();
                        foreach (Group g in book.Group)
                        {
                            CQApi.SendGroupMessage(long.Parse(g.GroupNo), atAll + "\n最新章节：\"" + newChapter.ChapterName + "\" \n发布时间：" + newChapter.ChapterTime + " \n本章字数：" + newChapter.WordNumber);
                        }
                        //把最新章节信息写入本地
                        writeLatestChapter(newChapter, book.Code);
                    }

                    //本次获取的最新章节名称
                    LogHelper.WriteMsgInLog("书名：" + book.Name + "，章节名称：" + newChapter.ChapterName);
                }

            }
        }

        private Chapter getCatalog(string Id)
        {
            //章节实体类
            Chapter chapter = new Chapter();

            //起点获取章节列表的请求地址
            string url = "https://book.qidian.com/ajax/book/category?bookId=" + Id;

            //发送get请求
            HttpWebResponse response = HttpHelper.CreateGetHttpResponse(url, 5000, "", null);

            //使用流获取response的数据
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string tmpResult = sr.ReadToEnd();

            //使用jobject处理数据，根据服务器返回的json结构，获取最新章节
            JObject obj = (JObject)JsonConvert.DeserializeObject(tmpResult);

            JArray vsArray = new JArray();

            if (obj["data"]["vs"].ToString() != "")
            {
                vsArray = JArray.Parse(obj["data"]["vs"].ToString());
            }

            JArray csArray = new JArray();
            csArray = JArray.Parse(vsArray[vsArray.Count - 1]["cs"].ToString());

            JObject newChapter = JObject.Parse(csArray[csArray.Count - 1].ToString());

            //设置最新章节的相关信息
            chapter.ChapterName = newChapter["cN"].ToString();
            chapter.ChapterTime = newChapter["uT"].ToString();
            chapter.WordNumber = int.Parse(newChapter["cnt"].ToString());

            //返回起点最新章节
            return chapter;
        }

        /// <summary>
        /// 读取存储的最新章节
        /// </summary>
        /// <returns></returns>
        private Chapter readLatestChapter(string bookCode)
        {
            string filePath = "F://LatestChapter//" + bookCode + ".json";

            //如果文件不存在，就新建该文件，并返回null，表示这本书是第一次获取更新  
            if (!File.Exists(filePath))
            {
                StreamWriter sr = File.CreateText(filePath);
                sr.Close();

                return null;
            }

            //读取本地的最新章节json文件
            string json = File.ReadAllText(filePath);

            //反序列化到章节实体类
            Chapter chapter = JsonConvert.DeserializeObject<Chapter>(json);

            //返回本地存储的最新章节
            return chapter;
        }

        /// <summary>
        /// 写入最新的章节信息
        /// </summary>
        /// <param name="chapter">章节信息</param>
        private void writeLatestChapter(Chapter chapter,string bookCode)
        {
            //文件地址
            string filePath = "F://LatestChapter//" + bookCode + ".json";

            //序列化为json字符串
            string json = JsonConvert.SerializeObject(chapter);

            //写入文件
            File.WriteAllText(filePath, json);
        }
    }
}
