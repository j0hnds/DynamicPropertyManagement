
using System;
using System.Data;
using System.Collections.Generic;
using DomainCore;
using DAOCore;

namespace DynPropertyDomain
{
    
    namespace DAO
    {
        /// <summary>
        /// The DAO for DataType domain objects.
        /// </summary>
        class DataTypeDAO : DAOBase
        {
            /// <summary>
            /// The attribute name column name mappings.
            /// </summary>
            private static readonly Dictionary<string,string> ATTR_COL_MAPPINGS = new Dictionary<string, string>();

            /// <summary>
            /// Static constructor to initialize column mappings.
            /// </summary>
            static DataTypeDAO()
            {
                ATTR_COL_MAPPINGS["Id"] = "DYN_DATA_TYPE_ID";
                ATTR_COL_MAPPINGS["Name"] = "DESCRIPTION";
            }

            /// <summary>
            /// Constructs a new DataTypeDAO object.
            /// </summary>
            public DataTypeDAO() : 
                base("DataType", "DYN_DATA_TYPE", ATTR_COL_MAPPINGS)
            {
                PopulateId = false;
            }

            public override Domain GetObject (object id)
            {
                IDbCommand cmd = Connection.CreateCommand();
                cmd.CommandText = String.Format("SELECT * FROM DYN_DATA_TYPE WHERE DYN_DATA_TYPE_ID = {0} ORDER BY DESCRIPTION", DAOUtils.ConvertValue(id));

                IDataReader reader = cmd.ExecuteReader();
                if (! reader.Read())
                {
                    throw new Exception("Unable to find specified data type");
                }

                Domain domain = PopulateDomain(reader);

                reader.Close();
                CloseConnection();

                return domain;
            }
            
            public override List<Domain> Get (params object[] argsRest)
            {
                List<Domain> dataTypes = new List<Domain>();
                
                IDbCommand cmd = Connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM DYN_DATA_TYPE ORDER BY DESCRIPTION";

                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Domain domain = PopulateDomain(reader);

                    dataTypes.Add(domain);
                }

                reader.Close();
                CloseConnection();

                return dataTypes;
            }
        }
    }

    /// <summary>
    /// This class represents a DataType domain object.
    /// </summary>
    public class DataType : Domain
    {
        /// <summary>
        /// Name of the Id attribute.
        /// </summary>
        private const string ID_ATTR = "Id";
        /// <summary>
        /// Name of the Name attribute.
        /// </summary>
        private const string NAME_ATTR = "Name";

        /// <summary>
        /// Constructs a new DataType object.
        /// </summary>
        /// <param name="dao">
        /// The DAO object associated with the DataType object.
        /// </param>
        public DataType(DomainDAO dao) :
            base(dao)
        {
            new LongAttribute(this, ID_ATTR, true);
            new StringAttribute(this, NAME_ATTR, false);
        }

        /// <value>
        /// The unique identifier for the data type
        /// </value>
        public long Id
        {
            get { return (long) GetValue(ID_ATTR); }
            set { SetValue(ID_ATTR, value); }
        }

        /// <value>
        /// The Name of the data type.
        /// </value>
        public string Name
        {
            get { return (string) GetValue(NAME_ATTR); }
            set { SetValue(NAME_ATTR, value); }
        }
    }
}
