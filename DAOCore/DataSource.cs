
using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace DAOCore
{
    
    
    public class DataSource
    {
        
        private const string CONNECTION_TEMPLATE = "Server={0};Database={1};User ID={2};Password={3};Pooling=false";

        private static DataSource instance = null;

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

        public static DataSource Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataSource();
                }
                return instance;
            }
        }
        
        private DataSource()
        {
        }

        public string Host
        {
            get { return host; }
            set { host = value; }
        }

        public string DBName
        {
            get { return dbName; }
            set { dbName = value; }
        }

        public string UserID
        {
            get { return userId; }
            set { userId = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public IDbConnection Connection
        {
            get
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
        }

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
