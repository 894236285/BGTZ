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
using System.Data;
using System.Reflection;
using directorchen.cqp.me.UI.Model;
using SQLiteHelper;

namespace directorchen.cqp.me.UI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        SQLiteHelp dbHelp;
        public MainWindow()
        {
            dbHelp = new SQLiteHelp();
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
            else if (item.Header.ToString() == "书籍管理")
            {
                if (this.dgBookList.ItemsSource == null)
                {
                    string sql = "select * from Book";
                    DataSet ds = dbHelp.GetDataSet(sql);
                    List<Book> bookList = DataSetToList<Book>(ds, 0);
                    this.dgBookList.ItemsSource = bookList;
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


        /// <summary>
        /// DataSetToList
        /// </summary>
        /// <typeparam name="T">转换类型</typeparam>
        /// <param name="dataSet">数据源</param>
        /// <param name="tableIndex">需要转换表的索引</param>
        /// <returns></returns>
        public List<T> DataSetToList<T>(DataSet dataSet, int tableIndex)
        {
            //确认参数有效
            if (dataSet == null || dataSet.Tables.Count <= 0 || tableIndex < 0)
                return null;

            DataTable dt = dataSet.Tables[tableIndex];

            List<T> list = new List<T>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //创建泛型对象
                T _t = Activator.CreateInstance<T>();
                //获取对象所有属性
                PropertyInfo[] propertyInfo = _t.GetType().GetProperties();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    foreach (PropertyInfo info in propertyInfo)
                    {
                        //属性名称和列名相同时赋值
                        if (dt.Columns[j].ColumnName.ToUpper().Equals(info.Name.ToUpper()))
                        {
                            if (dt.Rows[i][j] != DBNull.Value)
                            {
                                info.SetValue(_t, dt.Rows[i][j], null);
                            }
                            else
                            {
                                info.SetValue(_t, null, null);
                            }
                            break;
                        }
                    }
                }
                list.Add(_t);
            }
            return list;
        }

    }
}
