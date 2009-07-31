
using System;
using System.Data;
using System.Collections.Generic;
using DomainCore;
using CronUtils;
using DAOCore;

namespace DynPropertyDomain
{
    namespace DAO
    {
        class ValueCriteriaDAO : DAOBase
        {
            private static readonly Dictionary<string,string> ATTR_COL_MAPPINGS = new Dictionary<string, string>();

            static ValueCriteriaDAO()
            {
                ATTR_COL_MAPPINGS["Id"] = "DYN_VALUE_ID";
                ATTR_COL_MAPPINGS["EffectiveId"] = "DYN_EFFECTIVE_ID";
                ATTR_COL_MAPPINGS["RawCriteria"] = "CRITERIA";
                ATTR_COL_MAPPINGS["Value"] = "PROP_VALUE";
            }
            
            public ValueCriteriaDAO() : 
                base("ValueCriteria", "DYN_VALUE", ATTR_COL_MAPPINGS)
            {
            }
            public override Domain GetObject (object id)
            {
                IDbCommand cmd = Connection.CreateCommand();
                cmd.CommandText = String.Format("SELECT * FROM DYN_VALUE WHERE DYN_VALUE_ID = {0}", DAOUtils.ConvertValue(id));

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
                cmd.CommandText = "SELECT * FROM DYN_VALUE";

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
    
    public class ValueCriteria : Domain
    {
        
        public ValueCriteria(DomainDAO dao) : 
            base(dao)
        {
            new LongAttribute(this, "Id", true);
            new LongAttribute(this, "EffectiveId", false);
            new StringAttribute(this, "Value", false);
            new StringAttribute(this, "RawCriteria", false);
        }

        public CronSpecification CronSpec
        {
            get
            {
                CronSpecification cronSpec = null;

                if (! GetAttribute("RawCriteria").Empty)
                {
                    cronSpec = new CronSpecification((string) GetValue("RawCriteria"));
                }

                return cronSpec;
            }
        }

        public bool IsEffectiveAt(DateTime dateTime)
        {
            bool effective = true;

            CronSpecification cronSpec = CronSpec;
            if (cronSpec != null)
            {
                effective = cronSpec.IsDateEffective(dateTime);
            }

            return effective;
        }
    }
}
