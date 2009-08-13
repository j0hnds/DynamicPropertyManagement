
using System;

namespace ControlWrappers
{
    
    
    public interface BoundControl
    {
        string AttributeName { get; set; }
        void SetFromDomain(DomainCore.Domain domain);
        void SetToDomain(DomainCore.Domain domain);
    }
}
