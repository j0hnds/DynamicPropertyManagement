
using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace DAOCore
{
    
    /// <summary>
    /// This class represents a very simple abstraction of a connection to a
    /// relational data base.
    /// </summary>
    public class DataSource
    {
        /// <value>
        /// Constant connection string template. Populate using <c>string.Format</c> and
        /// the appropriate values.
        /// </value>
        private const string CONNECTION_TEMPLATE = "Server={0};Database={1};User ID={2};Password={3};Pooling=false";

        /// <value>
        /// The one-and-only instance of this class.
        /// </value>
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

        /// <value>
        /// The one-and-only instance of this class.
        /// </value>
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

        /// <summary>
        /// Constructs a new DataSource object.
        /// </summary>
        private DataSource()
        {
        }

        /// <value>
        /// The host of the relational data base system.
        /// </value>
        public string Host
        {
            get { return host; }
            set { host = value; }
        }

        /// <value>
        /// The name of the relational data base.
        /// </value>
        public string DBName
        {
            get { return dbName; }
            set { dbName = value; }
        }

        /// <value>
        /// The user id with which we will log onto the data base.
        /// </value>
        public string UserID
        {
            get { return userId; }
            set { userId = value; }
        }

        /// <value>
        /// The password with which we will log onto the data base.
        /// </value>
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        /// <summary>
        /// Makes a trial connection to the data base and immediately
        /// closes the connection.
        /// </summary>
        /// <remarks>
        /// Typically used to verify the authentication credentials. Throws an exception
        /// if the credentials are not correct or if the connection cannot be made for
        /// some other reason.
        /// </remarks>
        public void TestConnection()
        {
            try
            {
                IDbConnection conn = Connection;
                conn.ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                Close();
            }
        }

        /// <value>
        /// A connection to the database.
        /// </value>
        /// <remarks>
        /// If a connection has already been established, returns the same connection
        /// as earlier created. This will work until the <c>Close</c> method is called.
        /// </remarks>
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

        /// <summary>
        /// Closes any currently open connection to the data base.
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
