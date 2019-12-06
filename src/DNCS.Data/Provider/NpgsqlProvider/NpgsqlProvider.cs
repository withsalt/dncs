using DNCS.Config;
using Npgsql;
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace DNCS.Data.Provider.NpgsqlProvider
{
    public class NpgsqlProvider
    {
        //从配置文件读取连接字符串
        private string ConnectString
        {
            get
            {
                return ConfigManager.Now.ConnectionStrings.NpgsqlDbConnectionString;
            }
        }

        // 用于缓存参数的HASH表
        private Hashtable ParmCache
        {
            get
            {
                return Hashtable.Synchronized(new Hashtable());
            }
        }

        /// <summary>
        ///  给定连接的数据库用假设参数执行一个sql命令，返回影响的行数
        /// </summary>
        /// <param name="cmdText">存储过程名称或者sql命令语句</param>
        /// <param name="commandParameters">执行命令所用参数的集合</param>
        /// <returns>执行命令所影响的行数</returns>
        public async Task<int> ExecuteNonQuery(string cmdText, params NpgsqlParameter[] commandParameters)
        {
            return await ExecuteNonQuery(ConnectString, CommandType.Text, cmdText, commandParameters);
        }

        /// <summary>
        ///  给定连接的数据库用假设参数执行一个sql命令（不返回数据集）
        /// </summary>
        /// <param name="connectionString">一个有效的连接字符串</param>
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param>
        /// <param name="cmdText">存储过程名称或者sql命令语句</param>
        /// <param name="commandParameters">执行命令所用参数的集合</param>
        /// <returns>执行命令所影响的行数</returns>
        public async Task<int> ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params NpgsqlParameter[] commandParameters)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                    int val = await cmd.ExecuteNonQueryAsync();
                    cmd.Parameters.Clear();
                    return val;
                }
            }
        }

        /// <summary>
        /// 用现有的数据库连接执行一个sql命令（不返回数据集）
        /// </summary>
        /// <param name="connection">一个现有的数据库连接</param>
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param>
        /// <param name="cmdText">存储过程名称或者sql命令语句</param>
        /// <param name="commandParameters">执行命令所用参数的集合</param>
        /// <returns>执行命令所影响的行数</returns>
        public async Task<int> ExecuteNonQuery(NpgsqlConnection connection, CommandType cmdType, string cmdText, params NpgsqlParameter[] commandParameters)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                int val = await cmd.ExecuteNonQueryAsync();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        ///使用现有的SQL事务执行一个sql命令（不返回数据集）
        /// </summary>
        /// <remarks>
        ///举例:
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new NpgsqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="trans">一个现有的事务</param>
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param>
        /// <param name="cmdText">存储过程名称或者sql命令语句</param>
        /// <param name="commandParameters">执行命令所用参数的集合</param>
        /// <returns>执行命令所影响的行数</returns>
        public async Task<int> ExecuteNonQuery(NpgsqlTransaction trans, CommandType cmdType, string cmdText, params NpgsqlParameter[] commandParameters)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
                int val = await cmd.ExecuteNonQueryAsync();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// 用执行的数据库连接执行一个返回数据集的sql命令
        /// </summary>
        /// <remarks>
        /// 举例:
        ///  NpgsqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new NpgsqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">一个有效的连接字符串</param>
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param>
        /// <param name="cmdText">存储过程名称或者sql命令语句</param>
        /// <param name="commandParameters">执行命令所用参数的集合</param>
        /// <returns>包含结果的读取器</returns>
        public async Task<DbDataReader> ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params NpgsqlParameter[] commandParameters)
        {
            //创建一个NpgsqlCommand对象
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                //创建一个NpgsqlConnection对象
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    //在这里我们用一个try/catch结构执行sql文本命令/存储过程，因为如果这个方法产生一个异常我们要关闭连接，因为没有读取器存在，
                    //因此commandBehaviour.CloseConnection 就不会执行
                    try
                    {
                        //调用 PrepareCommand 方法，对 NpgsqlCommand 对象设置参数
                        PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                        //调用 NpgsqlCommand  的 ExecuteReader 方法
                        DbDataReader reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                        //清除参数
                        cmd.Parameters.Clear();
                        return reader;
                    }
                    catch
                    {
                        //关闭连接，抛出异常
                        conn.Close();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 返回DataSet
        /// </summary>
        /// <param name="connectionString">一个有效的连接字符串</param>
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param>
        /// <param name="cmdText">存储过程名称或者sql命令语句</param>
        /// <param name="commandParameters">执行命令所用参数的集合</param>
        /// <returns></returns>
        public DataSet GetDataSet(string connectionString, CommandType cmdType, string cmdText, params NpgsqlParameter[] commandParameters)
        {
            //创建一个NpgsqlCommand对象
            using NpgsqlCommand cmd = new NpgsqlCommand();
            //创建一个NpgsqlConnection对象
            using NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            //在这里我们用一个try/catch结构执行sql文本命令/存储过程，因为如果这个方法产生一个异常我们要关闭连接，因为没有读取器存在，
            try
            {
                //调用 PrepareCommand 方法，对 NpgsqlCommand 对象设置参数
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                //调用 NpgsqlCommand  的 ExecuteReader 方法
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                //清除参数
                cmd.Parameters.Clear();
                conn.Close();
                return ds;
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        /// <summary>
        /// 用指定的数据库连接字符串执行一个命令并返回一个数据集的第一列
        /// </summary>
        /// <remarks>
        ///例如:
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new NpgsqlParameter("@prodid", 24));
        /// </remarks>
        ///<param name="connectionString">一个有效的连接字符串</param>
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param>
        /// <param name="cmdText">存储过程名称或者sql命令语句</param>
        /// <param name="commandParameters">执行命令所用参数的集合</param>
        /// <returns>用 Convert.To{Type}把类型转换为想要的 </returns>
        public async Task<object> ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params NpgsqlParameter[] commandParameters)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                    object val = await cmd.ExecuteScalarAsync();
                    cmd.Parameters.Clear();
                    return val;
                }
            }
        }

        /// <summary>
        /// 用指定的数据库连接执行一个命令并返回一个数据集的第一列
        /// </summary>
        /// <remarks>
        /// 例如:
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new NpgsqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">一个存在的数据库连接</param>
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param>
        /// <param name="cmdText">存储过程名称或者sql命令语句</param>
        /// <param name="commandParameters">执行命令所用参数的集合</param>
        /// <returns>用 Convert.To{Type}把类型转换为想要的 </returns>
        public async Task<object> ExecuteScalar(NpgsqlConnection connection, CommandType cmdType, string cmdText, params NpgsqlParameter[] commandParameters)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                object val = await cmd.ExecuteScalarAsync();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// 将参数集合添加到缓存
        /// </summary>
        /// <param name="cacheKey">添加到缓存的变量</param>
        /// <param name="commandParameters">一个将要添加到缓存的sql参数集合</param>
        public void CacheParameters(string cacheKey, params NpgsqlParameter[] commandParameters)
        {
            ParmCache[cacheKey] = commandParameters;
        }

        /// <summary>
        /// 找回缓存参数集合
        /// </summary>
        /// <param name="cacheKey">用于找回参数的关键字</param>
        /// <returns>缓存的参数集合</returns>
        public NpgsqlParameter[] GetCachedParameters(string cacheKey)
        {
            NpgsqlParameter[] cachedParms = (NpgsqlParameter[])ParmCache[cacheKey];
            if (cachedParms == null)
            {
                return null;
            }
            NpgsqlParameter[] clonedParms = new NpgsqlParameter[cachedParms.Length];
            for (int i = 0, j = cachedParms.Length; i < j; i++)
            {
                clonedParms[i] = (NpgsqlParameter)((ICloneable)cachedParms[i]).Clone();
            }
            return clonedParms;
        }

        /// <summary>
        /// 准备执行一个命令
        /// </summary>
        /// <param name="cmd">sql命令</param>
        /// <param name="conn">OleDb连接</param>
        /// <param name="trans">OleDb事务</param>
        /// <param name="cmdType">命令类型例如 存储过程或者文本</param>
        /// <param name="cmdText">命令文本,例如:Select * from Products</param>
        /// <param name="cmdParms">执行命令的参数</param>
        private void PrepareCommand(NpgsqlCommand cmd, NpgsqlConnection conn, NpgsqlTransaction trans, CommandType cmdType, string cmdText, NpgsqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
            {
                cmd.Transaction = trans;
            }
            cmd.CommandType = cmdType;
            if (cmdParms != null)
            {
                foreach (NpgsqlParameter parm in cmdParms)
                {
                    cmd.Parameters.Add(parm);
                }
            }
        }
    }
}
