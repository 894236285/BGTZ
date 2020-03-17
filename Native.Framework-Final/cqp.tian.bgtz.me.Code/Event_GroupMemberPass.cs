using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using cqp.tian.bgtz.me.Code.Enum;
using Native.Sdk.Cqp;
using cqp.tian.bgtz.me.Code.Common;

namespace cqp.tian.bgtz.me.Code
{
    public class Event_GroupMemberPass : IGroupMemberIncrease
    {
        public void GroupMemberIncrease(object sender, CQGroupMemberIncreaseEventArgs e)
        {
            List<Group> grouList = ReadXml.GetGroupsData();

            foreach (Group group in grouList)
            {
                if (e.CQApi.IsAllowSendImage && e.FromGroup.Id == long.Parse(group.GroupNo))
                {
                    var picture = CQApi.CQCode_Image("fensizhi.png");
                    var at = CQApi.CQCode_At(e.BeingOperateQQ).ToSendString();

                    //发送群消息
                    e.CQApi.SendGroupMessage(e.FromGroup, string.Format(at + "  欢迎新道友加入,本群是玄浑道章V群,需要验证粉丝值,请发送带有ID的粉丝值截图在群里,谢谢！ 如何查看带有ID的粉丝值截图如下:{0}", picture));// 

                    e.Handler = true;
                    return;
                }
            }
        }
    }
}
