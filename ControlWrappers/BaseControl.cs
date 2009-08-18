
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

}
