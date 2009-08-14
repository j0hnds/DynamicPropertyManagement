
using System;
using System.Collections.Generic;
using DomainCore;
using Gtk;
using log4net;

namespace ControlWrappers
{

    
    public class BaseControl
    {
        protected Widget widget;
        protected ILog log;
        
        public BaseControl(Widget widget)
        {
            log = LogManager.GetLogger(this.GetType());
            this.widget = widget;
        }

    }

    public abstract class BaseBoundControl : BaseControl
    {

        private string attributeName;
        
        public BaseBoundControl(BoundDialog dlg, Widget widget, string attributeName) :
            base(widget)
        {
            this.attributeName = attributeName;
            
            if (dlg != null)
            {
                dlg.AddBoundControl(this);
            }
        }

        protected string AttributeName
        {
            get { return attributeName; }
        }

        protected string GetDomainValue(Domain domain)
        {
            string dValue = "";
            object rawValue = GetRawDomainValue(domain);
            if (rawValue != null)
            {
                dValue = rawValue.ToString();
            }

            return dValue;
        }

        protected object GetRawDomainValue(Domain domain)
        {
            return domain.GetValue(attributeName);
        }

        protected void SetDomainValue(Domain domain, object value)
        {
            domain.SetValue(attributeName, value);
        }

        public abstract void SetControlValue(Domain domain);

        public abstract void GetControlValue(Domain domain);
    }
}
