
using System;
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
                base("DYN_VALUE", ATTR_COL_MAPPINGS)
            {
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
