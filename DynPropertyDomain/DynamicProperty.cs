
using System;
using System.Collections.Generic;
using DomainCore;
using DAOCore;

namespace DynPropertyDomain
{
    
    
    namespace DAO
    {
        class DynamicPropertyDAO : DAOBase
        {
            private static readonly Dictionary<string,string> ATTR_COL_MAPPINGS = new Dictionary<string, string>();

            static DynamicPropertyDAO()
            {
                ATTR_COL_MAPPINGS["Id"] = "DYN_VALUE_ID";
                ATTR_COL_MAPPINGS["ApplicationId"] = "APPLICATION_ID";
                ATTR_COL_MAPPINGS["PropertyId"] = "DYN_PROPERTY_ID";
                ATTR_COL_MAPPINGS["Qualifier"] = "QUALIFIER";
                ATTR_COL_MAPPINGS["DefaultValue"] = "DFLT_VALUE";
            }
            
            public DynamicPropertyDAO() : 
                base("DYN_ASSIGN", ATTR_COL_MAPPINGS)
            {
            }
        }
    }
    
    public class DynamicProperty : Domain
    {
        
        public DynamicProperty(DomainDAO dao) :
            base(dao)
        {
            new LongAttribute(this, "Id", true);
            new LongAttribute(this, "ApplicationId", false);
            new StringAttribute(this, "ApplicationName", false);
            new LongAttribute(this, "PropertyId", false);
            new StringAttribute(this, "Category", false);
            new StringAttribute(this, "PropertyName", false);
            new StringAttribute(this, "DefaultValue", false);
            new LongAttribute(this, "PropertyType", false);
            new StringAttribute(this, "Qualifier", false);
            new CollectionRelationship(this, "EffectiveValues", "EffectiveValue", "AssignId");
        }

        public object GetEffectiveValue()
        {
            return GetEffectiveValue(DateTime.Now);
        }
        public object GetEffectiveValue(DateTime dateTime)
        {
            object defaultValue = GetValue("DefaultValue");
            object val = defaultValue;

            foreach (Domain domain in GetCollection("EffectiveValues"))
            {
                EffectiveValue ev = (EffectiveValue) domain;
                if (ev.IsEffectiveAt(dateTime))
                {
                    val = ev.GetEffectiveValue(dateTime, defaultValue);
                    break;
                }
            }

            return val;
        }

        public string QualifiedName
        {
            get
            {
                string propName = (string) GetValue("PropertyName");
                string qualifier = (string) GetValue("Qualifier");

                if (qualifier != null)
                {
                    if (propName != null)
                    {
                        propName += qualifier;
                    }
                    else
                    {
                        propName = qualifier;
                    }
                }

                return propName;
            }
        }
    }
}
