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
                if (e.FromGroup.Id == long.Parse(group.GroupNo))
                {
                    var picture = CQApi.CQCode_Image(group.ImageName).ToSendString();
                    var at = CQApi.CQCode_At(e.BeingOperateQQ).ToSendString();

                    //发送群消息
                    e.CQApi.SendGroupMessage(e.FromGroup, at + group.Text + (group.IsSendImage ? picture : string.Empty));

                    e.Handler = true;
                    return;
                }
            }
        }
    }
}
