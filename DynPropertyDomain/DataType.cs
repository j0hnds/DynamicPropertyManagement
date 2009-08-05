
using System;
using System.Data;
using System.Collections.Generic;
using DomainCore;
using DAOCore;

namespace DynPropertyDomain
{
    
    namespace DAO
    {
        class DataTypeDAO : DAOBase
        {
            private static readonly Dictionary<string,string> ATTR_COL_MAPPINGS = new Dictionary<string, string>();

            static DataTypeDAO()
            {
                ATTR_COL_MAPPINGS["Id"] = "DYN_DATA_TYPE_ID";
                ATTR_COL_MAPPINGS["Name"] = "DESCRIPTION";
            }
            
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
    
    public class DataType : Domain
    {
        private const string ID_ATTR = "Id";
        private const string NAME_ATTR = "Name";
        
        public DataType(DomainDAO dao) :
            base(dao)
        {
            new LongAttribute(this, ID_ATTR, true);
            new StringAttribute(this, NAME_ATTR, false);
        }

        public long Id
        {
            get { return (long) GetValue(ID_ATTR); }
            set { SetValue(ID_ATTR, value); }
        }

        public string Name
        {
            get { return (string) GetValue(NAME_ATTR); }
            set { SetValue(NAME_ATTR, value); }
        }
    }
}
