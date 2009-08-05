
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
        private const string ID_ATTR = "Id";
        private const string ASSIGNID_ATTR = "AssignId";
        private const string EFFECTIVESTARTDATE_ATTR = "EffectiveStartDate";
        private const string EFFECTIVEENDDATE_ATTR = "EffectiveEndDate";
        private const string VALUECRITERIA_REL = "ValueCriteria";
        
        public EffectiveValue(DomainDAO dao) :
            base(dao)
        {
            new LongAttribute(this, ID_ATTR, true);
            new LongAttribute(this, ASSIGNID_ATTR, false);
            new DateTimeAttribute(this, EFFECTIVESTARTDATE_ATTR);
            new DateTimeAttribute(this, EFFECTIVEENDDATE_ATTR);
            new CollectionRelationship(this, VALUECRITERIA_REL, "ValueCriteria", "EffectiveId");
        }

        public long Id
        {
            get { return (long) GetValue(ID_ATTR); }
            set { SetValue(ID_ATTR, value); }
        }

        public long AssignId
        {
            get { return (long) GetValue(ASSIGNID_ATTR); }
            set { SetValue(ASSIGNID_ATTR, value); }
        }

        public DateTime EffectiveStartDate
        {
            get { return (DateTime) GetValue(EFFECTIVESTARTDATE_ATTR); }
            set { SetValue(EFFECTIVESTARTDATE_ATTR, value); }
        }

        public DateTime EffectiveEndDate
        {
            get { return (DateTime) GetValue(EFFECTIVEENDDATE_ATTR); }
            set { SetValue(EFFECTIVEENDDATE_ATTR, value); }
        }

        public List<Domain> ValueCriteria
        {
            get { return GetCollection(VALUECRITERIA_REL); }
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
