
using System;

namespace ControlWrappers
{
    
    
    public interface ContextContainer
    {
        DataContext GetContext(string name);
        void SetContext(DataContext context);
    }
    
}
