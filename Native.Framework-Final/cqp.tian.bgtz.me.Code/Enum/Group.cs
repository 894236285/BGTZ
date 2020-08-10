using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cqp.tian.bgtz.me.Code.Enum
{
    public class Group
    {
        /// <summary>
        /// 群号
        /// </summary>
        public string GroupNo { get; set; }

        /// <summary>
        /// 提示语
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 是否发送图片
        /// </summary>
        public bool IsSendImage { get; set; }

        /// <summary>
        /// 图片名称
        /// </summary>
        public string ImageName { get; set; }
    }
}
