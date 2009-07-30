
using System;
using DomainCore;

namespace DynPropertyDomain
{
    
    
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
