
using System;

namespace ControlWrappers
{
    
    /// <summary>
    /// Interface to be implemented by objects that wish to be considered a
    /// container for data contexts.
    /// </summary>
    public interface ContextContainer
    {
        /// <summary>
        /// Retrieves a context by the specified name.
        /// </summary>
        /// <param name="name">
        /// The name of the context to retrieve.
        /// </param>
        /// <returns>
        /// Data context of the specified name. <c>null</c> if the context is not
        /// found on the container.
        /// </returns>
        DataContext GetContext(string name);

        /// <summary>
        /// Sets the specified context into the context container.
        /// </summary>
        /// <param name="context">
        /// The data context to set in the container. The name of the data context
        /// is used to register the context.
        /// </param>
        void SetContext(DataContext context);
    }
    
}
