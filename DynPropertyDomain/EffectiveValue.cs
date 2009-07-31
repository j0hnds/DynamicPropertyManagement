
using System;
using System.Data;
using System.Collections.Generic;
using DomainCore;
using DAOCore;

namespace DynPropertyDomain
{
    
    
    namespace DAO
    {
        class EffectiveValueDAO : DAOBase
        {
            private static readonly Dictionary<string,string> ATTR_COL_MAPPINGS = new Dictionary<string, string>();

            static EffectiveValueDAO()
            {
                ATTR_COL_MAPPINGS["Id"] = "DYN_EFFECTIVE_ID";
                ATTR_COL_MAPPINGS["AssignId"] = "DYN_ASSIGN_ID";
                ATTR_COL_MAPPINGS["EffectiveStartDate"] = "EFF_START_DT";
                ATTR_COL_MAPPINGS["EffectiveEndDate"] = "EFF_END_DT";
            }
            
            public EffectiveValueDAO() : 
                base("EffectiveValue", "DYN_EFFECTIVE", ATTR_COL_MAPPINGS)
            {
            }
            public override Domain GetObject (object id)
            {
                IDbCommand cmd = Connection.CreateCommand();
                cmd.CommandText = String.Format("SELECT * FROM DYN_EFFECTIVE WHERE DYN_EFFECTIVE_ID = {0}", DAOUtils.ConvertValue(id));

                IDataReader reader = cmd.ExecuteReader();
                if (! reader.Read())
                {
                    throw new Exception("Unable to find specified effective value");
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
                cmd.CommandText = "SELECT * FROM DYN_EFFECTIVE";

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
    
    public class EffectiveValue : Domain
    {
        
        public EffectiveValue(DomainDAO dao) :
            base(dao)
        {
            new LongAttribute(this, "Id", true);
            new LongAttribute(this, "AssignId", false);
            new DateTimeAttribute(this, "EffectiveStartDate");
            new DateTimeAttribute(this, "EffectiveEndDate");
            new CollectionRelationship(this, "ValueCriteria", "ValueCriteria", "EffectiveId");
        }

        public bool IsEffectiveAt(DateTime dateTime)
        {
            DateTime startDate = (DateTime) GetValue("EffectiveStartDate");
            DateTime endDate = (DateTime) GetValue("EffectiveEndDate");

            bool lowEndValid = startDate <= dateTime;

            bool highEndValid = false;
            if (endDate == DateTime.MinValue)
            {
                highEndValid = true;
            }
            else
            {
                highEndValid = endDate >= dateTime;
            }

            return lowEndValid && highEndValid;
        }

        public object GetEffectiveValue(DateTime dateTime)
        {
            return GetEffectiveValue(dateTime, null);
        }

        public object GetEffectiveValue(DateTime dateTime, object defaultValue)
        {
            object effectiveValue = defaultValue;

            foreach (Domain domain in GetCollection("ValueCriteria"))
            {
                if (((ValueCriteria) domain).IsEffectiveAt(dateTime))
                {
                    effectiveValue = domain.GetValue("Value");
                    break;
                }
            }
            
            return effectiveValue;
        }
    }
}
