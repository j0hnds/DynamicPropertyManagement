
using System;
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
                base("DYN_PROPERTY", ATTR_COL_MAPPINGS)
            {
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
