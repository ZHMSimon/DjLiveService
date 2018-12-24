//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Data;
//using System.Data.Common;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using log4net;

//namespace UtilTools.DbFactory.DbHelper
//{
//    internal class SqlServerDbHelper : IdbHelperInterface
//    {

//        public SqlServerDbHelper()
//        {
//            var serverIp = System.Configuration.ConfigurationManager.AppSettings["DbServer"];
//            string serverUserName = System.Configuration.ConfigurationManager.AppSettings["DBUserName"];
//            string serverPsd = System.Configuration.ConfigurationManager.AppSettings["DbPassword"];
//            string dbName = System.Configuration.ConfigurationManager.AppSettings["DbName"];
//            _mConnectionString = $"server={serverIp} ;initial catalog={dbName};user Id={serverUserName};password={serverPsd};Connection Timeout=15;Pooling=true;Min Pool Size=10;Max Pool Size=150;";
//        }
//        public SqlServerDbHelper(string serverIp, string serverUserName, string serverPsd, string dbName)
//        {
//            _mConnectionString = $"server= {serverIp} ;initial catalog ={dbName};user Id={serverUserName};password={serverPsd};Connection Timeout=15;Pooling=true;Min Pool Size=10;Max Pool Size=150;";
//        }

//        #region 字段

//        /// <summary>
//        /// 数据库连接器
//        /// </summary>
//        private SqlConnection _mConnection;
//        /// <summary>
//        /// 事务
//        /// </summary>
//        private SqlTransaction _mTransaction;

//        /// <summary>
//        /// 事务开关状态
//        /// </summary>
//        private bool _mTransactionState = false;

//        /// <summary>
//        /// 数据库连接字符串
//        /// </summary>
//        private readonly string _mConnectionString;

//        #endregion

//        #region 接口函数

//        /// <summary>
//        /// 打开数据库连接器
//        /// </summary>
//        public void OpenConnection()
//        {
//            ConnectionOpen(ref _mConnection, this._mConnectionString);
//        }
//        /// <summary>
//        /// 关闭数据库连接器
//        /// </summary>
//        public void CloseConnection()
//        {
//            if (_mConnection.State != ConnectionState.Closed)
//                _mConnection.Close();
//        }
//        /// <summary>
//        /// 开始事务
//        /// </summary>
//        public void BeginTran()
//        {
//            this._mTransactionState = true;
//            this.OpenConnection();
//            if (_mTransaction != null)
//                _mTransaction.Dispose();
//            _mTransaction = _mConnection.BeginTransaction();
//        }
//        /// <summary>
//        /// 提交事务
//        /// </summary>
//        public void CommitTran()
//        {
//            this._mTransactionState = false;
//            if (_mTransaction != null)
//                _mTransaction.Commit();
//            CloseConnection();
//        }
//        /// <summary>
//        /// 回滚事务
//        /// </summary>
//        public void RollBackTran()
//        {
//            this._mTransactionState = false;
//            if (_mTransaction != null)
//                _mTransaction.Rollback();
//            CloseConnection();
//        }



//        /// <summary>
//        /// 执行一个sql语句
//        /// </summary>
//        /// <param name="strCmd">sql语句或存储过程名</param>
//        /// <param name="cmdTimeOut"></param>
//        /// <returns>返回影响行数</returns>
//        public int ExecuteNoneQuery(string strCmd, int cmdTimeOut = 30)
//        {
//            int row = -1;
//            if (this._mTransactionState)
//            {
//                SqlCommand cmd = new SqlCommand();
//                cmd.Connection = _mConnection;
//                cmd.CommandText = strCmd;
//                cmd.CommandTimeout = cmdTimeOut;
//                cmd.Transaction = _mTransaction;
//                row = cmd.ExecuteNonQuery();
//            }
//            else
//            {
//                SqlConnection conn = null;
//                try
//                {
//                    ConnectionOpen(ref conn, this._mConnectionString);
//                    SqlCommand cmd = new SqlCommand(strCmd, conn) { CommandTimeout = cmdTimeOut };

//                    row = cmd.ExecuteNonQuery();
//                }
//                finally
//                {
//                    if (conn != null)
//                        conn.Close();
//                }
//            }
//            return row;
//        }

//        public int ExecuteNoneQuery(string strCmd, DbParameter[] parameters, int cmdTimeOut = 30)
//        {
//            int row = -1;
//            if (this._mTransactionState)
//            {
//                SqlCommand cmd = new SqlCommand();
//                cmd.Connection = _mConnection;
//                cmd.CommandText = strCmd;
//                cmd.CommandTimeout = cmdTimeOut;
//                cmd.Transaction = _mTransaction;
//                cmd.Parameters.AddRange(parameters);
//                row = cmd.ExecuteNonQuery();
//            }
//            else
//            {
//                SqlConnection conn = null;
//                try
//                {
//                    ConnectionOpen(ref conn, this._mConnectionString);
//                    SqlCommand cmd = new SqlCommand(strCmd, conn) { CommandTimeout = cmdTimeOut };
//                    cmd.Parameters.AddRange(parameters);
//                    row = cmd.ExecuteNonQuery();
//                }
//                finally
//                {
//                    if (conn != null)
//                        conn.Close();
//                }
//            }
//            return row;
//        }

//        public DbDataReader GetDataReader(string sql, DbParameter[] parameters = null, int cmdTimeOut = 30)
//        {
//            throw new NotImplementedException();
//        }
       

//        /// <summary>
//        /// 执行一个sql语句
//        /// </summary>
//        /// <param name="strCmd">sql语句或存储过程名</param>
//        /// <param name="cmdType">存储过程类型</param>
//        /// <param name="fields">参数名称</param>
//        /// <param name="obj">值集合</param>
//        /// <returns>返回影响行数</returns>
//        public int ExecuteNoneQuery(string strCmd, CommandType cmdType, string[] fields, object[] obj, int cmdTimeOut = 30)
//        {
//            int row = -1;
//            if (this._mTransactionState)
//            {
//                SqlCommand cmd = GetCommand(strCmd, cmdType, fields, obj);
//                cmd.CommandTimeout = cmdTimeOut;
//                cmd.Transaction = _mTransaction;
//                row = cmd.ExecuteNonQuery();
//            }
//            else
//            {
//                SqlConnection conn = null;
//                try
//                {
//                    ConnectionOpen(ref conn, this._mConnectionString);
//                    SqlCommand cmd = GetCommand2(conn, cmdTimeOut, strCmd, cmdType, fields, obj);

//                    row = cmd.ExecuteNonQuery();
//                }
//                finally
//                {
//                    if (conn != null)
//                        conn.Close();
//                }
//            }
//            return row;
//        }
//        /// <summary>
//        /// 执行查询，返回查询的第一行第一列，忽略其他列
//        /// </summary>
//        /// <param name="strCmd">sql语句或存储过程名</param>
//        /// <returns>返回一个obj对象</returns>
//        public object ExecuteScalar(string strCmd, DbParameter[] parms = null, int cmdTimeOut = 30)
//        {
//            object result = null;
//            if (this._mTransactionState)
//            {
//                SqlCommand cmd = new SqlCommand(strCmd, _mConnection);
//                cmd.CommandTimeout = cmdTimeOut;
//                cmd.Transaction = _mTransaction;
//                cmd.Parameters.AddRange(parms);
//                result = cmd.ExecuteScalar();
//            }
//            else
//            {
//                SqlConnection conn = null;
//                try
//                {
//                    ConnectionOpen(ref conn, this._mConnectionString);
//                    SqlCommand cmd = new SqlCommand(strCmd, conn) { CommandTimeout = cmdTimeOut };

//                    result = cmd.ExecuteScalar();
//                }
//                finally
//                {
//                    if (conn != null)
//                        conn.Close();
//                }
//            }
//            return result;
//        }

//        public object ExecuteScalar(string strCmd, SqlParameter[] parameters, int cmdTimeOut = 30)
//        {
//            object result = null;
//            if (this._mTransactionState)
//            {
//                SqlCommand cmd = new SqlCommand(strCmd, _mConnection);
//                cmd.CommandTimeout = cmdTimeOut;
//                cmd.Transaction = _mTransaction;
//                cmd.Parameters.AddRange(parameters);
//                result = cmd.ExecuteScalar();
//            }
//            else
//            {
//                SqlConnection conn = null;
//                try
//                {
//                    ConnectionOpen(ref conn, this._mConnectionString);
//                    SqlCommand cmd = new SqlCommand(strCmd, conn) { CommandTimeout = cmdTimeOut };
//                    cmd.Parameters.AddRange(parameters);
//                    result = cmd.ExecuteScalar();
//                }
//                finally
//                {
//                    if (conn != null)
//                        conn.Close();
//                }
//            }
//            return result;
//        }
//        /// <summary>
//        /// 执行查询，返回查询的第一行第一列，忽略其他列
//        /// </summary>
//        /// <param name="strCmd">sql语句或存储过程名</param>
//        /// <param name="cmdType">存储过程类型</param>
//        /// <param name="fields">参数集合</param>
//        /// <param name="obj">值集合</param>
//        /// <returns>返回一个obj对象</returns>
//        public object ExecuteScalar(string strCmd, System.Data.CommandType cmdType, string[] fields, object[] obj, int cmdTimeOut = 30)
//        {

//            object result = null;
//            if (this._mTransactionState)
//            {
//                SqlCommand cmd = GetCommand(strCmd, cmdType, fields, obj);
//                cmd.CommandTimeout = cmdTimeOut;
//                cmd.Transaction = _mTransaction;
//                result = cmd.ExecuteScalar();
//            }
//            else
//            {
//                SqlConnection conn = null;
//                try
//                {
//                    ConnectionOpen(ref conn, this._mConnectionString);
//                    SqlCommand cmd = GetCommand2(conn, cmdTimeOut, strCmd, cmdType, fields, obj);

//                    result = cmd.ExecuteScalar();
//                }
//                finally
//                {
//                    if (conn != null)
//                        conn.Close();
//                }
//            }
//            return result;
//        }
//        /// <summary>
//        /// 执行查询，返回一个数据表
//        /// </summary>
//        /// <param name="strCmd">sql语句或存储过程名</param>
//        /// <returns>返回一个数据表</returns>
//        public DataTable Gettable(string strCmd, int cmdTimeOut = 30)
//        {
//            DataTable result = new DataTable();
//            if (this._mTransactionState)
//            {
//                SqlCommand cmd = new SqlCommand(strCmd, _mConnection, _mTransaction);
//                cmd.CommandTimeout = cmdTimeOut;
//                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
//                adapter.Fill(result);
//            }
//            else
//            {
//                SqlConnection conn = null;
//                try
//                {
//                    ConnectionOpen(ref conn, this._mConnectionString);
//                    SqlCommand cmd = new SqlCommand(strCmd, conn) { CommandTimeout = cmdTimeOut };

//                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
//                    adapter.Fill(result);
//                }
//                finally
//                {
//                    if (conn != null)
//                        conn.Close();
//                }
//            }
//            return result;
//        }

//        /// <summary>
//        /// 执行查询，返回一个数据表
//        /// </summary>
//        /// <param name="strCmd">sql语句或存储过程名</param>
//        /// <returns>返回一个数据表</returns>
//        public DataTable GetTable(string strCmd, int cmdTimeOut = 30)
//        {
//            DataTable result = new DataTable();
//            if (this._mTransactionState)
//            {
//                SqlCommand cmd = new SqlCommand(strCmd, _mConnection, _mTransaction);
//                cmd.CommandTimeout = cmdTimeOut;
//                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
//                adapter.Fill(result);
//            }
//            else
//            {
//                SqlConnection conn = null;
//                try
//                {
//                    ConnectionOpen(ref conn, this._mConnectionString);
//                    SqlCommand cmd = new SqlCommand(strCmd, conn) { CommandTimeout = cmdTimeOut };

//                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
//                    adapter.Fill(result);
//                }
//                finally
//                {
//                    if (conn != null)
//                        conn.Close();
//                }
//            }
//            return result;
//        }

//        /// <summary>
//        /// 执行查询，返回一个数据表
//        /// </summary>
//        /// <param name="strCmd">sql语句或存储过程名</param>
//        /// <param name="cmdType">存储过程类型</param>
//        /// <param name="fields">参数集合</param>
//        /// <param name="obj">值集合</param>
//        /// <returns>返回一个数据表</returns>
//        public DataTable GetTable(string strCmd, CommandType cmdType, string[] fields, object[] obj, int cmdTimeOut = 30)
//        {
//            DataTable result = new DataTable();
//            if (this._mTransactionState)
//            {
//                SqlCommand cmd = GetCommand(strCmd, cmdType, fields, obj);
//                cmd.Transaction = _mTransaction;
//                cmd.CommandTimeout = cmdTimeOut;
//                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
//                adapter.Fill(result);
//            }
//            else
//            {
//                SqlConnection conn = null;
//                try
//                {
//                    ConnectionOpen(ref conn, this._mConnectionString);
//                    SqlCommand cmd = GetCommand2(conn, cmdTimeOut, strCmd, cmdType, fields, obj);

//                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
//                    adapter.Fill(result);
//                }
//                finally
//                {
//                    if (conn != null)
//                        conn.Close();
//                }
//            }
//            return result;
//        }

//        public DataTable GetTable(string strCmd, DbParameter[] parms =null, int cmdTimeOut = 30)
//        {
//            DataTable result = new DataTable();
//            if (this._mTransactionState)
//            {
//                SqlCommand cmd = new SqlCommand(strCmd, _mConnection);
//                cmd.Transaction = _mTransaction;
//                cmd.CommandTimeout = cmdTimeOut;
//                cmd.Parameters.AddRange(parms);
//                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
//                adapter.Fill(result);
//            }
//            else
//            {
//                SqlConnection conn = null;
//                try
//                {
//                    ConnectionOpen(ref conn, this._mConnectionString);
//                    SqlCommand cmd = new SqlCommand(strCmd, conn)
//                    {
//                        CommandTimeout = cmdTimeOut,
//                        CommandType = CommandType.Text,
//                    };
//                    cmd.Parameters.AddRange(parms);
//                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
//                    adapter.Fill(result);
//                }
//                finally
//                {
//                    if (conn != null)
//                        conn.Close();
//                }
//            }
//            return result;
//        }

//        public string GetFieldValue(string sql, SqlParameter[] parameters = null, int cmdTimeOut = 30)
//        {
//            throw new NotImplementedException();
//        }

//        public string GetPaerSql(string toString, int size, int number, out long count, SqlParameter[] toArray)
//        {
//            throw new NotImplementedException();
//        }

//        /// <summary>
//        /// 执行查询，返回一个数据集
//        /// </summary>
//        /// <param name="strCmd">sql语句或存储过程名</param>
//        /// <returns>返回一个数据集</returns>
//        public DataSet GetSet(string strCmd, int cmdTimeOut = 30)
//        {
//            DataSet result = new DataSet();
//            if (this._mTransactionState)
//            {
//                SqlCommand cmd = new SqlCommand(strCmd, _mConnection, _mTransaction);
//                cmd.CommandTimeout = cmdTimeOut;
//                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
//                adapter.Fill(result);
//            }
//            else
//            {
//                SqlConnection conn = null;
//                try
//                {
//                    ConnectionOpen(ref conn, this._mConnectionString);
//                    SqlCommand cmd = new SqlCommand(strCmd, conn) { CommandTimeout = cmdTimeOut };

//                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
//                    adapter.Fill(result);
//                }
//                finally
//                {
//                    if (conn != null)
//                        conn.Close();
//                }
//            }
//            return result;
//        }
//        /// <summary>
//        /// 执行查询，返回一个数据集
//        /// </summary>
//        /// <param name="strCmd">sql语句或存储过程名</param>
//        /// <param name="cmdType">存储过程类型</param>
//        /// <param name="fields">参数集合</param>
//        /// <param name="obj">值集合</param>
//        /// <returns>返回一个数据集</returns>
//        public DataSet GetSet(string strCmd, CommandType cmdType, string[] fields, object[] obj, int cmdTimeOut = 30)
//        {
//            DataSet result = new DataSet();
//            if (this._mTransactionState)
//            {
//                SqlCommand cmd = GetCommand(strCmd, cmdType, fields, obj);
//                cmd.Transaction = _mTransaction;
//                cmd.CommandTimeout = cmdTimeOut;
//                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
//                adapter.Fill(result);
//            }
//            else
//            {
//                SqlConnection conn = null;
//                try
//                {
//                    ConnectionOpen(ref conn, this._mConnectionString);
//                    SqlCommand cmd = GetCommand2(conn, cmdTimeOut, strCmd, cmdType, fields, obj);

//                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
//                    adapter.Fill(result);
//                }
//                finally
//                {
//                    if (conn != null)
//                        conn.Close();
//                }
//            }
//            return result;
//        }


//        public DataSet GetSet(string strCmd, SqlParameter[] parms, int cmdTimeOut = 30)
//        {
//            DataSet result = new DataSet();
//            if (this._mTransactionState)
//            {
//                SqlCommand cmd = new SqlCommand(strCmd, _mConnection);
//                cmd.Transaction = _mTransaction;
//                cmd.CommandTimeout = cmdTimeOut;
//                cmd.Parameters.AddRange(parms);
//                cmd.CommandType = CommandType.Text;
//                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
//                adapter.Fill(result);
//            }
//            else
//            {
//                SqlConnection conn = null;
//                try
//                {
//                    ConnectionOpen(ref conn, this._mConnectionString);
//                    SqlCommand cmd = new SqlCommand(strCmd, conn)
//                    {
//                        CommandTimeout = cmdTimeOut,
//                        CommandType = CommandType.Text,
//                    };
//                    cmd.Parameters.AddRange(parms);
//                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
//                    adapter.Fill(result);
//                }
//                finally
//                {
//                    if (conn != null)
//                        conn.Close();
//                }
//            }
//            return result;
//        }
//        /// <summary>
//        /// 批量导入数据库(注意导入的时候dt中的结构要和目标表结构完全一致才行,自增长除外)
//        /// </summary>
//        /// <param name="tableName">服务器上目标表的名称</param>
//        /// <param name="dt">要导入的数据集</param>
//        public int BulkInDataServer(string tableName, DataTable dt)
//        {
//            if (this._mTransactionState)
//            {
//                //第二个参数为枚举特性
//                SqlBulkCopy bulkCopy = new SqlBulkCopy(_mConnection, SqlBulkCopyOptions.FireTriggers, _mTransaction);
//                bulkCopy.DestinationTableName = tableName;
//                bulkCopy.BulkCopyTimeout = 120;
//                bulkCopy.WriteToServer(dt);
//            }
//            else
//            {
//                SqlConnection conn = null;
//                try
//                {
//                    ConnectionOpen(ref conn, this._mConnectionString);

//                    SqlBulkCopy bulkCopy = new SqlBulkCopy(conn);
//                    bulkCopy.DestinationTableName = tableName;
//                    bulkCopy.BulkCopyTimeout = 120;
//                    bulkCopy.WriteToServer(dt);
//                }
//                finally
//                {
//                    if (conn != null)
//                        conn.Close();
//                }
//            }
//            return dt.Rows.Count;
//        }

//        /// <summary>
//        /// 获取sqlserver连接并Open
//        /// </summary>
//        /// <param name="conn"></param>
//        /// <param name="connStr"></param>
//        /// <returns></returns>
//        private static bool ConnectionOpen(ref SqlConnection conn, string connStr)
//        {
//            //首次创建
//            if (conn == null)
//                conn = new SqlConnection(connStr);

//            try
//            {
//                conn.Open();
//            }
//            catch (SqlException ex)
//            {
//                if (conn != null)
//                    conn.Close();
//                SqlConnection.ClearPool(conn);                      //不放入连接池

//                //再次创建
//                conn = new SqlConnection(connStr);
//                try
//                {
//                    conn.Open();
//                }
//                catch (SqlException)
//                {
//                    if (conn != null)
//                        conn.Close();
//                    SqlConnection.ClearPool(conn);                  //不放入连接池

//                    //第三次,改为不使用连接池访问
//                    conn = new SqlConnection(connStr.Replace("Pooling=true;", "Pooling=false;"));
//                    try
//                    {
//                        conn.Open();
//                    }
//                    catch (SqlException)
//                    {
//                        if (conn != null)
//                            conn.Close();
//                    }
//                }
//            }

//            return true;
//        }
//        #endregion

//        #region 非接口函数
//        /// <summary>
//        /// 整理查询参数
//        /// </summary>
//        /// <param name="fields">参数集合</param>
//        /// <param name="obj">值集合</param>
//        /// <returns>返回SqlParameter集合</returns>
//        private SqlParameter[] GetSqlParameters(string[] fields, object[] obj)
//        {
//            if (fields == null || obj == null)
//                return new SqlParameter[] { };
//            try
//            {
//                SqlParameter[] parameters = new SqlParameter[obj.Length];
//                for (int i = 0; i < obj.Length; i++)
//                {
//                    parameters[i] = new SqlParameter(fields[i], obj[i]);
//                }
//                return parameters;
//            }
//            catch
//            {
//                return new SqlParameter[] { };
//            }
//        }

//        /// <summary>
//        /// 无事务构造SqlCommand
//        /// </summary>
//        /// <param name="conn"></param>
//        /// <param name="cmdTimeout"></param>
//        /// <param name="strCmd">sql语句或存储过程名</param>
//        /// <param name="cmdType">存储过程类型</param>
//        /// <param name="fields">参数集合</param>
//        /// <param name="obj">值集合</param>
//        /// <returns>返回一个sql查询器</returns>
//        private SqlCommand GetCommand2(SqlConnection conn, int cmdTimeout, string strCmd, CommandType cmdType, string[] fields, object[] obj)
//        {
//            SqlCommand cmd = new SqlCommand(strCmd, conn);
//            cmd.CommandType = cmdType;
//            cmd.CommandTimeout = cmdTimeout;

//            SqlParameter[] paras = GetSqlParameters(fields, obj);
//            if (paras != null)
//            {
//                cmd.Parameters.AddRange(paras);
//            }
//            return cmd;
//        }

//        /// <summary>
//        /// 加载查询参数
//        /// </summary>
//        /// <param name="strCmd">sql语句或存储过程名</param>
//        /// <param name="cmdType">存储过程类型</param>
//        /// <param name="fields">参数集合</param>
//        /// <param name="obj">值集合</param>
//        /// <returns>返回一个sql查询器</returns>
//        private SqlCommand GetCommand(string strCmd, CommandType cmdType, string[] fields, object[] obj)
//        {
//            SqlParameter[] paras = GetSqlParameters(fields, obj);
//            SqlCommand cmd = new SqlCommand(strCmd, _mConnection);
//            cmd.CommandType = cmdType;
//            if (this._mTransactionState)
//                cmd.Transaction = _mTransaction;
//            if (paras != null)
//            {
//                foreach (SqlParameter para in paras)
//                    cmd.Parameters.Add(para);
//            }
//            return cmd;
//        }


//        /// <summary>
//        /// 自动回收
//        /// </summary>
//        public void Dispose()
//        {
//            //析构自动调用
//            GC.SuppressFinalize(true);
//        }
//        #endregion
//    }
//}
