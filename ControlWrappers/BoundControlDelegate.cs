
using System;
using Gtk;
using log4net;
using DomainCore;

namespace ControlWrappers
{
    
    
    public class BoundControlDelegate : BoundControl
    {
        private Widget widget;
        private string attributeName;
        private string contextName;
        private string domainName;
        private DataContext context;
        private ILog log;
        
        public BoundControlDelegate(Widget widget)
        {
            log = LogManager.GetLogger(GetType());
            this.widget = widget;
        }

        private void ContextChangeHandler(string contextName, string itemName)
        {
            ContextChangeHandler handler = ContextChanged;

            if (handler != null)
            {
                handler(contextName, itemName);
            }
        }

        public event ContextChangeHandler ContextChanged;
        
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
