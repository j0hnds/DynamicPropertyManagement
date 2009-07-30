
using System;
using DomainCore;

namespace DynPropertyDomain
{
    
    
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
