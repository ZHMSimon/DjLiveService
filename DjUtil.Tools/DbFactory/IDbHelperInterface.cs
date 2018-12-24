using System.Data;
using System.Data.Common;

namespace DjUtil.Tools.DbFactory
{
    public interface IdbHelperInterface
    {
        /// <summary>
        /// 打开数据库连接器
        /// </summary>
        void OpenConnection();
        /// <summary>
        /// 关闭数据库连接器
        /// </summary>
        void CloseConnection();
        /// <summary>
        /// 开始事务
        /// </summary>
        void BeginTran();
        /// <summary>
        /// 提交事务
        /// </summary>
        void CommitTran();
        /// <summary>
        /// 回滚事务
        /// </summary>
        void RollBackTran();
        /// <summary>
        /// 执行一个sql语句
        /// </summary>
        /// <param name="strCmd">sql语句或存储过程名</param>
        /// <returns>返回影响行数</returns>
        int ExecuteNoneQuery(string strCmd, int cmdTimeOut = 30);

        /// <summary>
        /// 执行一个sql语句
        /// </summary>
        /// <param name="strCmd">sql语句或存储过程名</param>
        /// <param name="cmdType">存储过程类型</param>
        /// <param name="fields">参数名称</param>
        /// <param name="obj">值集合</param>
        /// <returns>返回影响行数</returns>
        int ExecuteNoneQuery(string strCmd, CommandType cmdType, string[] fields, object[] obj, int cmdTimeOut = 30);

        /// <summary>
        /// 执行查询，返回查询的第一行第一列，忽略其他列
        /// </summary>
        /// <param name="strCmd">sql语句或存储过程名</param>
        /// <param name="parameters">sql语句或存储过程名</param>
        /// <param name="cmdTimeOut">sql语句或存储过程名</param>
        /// <returns>返回一个obj对象</returns>
        object ExecuteScalar(string strCmd, DbParameter[] parameters = null, int cmdTimeOut = 30);


        /// <summary>
        /// 执行查询，返回查询的第一行第一列，忽略其他列
        /// </summary>
        /// <param name="strCmd">sql语句或存储过程名</param>
        /// <param name="cmdType">存储过程类型</param>
        /// <param name="fields">参数集合</param>
        /// <param name="obj">值集合</param>
        /// <returns>返回一个obj对象</returns>
        object ExecuteScalar(string strCmd, CommandType cmdType, string[] fields, object[] obj, int cmdTimeOut = 30);


        /// <summary>
        /// 执行查询，返回一个数据表
        /// </summary>
        /// <param name="strCmd">sql语句或存储过程名</param>
        /// <returns>返回一个数据表</returns>
        DataTable GetTable(string strCmd, int cmdTimeOut = 30);
        DataTable Gettable(string strCmd, int cmdTimeOut = 30);

        /// <summary>
        /// 执行查询，返回一个数据表
        /// </summary>
        /// <param name="strCmd">sql语句或存储过程名</param>
        /// <param name="cmdType">存储过程类型</param>
        /// <param name="fields">参数集合</param>
        /// <param name="obj">值集合</param>
        /// <returns>返回一个数据表</returns>
        DataTable GetTable(string strCmd, CommandType cmdType, string[] fields, object[] obj, int cmdTimeOut = 30);

        /// <summary>
        /// 执行查询，返回一个数据集
        /// </summary>
        /// <param name="strCmd">sql语句或存储过程名</param>
        /// <returns>返回一个数据集</returns>
        DataSet GetSet(string strCmd, int cmdTimeOut = 30);

        /// <summary>
        /// 执行查询，返回一个数据集
        /// </summary>
        /// <param name="strCmd">sql语句或存储过程名</param>
        /// <param name="cmdType">存储过程类型</param>
        /// <param name="fields">参数集合</param>
        /// <param name="obj">值集合</param>
        /// <returns>返回一个数据集</returns>
        DataSet GetSet(string strCmd, CommandType cmdType, string[] fields, object[] obj, int cmdTimeOut = 30);

        /// <summary>
        /// 批量导入数据库(注意导入的时候dt中的结构要和目标表结构完全一致才行,自增长除外)
        /// </summary>
        /// <param name="tableName">服务器上目标表的名称</param>
        /// <param name="dt">要导入的数据集</param>
        int BulkInDataServer(string tableName, DataTable dt);
        int ExecuteNoneQuery(string sql, DbParameter[] parameters=null, int cmdTimeOut = 30);
        DbDataReader GetDataReader(string sql, DbParameter[] parameters = null, int cmdTimeOut = 30);
        DataTable GetTable(string sql, DbParameter[] parameters = null, int cmdTimeOut = 30);
    }
}
