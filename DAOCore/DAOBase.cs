
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data;
using MySql.Data.MySqlClient;
using DomainCore;
using log4net;

namespace DAOCore
{
    /// <summary>
    /// This class is responsible for providing data access objects for the
    /// dynamic property application.
    /// </summary>
    public class DAOBase : DomainDAO
    {
        /// <summary>
        /// The builder to construct SQL Insert statements.
        /// </summary>
        private InsertBuilder insertBuilder;
        /// <summary>
        /// The builder to construct SQL Update statements.
        /// </summary>
        private UpdateBuilder updateBuilder;
        /// <summary>
        /// The builder to construct SQL Delete statements.
        /// </summary>
        private DeleteBuilder deleteBuilder;
        /// <summary>
        /// If true, the ID of the domain object must be populated in
        /// inserts and updates.
        /// </summary>
        private bool populateId = true;
        /// <summary>
        /// The name of the domain object. Used by the domain factory when
        /// creating new instances.
        /// </summary>
        private string domainName;
        /// <summary>
        /// The domain attribute name DB column mappings.
        /// </summary>
        private Dictionary<string,string> mappings;
        /// <summary>
        /// The logger to use for this class.
        /// </summary>
        protected ILog log;

        /// <summary>
        /// Constructs a new DAOBase object.
        /// </summary>
        /// <param name="domainName">
        /// The name of the domain object. Used by the domain factory when
        /// creating new instances.
        /// </param>
        /// <param name="tableName">
        /// The name of the data base table.
        /// </param>
        /// <param name="mappings">
        /// The domain attribute DB column mappings.
        /// </param>
        public DAOBase(string domainName, 
                       string tableName, 
                       Dictionary<string,string> mappings)
        {
            log = LogManager.GetLogger(this.GetType().Name);
            this.domainName = domainName;
            this.mappings = mappings;
            insertBuilder = new InsertBuilder(tableName, mappings);
            updateBuilder = new UpdateBuilder(tableName, mappings);
            deleteBuilder = new DeleteBuilder(tableName, mappings);
        }

        /// <value>
        /// The data base connection.
        /// </value>
        protected IDbConnection Connection
        {
            get { return DataSource.Instance.Connection; }
        }

        /// <summary>
        /// Closes the current data base connection.
        /// </summary>
        protected void CloseConnection()
        {
            DataSource.Instance.Close();
        }

        /// <value>
        /// If <c>true</c>, the domain object's ID should be populated in
        /// insert and update statements.
        /// </value>
        protected virtual bool PopulateId
        {
            get { return populateId; }
            set { populateId = value; }
        }

        /// <summary>
        /// Populates the domain object with information from a SQL result set.
        /// </summary>
        /// <param name="reader">
        /// A reader on the SQL result set.
        /// </param>
        /// <returns>
        /// Reference to a newly created and populated domain object.
        /// </returns>
        protected virtual Domain PopulateDomain(IDataReader reader)
        {
            Domain domain = DomainFactory.Create(domainName, false);
            BaseAttribute.BeginPopulation();
            foreach (KeyValuePair<string,DomainCore.Attribute> kvp in domain.Attributes)
            {
                object val = reader[mappings[kvp.Key]];

                kvp.Value.Value = (val is DBNull) ? null : val;
            }
            BaseAttribute.EndPopulation();

            return domain;
        }

        /// <summary>
        /// Helper method to extract a long value from the data base result set.
        /// </summary>
        /// <param name="reader">
        /// The result set reader
        /// </param>
        /// <param name="index">
        /// The index of the result set to read.
        /// </param>
        /// <returns>
        /// The long value from the data base. If the data base value is null,
        /// returns -1L.
        /// </returns>
        protected long GetLong(IDataReader reader, int index)
        {
            long val = -1L;

            if (! reader.IsDBNull(index))
            {
                val = reader.GetInt64(index);
            }

            return val;
        }

        /// <summary>
        /// Helper method to extract a string from a data base result set.
        /// </summary>
        /// <param name="reader">
        /// A reader on the data base result set.
        /// </param>
        /// <param name="index">
        /// The index of the value to read from the result set.
        /// </param>
        /// <returns>
        /// The string value from the data base. If the data base value is null,
        /// then <c>null</c> is returned.
        /// </returns>
        protected string GetString(IDataReader reader, int index)
        {
            string val = null;

            if (! reader.IsDBNull(index))
            {
                val = reader.GetString(index);
            }

            return val;
        }

        /// <summary>
        /// Helper method to extract a DateTime value from a data base result set.
        /// </summary>
        /// <param name="reader">
        /// A reader on a data base result set.
        /// </param>
        /// <param name="index">
        /// The index of the column to read from the result set.
        /// </param>
        /// <returns>
        /// The DateTime value from the data base. If the data base value is null,
        /// DateTime.MinValue is returned.
        /// </returns>
        protected DateTime GetDateTime(IDataReader reader, int index)
        {
            DateTime val = DateTime.MinValue;

            if (! reader.IsDBNull(index))
            {
                val = reader.GetDateTime(index);
            }

            return val;
        }

        #region DomainDAO implementation
        public string DeleteSQL (Domain obj)
        {
            return deleteBuilder.Build(obj);
        }
        
        public string InsertSQL (Domain obj)
        {
            return insertBuilder.Build(obj);
        }
        
        public string UpdateSQL (Domain obj)
        {
            return updateBuilder.Build(obj);
        }
        public void Delete (Domain obj)
        {
            IDbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = DeleteSQL(obj);

            int numRows = cmd.ExecuteNonQuery();
            if (numRows <= 0)
            {
                throw new Exception("Should have deleted at least one row");
            }

            CloseConnection();
        }
        
        public void Insert (Domain obj)
        {
            IDbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = InsertSQL(obj);

            int numRows = cmd.ExecuteNonQuery();
            if (numRows <= 0)
            {
                throw new Exception("Should have inserted at least one row");
            }

            if (PopulateId)
            {
                // Now, get the ID of the newly inserted record
                cmd.CommandText = "SELECT last_insert_id()";
                object oid = cmd.ExecuteScalar();
                long id = Convert.ToInt64(oid);
                obj.IdAttribute.Value = id;
            }

            obj.Clean();
            obj.NewObject = false;

            CloseConnection();
        }
        
        public void Update (Domain obj)
        {
            IDbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = UpdateSQL(obj);

            int numRows = cmd.ExecuteNonQuery();
            if (numRows <= 0)
            {
                throw new Exception("Should have updated at least one row");
            }

            CloseConnection();
        }

        public virtual List<Domain> Get (params object[] argsRest)
        {
            throw new System.NotImplementedException();
        }

        public virtual Domain GetObject (object id)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        
        
    }
}
