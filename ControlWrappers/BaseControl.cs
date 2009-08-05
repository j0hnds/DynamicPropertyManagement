
using System;
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
            log = LogManager.GetLogger(typeof(BaseControl));
            this.widget = widget;
        }
    }
}
