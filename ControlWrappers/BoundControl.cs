
using System;

namespace ControlWrappers
{
    
    
    public interface BoundControl
    {
        string AttributeName { get; set; }
        string ContextName { get; set; }
        string DomainName { get; set; }
        void SetFromContext();
        void SetToContext();
        void ConnectControl();
    }
}
