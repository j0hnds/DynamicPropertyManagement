
using System;
using System.Collections.Generic;
using DomainCore;
using DAOCore;

namespace DynPropertyDomain
{
    
    namespace DAO
    {
        class DataTypeDAO : DAOBase
        {
            private static readonly Dictionary<string,string> ATTR_COL_MAPPINGS = new Dictionary<string, string>();

            static DataTypeDAO()
            {
            }
            
            public DataTypeDAO() : 
                base("DYN_DATA", ATTR_COL_MAPPINGS)
            {
            }
        }
    }
    
    public class DataType : Domain
    {
        
        public DataType(DomainDAO dao) :
            base(dao)
        {
            new LongAttribute(this, "Id", true);
            new StringAttribute(this, "Name", false);
        }
    }
}
