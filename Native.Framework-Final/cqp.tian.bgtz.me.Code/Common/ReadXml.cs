using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using cqp.tian.bgtz.me.Code.Enum;

namespace cqp.tian.bgtz.me.Code.Common
{
    public class ReadXml
    {
        /// <summary>
        /// 获取更新的书籍信息
        /// </summary>
        /// <returns></returns>
        public static List<Book> GetBooksData()
        {
            List<Book> bookList = new List<Book>();
            XmlDocument doc = new XmlDocument();
            doc.Load(CommonApi.MainPath + "BookGroup\\Book.xml");
            XmlNodeList nodeList = doc.DocumentElement.FirstChild.ChildNodes;

            foreach (XmlElement node in nodeList)
            {
                Book book = new Book();
                book.Name = node.GetAttribute("name");
                book.Id = node.GetAttribute("id");
                book.Code = node.GetAttribute("code");
                book.IsAtAll = node.GetAttribute("isAtAll") == "Y" ? true : false;
                book.IsSendImage = node.GetAttribute("isSendImage") == "Y" ? true : false;
                book.ImageName = node.GetAttribute("imageName");
                book.Group = new List<Group>();

                foreach (XmlElement childNode in node.ChildNodes)
                {
                    book.Group.Add(new Group() { GroupNo = childNode.InnerText });
                }

                bookList.Add(book);
            }

            return bookList;
        }

        /// <summary>
        /// 获取群信息
        /// </summary>
        /// <returns></returns>
        public static List<Group> GetGroupsData()
        {
            List<Group> groupList = new List<Group>();

            XmlDocument doc = new XmlDocument();
            doc.Load(CommonApi.MainPath + "BookGroup\\Group.xml");
            XmlNodeList nodeList = doc.DocumentElement.FirstChild.ChildNodes;

            foreach (XmlElement node in nodeList)
            {
                Group group = new Group();
                group.GroupNo = node.InnerText;
                group.Text = node.GetAttribute("text");
                group.IsSendImage = node.GetAttribute("isSendImage") == "Y" ? true : false;
                group.ImageName = node.GetAttribute("imageName");
                groupList.Add(group);
            }

            return groupList;
        }
    }
}
