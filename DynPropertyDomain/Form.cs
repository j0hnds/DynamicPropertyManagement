
using System;
using System.Collections.Generic;
using DomainCore;
using DAOCore;

namespace DynPropertyDomain
{
    
    
    namespace DAO
    {
        class FormDAO : DAOBase
        {
            private static readonly Dictionary<string,string> ATTR_COL_MAPPINGS = new Dictionary<string, string>();

            static FormDAO()
            {
                ATTR_COL_MAPPINGS["Id"] = "FORM_ID";
                ATTR_COL_MAPPINGS["Description"] = "FORM_DESC";
            }
            
            public FormDAO() : 
                base("FORM", ATTR_COL_MAPPINGS)
            {
            }
        }
    }
    public class Form : Domain
    {
        
        public Form(DomainDAO dao) :
            base(dao)
        {
            new LongAttribute(this, "Id", true);
            new StringAttribute(this, "Description", false);
        }
    }
}
