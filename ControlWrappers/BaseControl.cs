
using System;
using System.Collections.Generic;
using DomainCore;
using Gtk;
using log4net;

namespace ControlWrappers
{


    /// <summary>
    /// Base class for all wrapped controls.
    /// </summary>
    public abstract class BaseControl
    {
        // The Widget being wrapped
        protected Widget widget;
        // The logger to use for wrapped controls.
        protected ILog log;

        /// <summary>
        /// Constructs a new BaseControl object.
        /// </summary>
        /// <remarks>
        /// Sets up the wrapped widget and the logger that will be used
        /// for derived controls.
        /// </remarks>
        /// <param name="widget">
        /// The widget to be wrapped.
        /// </param>
        public BaseControl(Widget widget)
        {
            log = LogManager.GetLogger(this.GetType());
            this.widget = widget;
        }

    }

}
