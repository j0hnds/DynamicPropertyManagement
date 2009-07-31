
using System;
using System.Data;
using System.Collections.Generic;
using DomainCore;
using DAOCore;

namespace DynPropertyDomain
{
    
    
    namespace DAO
    {
        class PropertyDefinitionDAO : DAOBase
        {
            private static readonly Dictionary<string,string> ATTR_COL_MAPPINGS = new Dictionary<string, string>();

            static PropertyDefinitionDAO()
            {
                ATTR_COL_MAPPINGS["Id"] = "DYN_PROPERTY_ID";
                ATTR_COL_MAPPINGS["Category"] = "CATEGORY";
                ATTR_COL_MAPPINGS["Name"] = "NAME";
                ATTR_COL_MAPPINGS["DataType"] = "DATA_TYPE";
                ATTR_COL_MAPPINGS["Description"] = "DESCRIPTION";
            }
            
            public PropertyDefinitionDAO() : 
                base("PropertyDefinition", "DYN_PROPERTY", ATTR_COL_MAPPINGS)
            {
            }
            
            public override Domain GetObject (object id)
            {
                IDbCommand cmd = Connection.CreateCommand();
                cmd.CommandText = String.Format("SELECT * FROM DYN_PROPERTY WHERE DYN_PROPERTY_ID = {0}", DAOUtils.ConvertValue(id));

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
                cmd.CommandText = "SELECT * FROM DYN_PROPERTY ORDER BY CATEGORY, NAME";

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
    public class PropertyDefinition : Domain
    {
        
        public PropertyDefinition(DomainDAO dao) :
            base(dao)
        {
            new LongAttribute(this, "Id", true);
            new StringAttribute(this, "Category", false);
            new StringAttribute(this, "Name", false);
            new LongAttribute(this, "DataType", false);
            new StringAttribute(this, "Description", false);
        }
    }
}
