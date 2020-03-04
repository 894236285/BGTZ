using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using directorchen.cqp.me.UI.Model;

namespace directorchen.cqp.me.UI.SQLiteHelper
{
    public class SQLiteAccess
    {

        #region 获取DataSet
        /// <summary>
        /// 获取DataSet
        /// </summary>
        /// <param name="sql">sql</param>       
        /// <returns></returns>
        public static DataSet GetDataSet(SQLiteConnection con, string sql, Dictionary<string, object> param, bool closeCon)
        {
            DataSet ds = null;
            SQLiteDataReader reader = null;
            if (con != null)
            {
                try
                {
                    SQLiteCommand cmd = con.CreateCommand();
                    cmd.CommandTimeout = 180;
                    cmd.CommandText = sql.Trim();
                    if (cmd.CommandText.IndexOf(' ') == -1)
                        cmd.CommandType = CommandType.StoredProcedure;
                    if (param != null && param.Count > 0)
                        SetCmdParameter(cmd, param);
                    SQLiteConn.OpenCon(con);
                    ds = new DataSet();
                    reader = cmd.ExecuteReader();
                    GetDataFromDataReader(reader, ds);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                    if (closeCon)
                        SQLiteConn.CloseCon(con);
                }

            }

            return ds;
        }
        #endregion

        #region 获取DataTable
        /// <summary>
        /// 执行Sql返回DataTable
        /// </summary>
        /// <param name="sql">sql</param>       
        /// <returns></returns>
        public static DataTable GetDataTable(SQLiteConnection con, string sql, Dictionary<string, object> param, bool closeCon)
        {
            DataSet ds = GetDataSet(con, sql, param, closeCon);
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];

            return null;
        }
        #endregion

        #region 从DataReader中读取数据到DataSet

        /// <summary>
        /// 从DataReader中读取数据到DataSet
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="ds"></param>
        private static void GetDataFromDataReader(SQLiteDataReader reader, DataSet ds)
        {
            DataTable dt = new DataTable();
            int j = reader.FieldCount;
            for (int i = 0; i < j; i++)
            {
                dt.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
            }

            while (reader.Read())
            {
                object[] values = new object[j];
                reader.GetValues(values);
                dt.Rows.Add(values);
            }
            ds.Tables.Add(dt);
            if (reader.NextResult())
                GetDataFromDataReader(reader, ds);

        }


        #endregion

        #region 执行Sql返回更新时受影响的行数
        /// <summary>
        /// 执行Sql返回更新时受影响的行数
        /// </summary>
        /// <param name="sql">sql</param>
        /// <returns></returns>
        public static int ExecuteResult(SQLiteConnection con, string sql, Dictionary<string, object> param, bool closeCon)
        {
            int RetInt = -1;
            if (con != null)
            {
                try
                {
                    SQLiteCommand cmd = con.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.CommandText = sql.Trim();
                    if (cmd.CommandText.IndexOf(' ') == -1)
                        cmd.CommandType = CommandType.StoredProcedure;
                    if (param != null && param.Count > 0)
                        SetCmdParameter(cmd, param);
                    SQLiteConn.OpenCon(con);
                    RetInt = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (closeCon)
                        SQLiteConn.CloseCon(con);
                }
            }
            return RetInt;
        }

        #endregion

        #region 执行Sql返回单个值
        /// <summary>
        /// 执行Sql返回单个值
        /// </summary>
        /// <param name="sql">sql</param> 
        /// <param name="param">参数</param>       
        /// <returns></returns>
        public static object GetScalar(SQLiteConnection con, string sql, Dictionary<string, object> param, bool closeCon)
        {
            object RetScale = null;
            if (con != null)
            {
                try
                {
                    SQLiteCommand cmd = con.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.CommandText = sql.Trim();
                    if (cmd.CommandText.IndexOf(' ') == -1)
                        cmd.CommandType = CommandType.StoredProcedure;
                    if (param != null && param.Count > 0)
                        SetCmdParameter(cmd, param);
                    SQLiteConn.OpenCon(con);
                    RetScale = cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (closeCon)
                        SQLiteConn.CloseCon(con);
                }
            }
            return RetScale;
        }

        #endregion

        #region 创建表
        public static void CreateTable(SQLiteConnection con, SQLiteTable table, bool closeCon)
        {
            string tableName = table.TableName;
            List<SQLiteColumn> columns = table.SQLiteColumns;
            StringBuilder sql = new StringBuilder();
            sql.Append("CREATE TABLE IF NOT EXISTS" + tableName + "(");

            foreach (SQLiteColumn column in columns)
            {
                sql.Append(column.ColumnName + " " + column.DataType + " " + (column.PrimaryKey ? "PRIMARY KEY " : " ") + (column.AutoIncrement ? "AUTOINCREMENT " : " ") + (column.NotNull ? "NOT NULL" : "") + ",");
            }
            sql.Remove(sql.Length - 1, 1);
            sql.Append(")");

            ExecuteResult(con, sql.ToString(), null, closeCon);
        }


        #endregion

        #region 设置参数
        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="db"></param>
        /// <param name="parameters"></param>
        private static void SetCmdParameter(SQLiteCommand db, Dictionary<string, object> parameters)
        {
            db.Parameters.Clear();
            if (parameters != null)
            {
                foreach (string key in parameters.Keys)
                {
                    SQLiteParameter p = db.CreateParameter();
                    p.Value = parameters[key];
                    p.ParameterName = key.ToString();
                    db.Parameters.Add(p);
                }
            }
        }
        #endregion

    }
}
