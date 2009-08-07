
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
        private InsertBuilder insertBuilder;
        private UpdateBuilder updateBuilder;
        private DeleteBuilder deleteBuilder;
        private bool populateId = true;
        private string domainName;
        private Dictionary<string,string> mappings;
        protected ILog log;

        public DAOBase(string domainName, string tableName, Dictionary<string,string> mappings)
        {
            log = LogManager.GetLogger(this.GetType().Name);
            this.domainName = domainName;
            this.mappings = mappings;
            insertBuilder = new InsertBuilder(tableName, mappings);
            updateBuilder = new UpdateBuilder(tableName, mappings);
            deleteBuilder = new DeleteBuilder(tableName, mappings);
        }

        protected IDbConnection Connection
        {
            get { return DataSource.Instance.Connection; }
        }

        protected void CloseConnection()
        {
            DataSource.Instance.Close();
        }

        protected virtual bool PopulateId
        {
            get { return populateId; }
            set { populateId = value; }
        }

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

        protected long GetLong(IDataReader reader, int index)
        {
            long val = -1L;

            if (! reader.IsDBNull(index))
            {
                val = reader.GetInt64(index);
            }

            return val;
        }

        protected string GetString(IDataReader reader, int index)
        {
            string val = null;

            if (! reader.IsDBNull(index))
            {
                val = reader.GetString(index);
            }

            return val;
        }

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
            Console.Out.WriteLine("Starting the Insert");
            IDbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = InsertSQL(obj);

            int numRows = cmd.ExecuteNonQuery();
            if (numRows <= 0)
            {
                throw new Exception("Should have inserted at least one row");
            }

            if (PopulateId)
            {
                Console.Out.WriteLine("Populating ID");
                // Now, get the ID of the newly inserted record
                cmd.CommandText = "SELECT last_insert_id()";
                object oid = cmd.ExecuteScalar();
                long id = Convert.ToInt64(oid);
                Console.Out.WriteLine(String.Format("ID assigned to domain = {0}", oid));
                obj.IdAttribute.Value = id;
                Console.Out.WriteLine("Done Populating ID");
            }

            obj.Clean();
            obj.NewObject = false;

            CloseConnection();
            Console.Out.WriteLine("Done with Insert");
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
