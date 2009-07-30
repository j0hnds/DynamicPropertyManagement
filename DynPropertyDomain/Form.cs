
using System;
using DomainCore;

namespace DynPropertyDomain
{
    
    
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
