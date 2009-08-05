
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
        private const string ID_ATTR = "Id";
        private const string EFFECTIVEID_ATTR = "EffectiveId";
        private const string VALUE_ATTR = "Value";
        private const string RAWCRITERIA_ATTR = "RawCriteria";
        
        public ValueCriteria(DomainDAO dao) : 
            base(dao)
        {
            new LongAttribute(this, ID_ATTR, true);
            new LongAttribute(this, EFFECTIVEID_ATTR, false);
            new StringAttribute(this, VALUE_ATTR, false);
            new StringAttribute(this, RAWCRITERIA_ATTR, false);
        }

        public long Id
        {
            get { return (long) GetValue(ID_ATTR); }
            set { SetValue(ID_ATTR, value); }
        }

        public long EffectiveId
        {
            get { return (long) GetValue(EFFECTIVEID_ATTR); }
            set { SetValue(EFFECTIVEID_ATTR, value); }
        }

        public string Value
        {
            get { return (string) GetValue(VALUE_ATTR); }
            set { SetValue(VALUE_ATTR, value); }
        }

        public string RawCriteria
        {
            get { return (string) GetValue(RAWCRITERIA_ATTR); }
            set { SetValue(RAWCRITERIA_ATTR, value); }
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
