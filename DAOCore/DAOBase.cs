
using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace DAOCore
{
    /// <summary>
    /// This class is responsible for providing data access objects for the
    /// dynamic property application.
    /// </summary>
    public class DAOBase
    {
        private const string CONNECTION_TEMPLATE = "Server={0};Database={1};User ID={2};Password={3};Pooling=false";

        // The current DB connection
        private IDbConnection conn;

        // The host name of the DB server
        private string host;

        // The name of the data base
        private string dbName;

        // The userID of the data base
        private string userId;

        // The password of the user
        private string password;
        
        public DAOBase()
        {
        }
        
        /// <summary>
        /// Returns the connection associated with this data access object. Acts
        /// as a factory method in that - until the connection is closed - it will
        /// always return the same connection.
        /// </summary>
        /// <param name="host">
        /// The name of the host for the data base.
        /// </param>
        /// <param name="dbName">
        /// The name of the data base on the host.
        /// </param>
        /// <param name="userId">
        /// A valid user ID for the data base.
        /// </param>
        /// <param name="password">
        /// The password for the valid user id.
        /// </param>
        /// <returns>
        /// A connection to the data base.
        /// </returns>
        public IDbConnection GetConnection(string host, string dbName, string userId, string password)
        {
            this.host = host;
            this.dbName = dbName;
            this.userId = userId;
            this.password = password;

            return GetConnection();
        }

        /// <summary>
        /// Returns a connection to the data base. Uses the values from previous call to 
        /// GetConnection(host, dbName, userId, password).
        /// </summary>
        /// <returns>
        /// A connection to the data base.
        /// </returns>
        public IDbConnection GetConnection()
        {
            if (host == null || dbName == null || userId == null || password == null)
            {
                throw new InvalidOperationException("Connection string parameters have not been set.");
            }
            
            if (conn == null)
            {
                string connectionString = String.Format(CONNECTION_TEMPLATE, host, dbName, userId, password);
    
                conn = new MySqlConnection(connectionString);
    
                conn.Open();
            }
            return conn;
        }

        /// <summary>
        /// Closes the currently open connection (if any) and nulls out the member
        /// connection.
        /// </summary>
        public void Close()
        {
            if (conn != null)
            {
                conn.Close();
                conn = null;
            }
        }
        
    }
}
