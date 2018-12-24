//using System;
//using System.Data;
//using System.Data.Common;
//using System.IO;
//using System.Text;
//using MySql.Data.MySqlClient;


//namespace UtilTools.DbFactory.DbHelper
//{
//    internal class MySqlDbHelper : IdbHelperInterface
//    {

//        public MySqlDbHelper()
//        {
//            var serverIp = System.Configuration.ConfigurationManager.AppSettings["DbServer"];
//            string serverUserName = System.Configuration.ConfigurationManager.AppSettings["DbUserName"];
//            string serverPsd = System.Configuration.ConfigurationManager.AppSettings["DbPassword"];
//            string dbName = System.Configuration.ConfigurationManager.AppSettings["DbName"];
//            _mConnectionString = $"server={serverIp} ;initial catalog={dbName};user Id={serverUserName};password={serverPsd};Connection Timeout=15;Pooling=true;Min Pool Size=10;Max Pool Size=150;";
//        }
//        public MySqlDbHelper(string serverIp, string serverUserName, string serverPsd, string dbName)
//        {
//            _mConnectionString = $"server={serverIp} ;initial catalog ={dbName};user Id={serverUserName};password={serverPsd};Connection Timeout=15;Pooling=true;Min Pool Size=10;Max Pool Size=150;";
//        }

//        #region 字段

//        /// <summary>
//        /// 数据库连接器
//        /// </summary>
//        private MySqlConnection _mConnection;
//        /// <summary>
//        /// 事务
//        /// </summary>
//        private MySqlTransaction _mTransaction;

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
//            _mTransaction?.Commit();
//            CloseConnection();
//        }
//        /// <summary>
//        /// 回滚事务
//        /// </summary>
//        public void RollBackTran()
//        {
//            this._mTransactionState = false;
//            _mTransaction?.Rollback();
//            CloseConnection();
//        }



//        /// <summary>
//        /// 执行一个MySql语句
//        /// </summary>
//        /// <param name="strCmd">MySql语句或存储过程名</param>
//        /// <param name="cmdTimeOut"></param>
//        /// <returns>返回影响行数</returns>
//        public int ExecuteNoneQuery(string strCmd, int cmdTimeOut = 30)
//        {
//            int row = -1;
//            if (this._mTransactionState)
//            {
//                MySqlCommand cmd = new MySqlCommand
//                {
//                    Connection = _mConnection,
//                    CommandText = strCmd,
//                    CommandTimeout = cmdTimeOut,
//                    Transaction = _mTransaction
//                };
//                row = cmd.ExecuteNonQuery();
//            }
//            else
//            {
//                MySqlConnection conn = null;
//                try
//                {
//                    ConnectionOpen(ref conn, this._mConnectionString);
//                    MySqlCommand cmd = new MySqlCommand(strCmd, conn) { CommandTimeout = cmdTimeOut };

//                    row = cmd.ExecuteNonQuery();
//                }
//                finally
//                {
//                    conn?.Close();
//                }
//            }
//            return row;
//        }

//        public int ExecuteNoneQuery(string strCmd, DbParameter[] parameters, int cmdTimeOut = 30)
//        {
//            int row = -1;
//            if (this._mTransactionState)
//            {
//                MySqlCommand cmd = new MySqlCommand
//                {
//                    Connection = _mConnection,
//                    CommandText = strCmd,
//                    CommandTimeout = cmdTimeOut,
//                    Transaction = _mTransaction
//                };
//                cmd.Parameters.AddRange(parameters);
//                row = cmd.ExecuteNonQuery();
//            }
//            else
//            {
//                MySqlConnection conn = null;
//                try
//                {
//                    ConnectionOpen(ref conn, this._mConnectionString);
//                    MySqlCommand cmd = new MySqlCommand(strCmd, conn) { CommandTimeout = cmdTimeOut };
//                    var c = parameters as MySqlParameter[];
//                    cmd.Parameters.AddRange(c);
//                    row = cmd.ExecuteNonQuery();
//                }
//                finally
//                {
//                    conn?.Close();
//                }
//            }
//            return row;
//        }
//        /// <summary>
//        /// 执行一个MySql语句
//        /// </summary>
//        /// <param name="strCmd">MySql语句或存储过程名</param>
//        /// <param name="cmdType">存储过程类型</param>
//        /// <param name="fields">参数名称</param>
//        /// <param name="obj">值集合</param>
//        /// <returns>返回影响行数</returns>
//        public int ExecuteNoneQuery(string strCmd, CommandType cmdType, string[] fields, object[] obj, int cmdTimeOut = 30)
//        {
//            int row = -1;
//            if (this._mTransactionState)
//            {
//                MySqlCommand cmd = GetCommand(strCmd, cmdType, fields, obj);
//                cmd.CommandTimeout = cmdTimeOut;
//                cmd.Transaction = _mTransaction;
//                row = cmd.ExecuteNonQuery();
//            }
//            else
//            {
//                MySqlConnection conn = null;
//                try
//                {
//                    ConnectionOpen(ref conn, this._mConnectionString);
//                    MySqlCommand cmd = GetCommand2(conn, cmdTimeOut, strCmd, cmdType, fields, obj);

//                    row = cmd.ExecuteNonQuery();
//                }
//                finally
//                {
//                    conn?.Close();
//                }
//            }
//            return row;
//        }
//        /// <summary>
//        /// 执行查询，返回查询的第一行第一列，忽略其他列
//        /// </summary>
//        /// <param name="strCmd">MySql语句或存储过程名</param>
//        /// <returns>返回一个obj对象</returns>
//        public object ExecuteScalar(string strCmd, DbParameter[] parms = null, int cmdTimeOut = 30)
//        {
//            object result = null;
//            if (this._mTransactionState)
//            {
//                MySqlCommand cmd = new MySqlCommand(strCmd, _mConnection);
//                if (parms != null) cmd.Parameters.AddRange(parms);
//                cmd.CommandTimeout = cmdTimeOut;
//                cmd.Transaction = _mTransaction;
//                result = cmd.ExecuteScalar();
//            }
//            else
//            {
//                MySqlConnection conn = null;
//                try
//                {
//                    ConnectionOpen(ref conn, this._mConnectionString);
//                    MySqlCommand cmd = new MySqlCommand(strCmd, conn) { CommandTimeout = cmdTimeOut };

//                    result = cmd.ExecuteScalar();
//                }
//                finally
//                {
//                    conn?.Close();
//                }
//            }
//            return result;
//        }

//        public object ExecuteScalar(string strCmd, MySqlParameter[] parameters, int cmdTimeOut = 30)
//        {
//            object result = null;
//            if (this._mTransactionState)
//            {
//                MySqlCommand cmd = new MySqlCommand(strCmd, _mConnection);
//                cmd.CommandTimeout = cmdTimeOut;
//                cmd.Transaction = _mTransaction;
//                cmd.Parameters.AddRange(parameters);
//                result = cmd.ExecuteScalar();
//            }
//            else
//            {
//                MySqlConnection conn = null;
//                try
//                {
//                    ConnectionOpen(ref conn, this._mConnectionString);
//                    MySqlCommand cmd = new MySqlCommand(strCmd, conn) { CommandTimeout = cmdTimeOut };
//                    cmd.Parameters.AddRange(parameters);
//                    result = cmd.ExecuteScalar();
//                }
//                finally
//                {
//                    conn?.Close();
//                }
//            }
//            return result;
//        }
//        /// <summary>
//        /// 执行查询，返回查询的第一行第一列，忽略其他列
//        /// </summary>
//        /// <param name="strCmd">MySql语句或存储过程名</param>
//        /// <param name="cmdType">存储过程类型</param>
//        /// <param name="fields">参数集合</param>
//        /// <param name="obj">值集合</param>
//        /// <returns>返回一个obj对象</returns>
//        public object ExecuteScalar(string strCmd, System.Data.CommandType cmdType, string[] fields, object[] obj, int cmdTimeOut = 30)
//        {

//            object result = null;
//            if (this._mTransactionState)
//            {
//                MySqlCommand cmd = GetCommand(strCmd, cmdType, fields, obj);
//                cmd.CommandTimeout = cmdTimeOut;
//                cmd.Transaction = _mTransaction;
//                result = cmd.ExecuteScalar();
//            }
//            else
//            {
//                MySqlConnection conn = null;
//                try
//                {
//                    ConnectionOpen(ref conn, this._mConnectionString);
//                    MySqlCommand cmd = GetCommand2(conn, cmdTimeOut, strCmd, cmdType, fields, obj);

//                    result = cmd.ExecuteScalar();
//                }
//                finally
//                {
//                    conn?.Close();
//                }
//            }
//            return result;
//        }
//        /// <summary>
//        /// 执行查询，返回一个数据表
//        /// </summary>
//        /// <param name="strCmd">MySql语句或存储过程名</param>
//        /// <returns>返回一个数据表</returns>
//        public DataTable Gettable(string strCmd, int cmdTimeOut = 30)
//        {
//            DataTable result = new DataTable();
//            if (this._mTransactionState)
//            {
//                MySqlCommand cmd = new MySqlCommand(strCmd, _mConnection, _mTransaction);
//                cmd.CommandTimeout = cmdTimeOut;
//                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
//                adapter.Fill(result);
//            }
//            else
//            {
//                MySqlConnection conn = null;
//                try
//                {
//                    ConnectionOpen(ref conn, this._mConnectionString);
//                    MySqlCommand cmd = new MySqlCommand(strCmd, conn) { CommandTimeout = cmdTimeOut };

//                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
//                    adapter.Fill(result);
//                }
//                finally
//                {
//                    conn?.Close();
//                }
//            }
//            return result;
//        }

//        /// <summary>
//        /// 执行查询，返回一个数据表
//        /// </summary>
//        /// <param name="strCmd">MySql语句或存储过程名</param>
//        /// <returns>返回一个数据表</returns>
//        public DataTable GetTable(string strCmd, int cmdTimeOut = 30)
//        {
//            DataTable result = new DataTable();
//            if (this._mTransactionState)
//            {
//                MySqlCommand cmd = new MySqlCommand(strCmd, _mConnection, _mTransaction) {CommandTimeout = cmdTimeOut};
//                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
//                adapter.Fill(result);
//            }
//            else
//            {
//                MySqlConnection conn = null;
//                try
//                {
//                    ConnectionOpen(ref conn, this._mConnectionString);
//                    MySqlCommand cmd = new MySqlCommand(strCmd, conn) { CommandTimeout = cmdTimeOut };

//                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
//                    adapter.Fill(result);
//                }
//                finally
//                {
//                    conn?.Close();
//                }
//            }
//            return result;
//        }

//        /// <summary>
//        /// 执行查询，返回一个数据表
//        /// </summary>
//        /// <param name="strCmd">MySql语句或存储过程名</param>
//        /// <param name="cmdType">存储过程类型</param>
//        /// <param name="fields">参数集合</param>
//        /// <param name="obj">值集合</param>
//        /// <returns>返回一个数据表</returns>
//        public DataTable GetTable(string strCmd, CommandType cmdType, string[] fields, object[] obj, int cmdTimeOut = 30)
//        {
//            DataTable result = new DataTable();
//            if (this._mTransactionState)
//            {
//                MySqlCommand cmd = GetCommand(strCmd, cmdType, fields, obj);
//                cmd.Transaction = _mTransaction;
//                cmd.CommandTimeout = cmdTimeOut;
//                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
//                adapter.Fill(result);
//            }
//            else
//            {
//                MySqlConnection conn = null;
//                try
//                {
//                    ConnectionOpen(ref conn, this._mConnectionString);
//                    MySqlCommand cmd = GetCommand2(conn, cmdTimeOut, strCmd, cmdType, fields, obj);

//                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
//                    adapter.Fill(result);
//                }
//                finally
//                {
//                    conn?.Close();
//                }
//            }
//            return result;
//        }

//        public DataTable GetTable(string strCmd, DbParameter[] parms = null, int cmdTimeOut = 30)
//        {
//            DataTable result = new DataTable();
//            if (this._mTransactionState)
//            {
//                MySqlCommand cmd = new MySqlCommand(strCmd, _mConnection);
//                cmd.Transaction = _mTransaction;
//                cmd.CommandTimeout = cmdTimeOut;
//                if (parms != null) cmd.Parameters.AddRange(parms);
//                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
//                adapter.Fill(result);
//            }
//            else
//            {
//                MySqlConnection conn = null;
//                try
//                {
//                    ConnectionOpen(ref conn, this._mConnectionString);
//                    MySqlCommand cmd = new MySqlCommand(strCmd, conn)
//                    {
//                        CommandTimeout = cmdTimeOut,
//                        CommandType = CommandType.Text,
//                    };
//                    if (parms != null) cmd.Parameters.AddRange(parms);
//                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
//                    adapter.Fill(result);
//                }
//                finally
//                {
//                    conn?.Close();
//                }
//            }
//            return result;
//        }
//        /// <summary>
//        /// 执行查询，返回一个数据集
//        /// </summary>
//        /// <param name="strCmd">MySql语句或存储过程名</param>
//        /// <returns>返回一个数据集</returns>
//        public DataSet GetSet(string strCmd, int cmdTimeOut = 30)
//        {
//            DataSet result = new DataSet();
//            if (this._mTransactionState)
//            {
//                MySqlCommand cmd = new MySqlCommand(strCmd, _mConnection, _mTransaction);
//                cmd.CommandTimeout = cmdTimeOut;
//                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
//                adapter.Fill(result);
//            }
//            else
//            {
//                MySqlConnection conn = null;
//                try
//                {
//                    ConnectionOpen(ref conn, this._mConnectionString);
//                    MySqlCommand cmd = new MySqlCommand(strCmd, conn) { CommandTimeout = cmdTimeOut };

//                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
//                    adapter.Fill(result);
//                }
//                finally
//                {
//                    conn?.Close();
//                }
//            }
//            return result;
//        }
//        /// <summary>
//        /// 执行查询，返回一个数据集
//        /// </summary>
//        /// <param name="strCmd">MySql语句或存储过程名</param>
//        /// <param name="cmdType">存储过程类型</param>
//        /// <param name="fields">参数集合</param>
//        /// <param name="obj">值集合</param>
//        /// <returns>返回一个数据集</returns>
//        public DataSet GetSet(string strCmd, CommandType cmdType, string[] fields, object[] obj, int cmdTimeOut = 30)
//        {
//            DataSet result = new DataSet();
//            if (this._mTransactionState)
//            {
//                MySqlCommand cmd = GetCommand(strCmd, cmdType, fields, obj);
//                cmd.Transaction = _mTransaction;
//                cmd.CommandTimeout = cmdTimeOut;
//                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
//                adapter.Fill(result);
//            }
//            else
//            {
//                MySqlConnection conn = null;
//                try
//                {
//                    ConnectionOpen(ref conn, this._mConnectionString);
//                    MySqlCommand cmd = GetCommand2(conn, cmdTimeOut, strCmd, cmdType, fields, obj);

//                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
//                    adapter.Fill(result);
//                }
//                finally
//                {
//                    conn?.Close();
//                }
//            }
//            return result;
//        }


//        public DataSet GetSet(string strCmd, MySqlParameter[] parms, int cmdTimeOut = 30)
//        {
//            DataSet result = new DataSet();
//            if (this._mTransactionState)
//            {
//                MySqlCommand cmd = new MySqlCommand(strCmd, _mConnection)
//                {
//                    Transaction = _mTransaction,
//                    CommandTimeout = cmdTimeOut
//                };
//                cmd.Parameters.AddRange(parms);
//                cmd.CommandType = CommandType.Text;
//                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
//                adapter.Fill(result);
//            }
//            else
//            {
//                MySqlConnection conn = null;
//                try
//                {
//                    ConnectionOpen(ref conn, this._mConnectionString);
//                    MySqlCommand cmd = new MySqlCommand(strCmd, conn)
//                    {
//                        CommandTimeout = cmdTimeOut,
//                        CommandType = CommandType.Text,
//                    };
//                    cmd.Parameters.AddRange(parms);
//                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
//                    adapter.Fill(result);
//                }
//                finally
//                {
//                    conn?.Close();
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
//            if (string.IsNullOrWhiteSpace(tableName)) throw new Exception("请给DataTable的TableName属性附上表名称");
//            if (dt.Rows.Count == 0) return 0;
//            string tmpPath = Path.GetTempFileName();
//            string csv = DataTableToCsv(dt);
//            File.WriteAllText(tmpPath, csv);
//            MySqlConnection conn = null;
//            try
//            {
//                ConnectionOpen(ref conn, this._mConnectionString);
//                MySqlBulkLoader bulkCopy = new MySqlBulkLoader(conn)
//                {
//                    FieldTerminator = ",",
//                    FieldQuotationCharacter = '"',
//                    EscapeCharacter = '"',
//                    LineTerminator = "\r\n",
//                    FileName = tmpPath,
//                    NumberOfLinesToSkip = 0,
//                    TableName = tableName,
//                };
//                return bulkCopy.Load();
//            }
//            finally
//            {
//                conn?.Close();
//                File.Delete(tmpPath);
//            }
//        }
//        public DbDataReader GetDataReader(string sql, DbParameter[] parameters = null, int cmdTimeOut = 30)
//        {
//            throw new NotImplementedException();
//        }


//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="table"></param>
//        /// <returns></returns>
//        private static string DataTableToCsv(DataTable table)
//        {
//            //以半角逗号（即,）作分隔符，列为空也要表达其存在。  
//            //列内容如存在半角逗号（即,）则用半角引号（即""）将该字段值包含起来。  
//            //列内容如存在半角引号（即"）则应替换成半角双引号（""）转义，并用半角引号（即""）将该字段值包含起来。  
//            StringBuilder sb = new StringBuilder();
//            DataColumn colum;
//            foreach (DataRow row in table.Rows)
//            {
//                for (int i = 0; i < table.Columns.Count; i++)
//                {
//                    colum = table.Columns[i];
//                    if (i != 0) sb.Append(",");
//                    if (colum.DataType == typeof(string) && row[colum].ToString().Contains(","))
//                    {
//                        sb.Append("\"" + row[colum].ToString().Replace("\"", "\"\"") + "\"");
//                    }
//                    else sb.Append(row[colum].ToString());
//                }
//                sb.AppendLine();
//            }


//            return sb.ToString();
//        }
//        /// <summary>
//        /// 获取MySqlserver连接并Open
//        /// </summary>
//        /// <param name="conn"></param>
//        /// <param name="connStr"></param>
//        /// <returns></returns>
//        private static bool ConnectionOpen(ref MySqlConnection conn, string connStr)
//        {
//            //首次创建
//            if (conn == null)
//                conn = new MySqlConnection(connStr);

//            try
//            {
//                conn.Open();
//            }
//            catch (MySqlException ex)
//            {
//                conn?.Close();
//                MySqlConnection.ClearPool(conn);                      //不放入连接池

//                //再次创建
//                conn = new MySqlConnection(connStr);
//                try
//                {
//                    conn.Open();
//                }
//                catch (MySqlException)
//                {
//                    conn?.Close();
//                    MySqlConnection.ClearPool(conn);                  //不放入连接池

//                    //第三次,改为不使用连接池访问
//                    conn = new MySqlConnection(connStr.Replace("Pooling=true;", "Pooling=false;"));
//                    try
//                    {
//                        conn.Open();
//                    }
//                    catch (MySqlException)
//                    {
//                        conn?.Close();
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
//        /// <returns>返回MySqlParameter集合</returns>
//        private MySqlParameter[] GetMySqlParameters(string[] fields, object[] obj)
//        {
//            if (fields == null || obj == null)
//                return new MySqlParameter[] { };
//            try
//            {
//                MySqlParameter[] parameters = new MySqlParameter[obj.Length];
//                for (int i = 0; i < obj.Length; i++)
//                {
//                    parameters[i] = new MySqlParameter(fields[i], obj[i]);
//                }
//                return parameters;
//            }
//            catch
//            {
//                return new MySqlParameter[] { };
//            }
//        }

//        /// <summary>
//        /// 无事务构造MySqlCommand
//        /// </summary>
//        /// <param name="conn"></param>
//        /// <param name="cmdTimeout"></param>
//        /// <param name="strCmd">MySql语句或存储过程名</param>
//        /// <param name="cmdType">存储过程类型</param>
//        /// <param name="fields">参数集合</param>
//        /// <param name="obj">值集合</param>
//        /// <returns>返回一个MySql查询器</returns>
//        private MySqlCommand GetCommand2(MySqlConnection conn, int cmdTimeout, string strCmd, CommandType cmdType, string[] fields, object[] obj)
//        {
//            MySqlCommand cmd = new MySqlCommand(strCmd, conn);
//            cmd.CommandType = cmdType;
//            cmd.CommandTimeout = cmdTimeout;

//            MySqlParameter[] paras = GetMySqlParameters(fields, obj);
//            if (paras != null)
//            {
//                cmd.Parameters.AddRange(paras);
//            }
//            return cmd;
//        }

//        /// <summary>
//        /// 加载查询参数
//        /// </summary>
//        /// <param name="strCmd">MySql语句或存储过程名</param>
//        /// <param name="cmdType">存储过程类型</param>
//        /// <param name="fields">参数集合</param>
//        /// <param name="obj">值集合</param>
//        /// <returns>返回一个MySql查询器</returns>
//        private MySqlCommand GetCommand(string strCmd, CommandType cmdType, string[] fields, object[] obj)
//        {
//            MySqlParameter[] paras = GetMySqlParameters(fields, obj);
//            MySqlCommand cmd = new MySqlCommand(strCmd, _mConnection) {CommandType = cmdType};
//            if (this._mTransactionState)
//                cmd.Transaction = _mTransaction;
//            if (paras != null)
//            {
//                foreach (MySqlParameter para in paras)
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
