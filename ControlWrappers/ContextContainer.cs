
using System;

namespace ControlWrappers
{
    
    
    public interface ContextContainer
    {
        DataContext GetContext(string name);
        void SetContext(string name, DataContext context);
    }
    
}
