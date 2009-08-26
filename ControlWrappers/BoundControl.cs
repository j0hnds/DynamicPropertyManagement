
using System;

namespace ControlWrappers
{
    
    /// <summary>
    /// The methods and properties required for a control to be considered a
    /// Bound Control.
    /// </summary>
    /// <remarks>
    /// The binding being discussed here is semi-automated control binding to
    /// so-called Domain-objects, which are rather more fancy value objects.
    /// </remarks>
    public interface BoundControl
    {
        /// <value>
        /// The name of the attribute on the domain object to which this control
        /// should be bound.
        /// </value>
        string AttributeName { get; set; }
        /// <value>
        /// The name of the DataContext to which this control should be bound.
        /// </value>
        string ContextName { get; set; }
        /// <value>
        /// The name of the domain object to which this control should be bound.
        /// </value>
        string DomainName { get; set; }
        /// <summary>
        /// Set the displayed value of the control to the value in the domain/context.
        /// </summary>
        void SetFromContext();
        /// <summary>
        /// Updates the domian/context with the value represented by the control.
        /// </summary>
        void SetToContext();
        /// <summary>
        /// Connects the control to the data context identified by the <c>ContextName</c> property
        /// </summary>
        void ConnectControl();
    }
}
