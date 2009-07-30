
using System;
using System.Collections.Generic;
using DomainCore;
using DAOCore;

namespace DynPropertyDomain
{
    
    
    namespace DAO
    {
        class ApplicationDAO : DAOBase
        {
            private static readonly Dictionary<string,string> ATTR_COL_MAPPINGS = new Dictionary<string, string>();

            static ApplicationDAO()
            {
                ATTR_COL_MAPPINGS["Id"] = "DYN_APPLICATION_ID";
                ATTR_COL_MAPPINGS["Name"] = "APPLICATION_NAME";
            }
            
            public ApplicationDAO() : 
                base("DYN_APPLICATION", ATTR_COL_MAPPINGS)
            {
            }
        }
    }
    public class Application : Domain
    {
        
        public Application(DomainDAO dao) : 
            base(dao)
        {
            new LongAttribute(this, "Id", true);
            new StringAttribute(this, "Name", false);
        }
    }
}
