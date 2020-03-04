using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace directorchen.cqp.me.UI.SQLiteHelper
{
    public class SQLiteConn
    {
        static Func<SQLiteConnection>[] connections;
        static Func<SQLiteConnection> MainConnect;
        static int[] connectionsQz;
        static int totalQz = 0;
        static Random random = new Random();
        static Type SQLiteConnectiontype = typeof(SQLiteConnection);

        static string MainConnectStr;

        /// <summary>
        /// 根据连接字符串配置创建数据库连接
        /// </summary>
        /// <param name="css"></param>
        /// <returns></returns>
        static Func<SQLiteConnection> CreateConnection(ConnectionStringSettings css)
        {
            
            string providerName = css.ProviderName;
            Func<SQLiteConnection> con = null;
            if (string.IsNullOrEmpty(providerName) || providerName == "System.Data.SQLite")
            {
                con = () => { return new SQLiteConnection(css.ConnectionString); };
            }
            else
            {


                Assembly ass = Assembly.LoadFrom(providerName);

                Type[] types = ass.GetTypes();

                foreach (Type t in types)
                {
                    if (t.IsPublic && !t.IsNested && t.IsClass && !t.IsAbstract && t.IsSubclassOf(SQLiteConnectiontype))
                    {
                        ConstructorInfo ci = t.GetConstructor(Type.EmptyTypes);
                        if (ci == null)
                        {
                            con = () =>
                            {
                                SQLiteConnection connection = ci.Invoke(null) as SQLiteConnection;
                                connection.ConnectionString = css.ConnectionString;
                                return connection;
                            };
                            break;
                        }
                    }
                }

            }

            return con;
        }

        class QZConnection : IComparable
        {
            public int QZ { get; set; }
            public Func<SQLiteConnection> FnCreateCon { get; set; }



            public QZConnection(int qz, Func<SQLiteConnection> fn)
            {
                QZ = qz;
                FnCreateCon = fn;
            }

            public int CompareTo(object obj)
            {
                QZConnection other = obj as QZConnection;
                if (other == this || this.GetHashCode() == other.GetHashCode())
                    return 0;
                return this.QZ - other.QZ;
            }
        }

        /// <summary>
        /// 静初始化化，建立数据库连接
        /// </summary>
        static SQLiteConn()
        {
            WriteMsgInLog("静态构造函数执行成功");
            NameValueCollection settings = ConfigurationManager.AppSettings;

            ConnectionStringSettingsCollection cons = ConfigurationManager.ConnectionStrings;
            ConnectionStringSettings css = null;
            int qz = 0;
            // List<int> listQz = new List<int>();
            // List<Func<SQLiteConnection>> listconnection = new List<Func<SQLiteConnection>>();
            ArrayList qzcon = new ArrayList();
            foreach (string key in settings.Keys)
            {
                WriteMsgInLog("key是：" + key);
                css = cons[key];
                if (css != null)
                {
                    Func<SQLiteConnection> dbcon = CreateConnection(css);
                    if (dbcon != null)
                    {
                        int.TryParse(settings[key], out qz);
                        if (qz == 0 && MainConnect == null)
                        {
                            MainConnect = dbcon;
                            MainConnectStr = css.ConnectionString;
                            continue;
                        }
                        WriteMsgInLog("链接字符串是："+css.ConnectionString);
                        qz = qz <= 0 ? 1 : qz;
                        totalQz += qz;
                        qzcon.Add(new QZConnection(qz, dbcon));

                        //listconnection.Add(dbcon);
                        //listQz.Add(qz);
                    }
                }
            }
            qzcon.Sort();

            connections = new Func<SQLiteConnection>[qzcon.Count]; //qzcon.Values.ToArray(); listconnection.ToArray();
            connectionsQz = new int[qzcon.Count];// qzcon.Keys.ToArray(); Array.Sort<int>(connectionsQz);
            for (int i = 0, j = qzcon.Count; i < j; i++)
            {
                QZConnection tmp = qzcon[i] as QZConnection;
                connections[i] = tmp.FnCreateCon;
                connectionsQz[i] = tmp.QZ;
            }
        }

        /// <summary>
        /// 根据权重获取连接
        /// </summary>
        /// <param name="useMainconnection">是否使用主要的数据连接</param>
        /// <returns></returns>
        public static SQLiteConnection GetConnection(bool useMainconnection)
        {

            if (useMainconnection)
            {
                return MainConnect == null ? null : MainConnect();
            }

            if (connectionsQz.Length <= 1)
                return connections.Length == 0 ? MainConnect() : connections[0]();
            int index = random.Next(totalQz) + 1;

            return connections[Compare(index)]();
        }
        static int Compare(int index)
        {
            int temp = 0;
            int arry = 0;
            foreach (int item in connectionsQz)
            {
                temp = temp + item;
                if (temp < index)
                {
                    arry++;
                }
                else
                {
                    break;
                }
            }

            return arry;
        }

        /// <summary>
        /// 打开连接
        /// </summary>
        /// <param name="con"></param>
        public static void OpenCon(SQLiteConnection con)
        {
            if (con != null)
            {
                if (con.State == ConnectionState.Broken)
                    con.Close();
                if (con.State == ConnectionState.Closed)
                    con.Open();
            }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <param name="con"></param>
        public static void CloseCon(SQLiteConnection con)
        {
            if (con != null)
            {
                RollbackTrans(con);
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }

        }

        /// <summary>
        /// 数据连接所开启的事务
        /// </summary>
        static Hashtable DbConTrans = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        public static DbTransaction BeginTrans(SQLiteConnection con)
        {
            DbTransaction trans = null;
            if (con != null)
            {
                if (DbConTrans.ContainsKey(con))
                    throw new Exception("该连接已经开启事务，不能进行并行事务");
                trans = con.BeginTransaction();
                DbConTrans.Add(con, trans);
            }
            return trans;
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        /// <param name="con"></param>
        public static void CommitTrans(SQLiteConnection con)
        {
            DbTransaction trans = null;
            if (con != null && DbConTrans.ContainsKey(con))
            {
                trans = DbConTrans[con] as DbTransaction;
                trans.Commit();
                DbConTrans.Remove(con);
            }
        }


        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <param name="con"></param>
        public static void RollbackTrans(SQLiteConnection con)
        {
            DbTransaction trans = null;
            if (con != null && DbConTrans.ContainsKey(con))
            {
                trans = DbConTrans[con] as DbTransaction;
                trans.Rollback();
                DbConTrans.Remove(con);
            }
        }

        #region 日志
        /// <summary>
        /// 日志记录
        /// </summary>
        /// <param name="msg">提示信息</param>
        private static void WriteMsgInLog(string msg)
        {
            string filePath = "F:\\Log\\DBLog\\" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + "Log.txt";


            //如果文件夹不存在，创建一个新的文件夹
            if (!Directory.Exists("F:\\Log"))
            {
                Directory.CreateDirectory("F:\\Log");
            }

            if (!Directory.Exists("F:\\Log\\DBLog"))
            {
                Directory.CreateDirectory("F:\\Log\\DBLog");
            }

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
        #endregion

    }
}
