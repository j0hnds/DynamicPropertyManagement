
using System;
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
                ATTR_COL_MAPPINGS["Id"] = "DYN_VALUE_ID";
                ATTR_COL_MAPPINGS["EffectiveId"] = "DYN_EFFECTIVE_ID";
                ATTR_COL_MAPPINGS["RawCriteria"] = "CRITERIA";
                ATTR_COL_MAPPINGS["Value"] = "PROP_VALUE";
            }
            
            public EffectiveValueDAO() : 
                base("DYN_VALUE", ATTR_COL_MAPPINGS)
            {
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
