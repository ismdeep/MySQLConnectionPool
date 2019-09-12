using System;
using System.Linq;
using System.Data.SqlClient;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ConnectionPool.Core
{
    /// <summary>
    /// ***********************************
    /// * Del Cooper  2019-9-12 09:45:18  *
    /// * 数据库连接池代理，对外提供方法      *
    /// ***********************************
    /// </summary>
    public class ConnectionPoolProxy
    {
        #region property

        static ConnectionPool connectionPool = null;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        static ConnectionPoolProxy()
        {
            connectionPool = ConnectionPool.getInstance();
        }

        #endregion

        #region 获取数据库连接池连接

        /// <summary>
        /// 获取数据库连接池连接
        /// </summary>
        /// <returns></returns>
        public static MySqlConnection getConnecion()
        {
            MySqlConnection sqlConnection = null;

            try
            {
                sqlConnection = connectionPool.getConnecion();
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return sqlConnection;
        }

        #endregion

        #region 关闭连接池连接

        /// <summary>
        /// 关闭连接池连接
        /// </summary>
        /// <returns></returns>
        public static void closeConnecion(MySqlConnection conn)
        {
            try
            {
                Action<MySqlConnection> action = connectionPool.closeConnection;

                action.BeginInvoke(conn,(ar)=>
                {
                   action.EndInvoke(ar);
                },
                null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 重置数据库连接池

        /// <summary>
        /// 重置数据库连接池
        /// </summary>
        /// <returns></returns>
        public static void resetConnecion()
        {
            Task.Factory.StartNew(() =>
            {
                connectionPool.clearConnection();
                connectionPool.initConnection();
            });
        }

        #endregion
    }
}
