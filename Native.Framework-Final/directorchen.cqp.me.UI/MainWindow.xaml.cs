using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Native.Sdk.Cqp;
using Native.Sdk.Cqp.Model;

namespace directorchen.cqp.me.UI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 点击发送消息按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_send_Click(object sender, RoutedEventArgs e)
        {

            TextRange textRange = new TextRange(this.txtMessage.Document.ContentStart, this.txtMessage.Document.ContentEnd);
            GroupInfo groupinfo = this.cbGroup.SelectedItem as GroupInfo;
            QQMessage msg = Menu_OpenWindow.CQApi.SendGroupMessage(groupinfo.Group, (this.atAll.IsChecked.Value ? CQApi.CQCode_AtAll().ToSendString() : "") + textRange.Text.Trim());
        }

        /// <summary>
        /// tab选项卡改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabItem item = this.tabControl.SelectedItem as TabItem;
            if (item.Header.ToString() == "消息发送")
            {
                if (this.cbGroup.ItemsSource == null)
                {
                    List<GroupInfo> group = Menu_OpenWindow.CQApi.GetGroupList();
                    this.cbGroup.SelectedValuePath = "Group";
                    this.cbGroup.DisplayMemberPath = "Name";
                    this.cbGroup.SelectedIndex = 0;
                    this.cbGroup.ItemsSource = group;
                }
            }
        }

        /// <summary>
        /// 群下拉框选项改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
        }

        /// <summary>
        /// @所有人按钮选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void atAll_Checked(object sender, RoutedEventArgs e)
        {
            if (this.atAll.IsChecked.Value)
            {
                //this.cbGroupMember.Visibility = Visibility.Collapsed;
            }

        }

        /// <summary>
        /// 群成员数据绑定
        /// </summary>
        private void groupMemberDataBinding()
        {

            Group group = (this.cbGroup.SelectedItem as GroupInfo).Group;
            List<GroupMemberInfo> groupMember = Menu_OpenWindow.CQApi.GetGroupMemberList(group);

        }

    }
}
