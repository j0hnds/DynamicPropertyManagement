
using System;
using DomainCore;

namespace DynPropertyDomain
{
    
    
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
