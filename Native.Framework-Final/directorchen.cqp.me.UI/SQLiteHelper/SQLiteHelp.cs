using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Data.SQLite;
using System.Reflection;
using System.Collections;
using Newtonsoft.Json;
using directorchen.cqp.me.UI.Model;

namespace directorchen.cqp.me.UI.SQLiteHelper
{
    public class SQLiteHelp
    {
        Dictionary<string, object> parameters;

        public SQLiteHelp() {}

        #region 清空参数
        /// <summary>
        /// 清空参数
        /// </summary>
        public void CleanParam()
        {
            if (parameters != null)
                parameters.Clear();
            parameters = null;
        }
        #endregion

        #region 无参数获取DataSet
        /// <summary>
        /// 执行Sql返回DataSet,如果无事务，则关闭连接
        /// </summary>
        /// <param name="sql">sql</param>             
        /// <returns></returns>
        public DataSet GetDataSet(string sql, bool b = true)
        {
            try
            {
                DataSet ds = null;
                SQLiteConnection con = SQLiteConn.GetConnection(b);
                ds = SQLiteAccess.GetDataSet(con, sql, parameters, true);

                WriteMsgInLog("无参数获取DataSet成功,sql是：" + sql + "，返回值是：" + JsonConvert.SerializeObject(ds));
                return ds;
            }
            catch (Exception e)
            {
                WriteMsgInLog("无参数获取DataSet失败,sql是：" + sql + "，错误内容是：" + e.Message);
                return null;
            }
        }
        #endregion

        #region 通过对象参数获取DataSet
        //<summary>
        ///执行Sql返回DataSet,如果无事务，则关闭连接
        ///</summary>
        ///<param name="sql">sql</param>     
        ///<param name="param">参数，实体对象</param> 
        /// <returns></returns>
        public DataSet GetDataSet(string sql, object param, bool b = true)
        {
            try
            {
                DataSet ds = null;
                SQLiteConnection con = SQLiteConn.GetConnection(b);
                AddFields(param);
                ds = SQLiteAccess.GetDataSet(con, sql, parameters, true);

                WriteMsgInLog("通过对象参数获取DataSet成功，sql是：" + sql + "，参数是：" + JsonConvert.SerializeObject(parameters) + "，返回值是：" + JsonConvert.SerializeObject(ds));

                return ds;
            }
            catch (Exception e)
            {
                WriteMsgInLog("通过对象参数获取DataSet失败,sql是：" + sql + "，参数是：" + JsonConvert.SerializeObject(parameters) + ",错误内容是：" + e.Message);
                return null;
            }
        }
        #endregion

        #region 无参数获取DataTable
        /// <summary>
        /// 执行Sql返回DataTable,如果无事务，则关闭连接
        /// </summary>
        /// <param name="sql">sql</param>       
        /// <returns></returns>
        public DataTable GetDataTable(string sql, bool b = true)
        {
            DataSet ds = GetDataSet(sql, b);
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }
        #endregion

        #region 通过对象参数获取DataTable
        /// <summary>
        /// 执行Sql返回DataTable,如果无事务，则关闭连接
        /// </summary>
        /// <param name="sql">sql</param>       
        /// <returns></returns>
        public DataTable GetDataTable(string sql, object param, bool b = true)
        {
            DataSet ds = GetDataSet(sql, param, b);
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }
        #endregion

        #region 执行Sql返回更新时受影响的行数
        /// <summary>
        /// 执行Sql返回更新时受影响的行数
        /// </summary>
        /// <param name="sql">sql</param>
        /// <returns></returns>
        public int ExecuteResult(string sql, bool b = true)
        {
            try
            {
                int RetInt = -1;
                SQLiteConnection con = SQLiteConn.GetConnection(b);
                RetInt = SQLiteAccess.ExecuteResult(con, sql, parameters, true);

                WriteMsgInLog("执行Sql返回更新时受影响的行数成功，sql是：" + sql + "，返回值是：" + RetInt);

                return RetInt;
            }
            catch (Exception e)
            {
                WriteMsgInLog("执行Sql返回更新时受影响的行数失败，sql是：" + sql + "，错误内容是：" + e.Message);
                return -1;
            }
        }
        #endregion

        #region 执行Sql返回更新时受影响的行数(有参数)
        /// <summary>
        /// 执行Sql返回更新时受影响的行数
        /// </summary>
        /// <param name="sql">sql</param>
        /// <returns></returns>
        public int ExecuteResult(string sql, object param, bool b = true)
        {
            try
            {
                int RetInt = -1;

                SQLiteConnection con = SQLiteConn.GetConnection(b);
                AddFields(param);
                RetInt = SQLiteAccess.ExecuteResult(con, sql, parameters, true);

                WriteMsgInLog("执行Sql返回更新时受影响的行数(有参数)成功，sql是：" + sql + "，参数是：" + JsonConvert.SerializeObject(parameters) + "，返回值是：" + RetInt);

                return RetInt;
            }
            catch (Exception e)
            {
                WriteMsgInLog("执行Sql返回更新时受影响的行数(有参数)失败，sql是：" + sql + "，参数是：" + JsonConvert.SerializeObject(parameters) + "，错误内容是：" + e.Message);
                return -1;
            }
        }
        #endregion

        #region 返回单个结果
        /// <summary>
        /// 执行Sql返回单个值
        /// </summary>
        /// <param name="sql">sql</param> 
        /// <param name="param">参数</param>       
        /// <returns></returns>
        public object GetScale(string sql, bool b = true)
        {
            try
            {
                object RetScale = -1;
                SQLiteConnection con = SQLiteConn.GetConnection(b);
                RetScale = SQLiteAccess.GetScalar(con, sql, parameters, true);

                WriteMsgInLog("返回单个结果成功，sql是：" + sql + "，返回值是：" + JsonConvert.SerializeObject(RetScale));
                return RetScale;
            }
            catch (Exception e)
            {
                WriteMsgInLog("执行Sql返回更新时受影响的行数失败，sql是：" + sql + "，错误内容是：" + e.Message);
                return -1;
            }
        }
        #endregion

        #region 返回单个结果(有参数)
        /// <summary>
        /// 执行Sql返回单个值
        /// </summary>
        /// <param name="sql">sql</param> 
        /// <param name="param">参数</param>       
        /// <returns></returns>
        public object GetScale(string sql, object param, bool b = true)
        {
            try
            {
                object RetScale = -1;

                SQLiteConnection con = SQLiteConn.GetConnection(b);
                AddFields(param);
                RetScale = SQLiteAccess.GetScalar(con, sql, parameters, true);
                WriteMsgInLog("返回单个结果(有参数)成功，sql是：" + sql + "，返回值是：" + JsonConvert.SerializeObject(RetScale));
                return RetScale;
            }
            catch (Exception e)
            {
                WriteMsgInLog("执行Sql返回更新时受影响的行数(有参数)失败，sql是：" + sql + "，参数是：" + JsonConvert.SerializeObject(parameters) + "，错误内容是：" + e.Message);
                return -1;
            }
        }
        #endregion

        #region 创建表

        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="table"></param>
        /// <param name="b"></param>
        public void CreateTable(SQLiteTable table, bool b = true)
        {
            try
            {
                SQLiteConnection con = SQLiteConn.GetConnection(b);
                SQLiteAccess.CreateTable(con, table, true);
                WriteMsgInLog("创建表成功，参数是" + JsonConvert.SerializeObject(table));
            }
            catch (Exception e)
            {
                WriteMsgInLog("创建表成功，参数是" + JsonConvert.SerializeObject(table) + "，错误内容是：" + e.Message);
            }
        }

        #endregion

        #region 添加查询参数(object)
        /// <summary>
        /// 添加查询参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public SQLiteHelp AddFields(object obj)
        {
            if (parameters == null)
                parameters = new Dictionary<string, object>();
            else
                parameters.Clear();

            Type t = obj.GetType();

            PropertyInfo[] p = t.GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);

            foreach (PropertyInfo key in p)
            {
                if (key == null)
                    continue;
                var mappe = Attribute.GetCustomAttribute(key, typeof(MappedAttribute)) as MappedAttribute;
                if (mappe != null && !mappe.IsMapped)
                {
                    continue;
                }
                string name = key.Name;
                object value = key.GetValue(obj, null);
                if (value == null || (name == "Rows" && value.ToString() == "0") || (name == "Page" && value.ToString() == "0"))
                    continue;
                if (key.PropertyType == typeof(bool))
                    continue;
                if (parameters.ContainsKey(name))
                    parameters[name] = value;
                else
                    parameters.Add(name, value);
            }
            return this;
        }
        #endregion

        #region 添加查询参数(Hanstable)
        /// <summary>
        /// 添加查询参数
        /// </summary>
        /// <param name="table">参数集合</param>
        /// <returns></returns>
        public SQLiteHelp AddFields(Hashtable table)
        {
            if (parameters == null)
                parameters = new Dictionary<string, object>();
            else
                parameters.Clear();

            foreach (object key in table.Keys)
            {
                string name = key.ToString();
                object value = table[key];
                if (parameters.ContainsKey(name))
                    parameters[name] = value;
                else
                    parameters.Add(name, value);
            }
            return this;
        }
        #endregion

        #region 添加查询参数(key,value)
        /// <summary>
        /// 添加查询参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SQLiteHelp AddFields(string name, object value)
        {

            if (parameters == null)
                parameters = new Dictionary<string, object>();
            if (parameters.ContainsKey(name))
                parameters[name] = value;
            else
                parameters.Add(name, value);
            return this;

        }
        #endregion

        #region 日志
        /// <summary>
        /// 日志记录
        /// </summary>
        /// <param name="msg">提示信息</param>
        private void WriteMsgInLog(string msg)
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
