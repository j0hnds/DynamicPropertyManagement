
using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using DomainCore;

namespace DAOCore
{
    /// <summary>
    /// This class is responsible for providing data access objects for the
    /// dynamic property application.
    /// </summary>
    public class DAOBase : DomainDAO
    {
//        private string tableName;
//        private Dictionary<string,string> mappings;
        private InsertBuilder insertBuilder;
        private UpdateBuilder updateBuilder;
        private DeleteBuilder deleteBuilder;

        public DAOBase(string tableName, Dictionary<string,string> mappings)
        {
//            this.tableName = tableName;
//            this.mappings = mappings;

            insertBuilder = new InsertBuilder(tableName, mappings);
            updateBuilder = new UpdateBuilder(tableName, mappings);
            deleteBuilder = new DeleteBuilder(tableName, mappings);
        }

        private IDbConnection Connection
        {
            get { return DataSource.Instance.Connection; }
        }

        private void CloseConnection()
        {
            DataSource.Instance.Close();
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
        #endregion

        
        
    }
}
