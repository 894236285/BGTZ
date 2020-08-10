using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cqp.tian.bgtz.me.Code.Enum
{
    public class Book
    {
        /// <summary>
        /// 书籍名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 书籍ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 书籍Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 是否@全体成员
        /// </summary>
        public bool IsAtAll { get; set; }

        /// <summary>
        /// 是否发送图片
        /// </summary>
        public bool IsSendImage { get; set; }

        /// <summary>
        /// 图片名称
        /// </summary>
        public string ImageName { get; set; }

        /// <summary>
        /// 群
        /// </summary>
        public List<Group> Group { get; set; }
    }
}
