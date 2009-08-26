
using System;
using Gtk;
using log4net;
using DomainCore;

namespace ControlWrappers
{
    
    /// <summary>
    /// A helper class to which bound controls can delegate to handle the common
    /// functionality of data bound controls.
    /// </summary>
    public class BoundControlDelegate : BoundControl
    {
        /// <summary>
        /// The data bound widget that is delegating work to us.
        /// </summary>
        private Widget widget;
        /// <summary>
        /// The name of the attribute on the domain object to which this control
        /// should be bound.
        /// </summary>
        private string attributeName;
        /// <summary>
        /// The name of the DataContext to which this control should be bound.
        /// </summary>
        private string contextName;
        /// <summary>
        /// The name of the domain object to which this control should be bound.
        /// </summary>
        private string domainName;
        /// <summary>
        /// Reference to the data context to which we are connected.
        /// </summary>
        private DataContext context;
        /// <summary>
        /// The logger we will be using to log information about the data binding
        /// process.
        /// </summary>
        private ILog log;

        /// <summary>
        /// Constructs a new BoundControlDelegate object.
        /// </summary>
        /// <param name="widget">
        /// The widget for which we are the delegate for data binding.
        /// </param>
        public BoundControlDelegate(Widget widget)
        {
            log = LogManager.GetLogger(GetType());
            this.widget = widget;
        }

        /// <summary>
        /// Signal handler for ContextChange events issued by the data context to which
        /// we are connected.
        /// </summary>
        /// <param name="contextName">
        /// The name of the data context that changed.
        /// </param>
        /// <param name="itemName">
        /// The name of the item on the data context that changed.
        /// </param>
        private void ContextChangeHandler(string contextName, string itemName)
        {
            ContextChangeHandler handler = ContextChanged;

            if (handler != null)
            {
                handler(contextName, itemName);
            }
        }

        /// <summary>
        /// Event issued when a change has occurred on the data context
        /// to which we are connected.
        /// </summary>
        public event ContextChangeHandler ContextChanged;

        /// <value>
        /// The data context to which we are connected. If not already connected,
        /// will try to connect to the data context.
        /// </value>
        public DataContext Context
        {
            get 
            {
                if (context == null)
                {
                    if (contextName != null && contextName.Length > 0)
                    {
                        context = GetContext(widget);
                        if (context == null)
                        {
                            log.ErrorFormat("Unable to find data context '{0}'", contextName);
                        }
                    }
                    else
                    {
                        log.Warn("Context Name is not set on control");
                    }
                }

                return context; 
            }
        }

        /// <value>
        /// The domain object to which the control is data bound.
        /// </value>
        public Domain DomainObject
        {
            get
            {
                Domain domainObject = null;
                
                DataContext context = Context;
                if (context != null)
                {
                    if (domainName != null && domainName.Length > 0)
                    {
                        domainObject = context.GetDomain(domainName);
                        if (domainObject == null)
                        {
                            log.ErrorFormat("Unable to find domain object in context: {0}", domainName);
                        }
                    }
                    else
                    {
                        log.Warn("Domain Name is not set on control");
                    }
                }

                return domainObject;
            }
        }

        /// <value>
        /// The value associated with the data bound domain object.
        /// </value>
        public object DomainValue
        {
            get
            {
                object domainValue = null;

                Domain domain = DomainObject;
                if (domain != null)
                {
                    if (attributeName != null && attributeName.Length > 0)
                    {
                        domainValue = domain.GetValue(attributeName);
                    }
                    else
                    {
                        log.Warn("Attribute name not set on control");
                    }
                }
                

                return domainValue;
            }

            set
            {
                Domain domain = DomainObject;
                if (domain != null)
                {
                    if (attributeName != null && attributeName.Length > 0)
                    {
                        domain.SetValue(attributeName, value);
                    }
                    else
                    {
                        log.Warn("Attribute name not set on control");
                    }
                }
            }
        }

        /// <summary>
        /// Recursively searches the widget hierarchy for a data context container
        /// that has the required data context.
        /// </summary>
        /// <param name="widget">
        /// The widget on which to search for the data context.
        /// </param>
        /// <returns>
        /// A reference to the data context if found. Otherwise, return <c>null</c>.
        /// </returns>
        private DataContext GetContext(Widget widget)
        {
            DataContext context = null;

            if (widget != null)
            {
                if (widget is ContextContainer)
                {
                    context = ((ContextContainer) widget).GetContext(contextName);
                }
                else
                {
                    context = GetContext(widget.Parent);
                }
            }

            return context;
        }
        
        #region BoundControl implementation
        public string AttributeName 
        {
            get { return attributeName; }
            set { attributeName = value; }
        }
        
        public string ContextName 
        {
            get { return contextName; }
            set { contextName = value; }
        }
        
        public string DomainName {
            get { return domainName; }
            set { domainName = value; }
        }
        
        public void SetFromContext ()
        {
            throw new System.NotImplementedException();
        }
        
        public void SetToContext ()
        {
            throw new System.NotImplementedException();
        }

        public void ConnectControl ()
        {
            context = Context;

            if (context != null)
            {
                context.ContextChanged += ContextChangeHandler;
            }
        }
        #endregion
           
    }
}
