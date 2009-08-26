
using System;

namespace ControlWrappers
{
    
    /// <summary>
    /// Custom Gtk Control that provides a data bound Calendar control.
    /// </summary>
    [System.ComponentModel.ToolboxItem(true)]
    public partial class BoundCalendar : Gtk.Bin, BoundControl
    {
        /// <summary>
        /// Reference to a delegate handling the data binding functionality.
        /// </summary>
        private BoundControlDelegate bcd;
        /// <summary>
        /// <c>true</c> if the control is connected to its associated data context.
        /// </summary>
        private bool connected = false;
        /// <summary>
        /// <c>true</c> if the control is having its value initialized.
        /// </summary>
        private bool initializing = false;

        /// <summary>
        /// This event is fired whenever the date/time values of the control have changed.
        /// </summary>
        public event EventHandler DateTimeChanged;

        /// <summary>
        /// Constructs a new BoundCalendar object.
        /// </summary>
        public BoundCalendar()
        {
            this.Build();

            // Instantiate the data binding delegate.
            bcd = new BoundControlDelegate(this);

            // Connect a handler to the context change event of the delegate.
            bcd.ContextChanged += ContextChangeHandler;
        }

        /// <summary>
        /// Helper method to notify all subscribers to the DateTimeChanged event.
        /// </summary>
        /// <param name="e">
        /// The event arguments. Nothing of any interest here.
        /// </param>
        private void NotifyDateTimeChanged(EventArgs e)
        {
            EventHandler handler = DateTimeChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Event handler to respond to a change in the data context to which this
        /// control is connected.
        /// </summary>
        /// <param name="contextName">
        /// The name of the context that has changed.
        /// </param>
        /// <param name="itemName">
        /// The name of the item on the context that has changed.
        /// </param>
        private void ContextChangeHandler(string contextName, string itemName)
        {
            SetFromContext();
        }

        /// <value>
        /// The DateTime to get/set to/from the control.
        /// </value>
        public DateTime Date
        {
            get 
            {
                DateTime dt = DateTime.MinValue;
                
                if (cbNullDateTime.Active)
                {
                    DateTime calCtl = calBound.Date;
    
                    int hour = (int) sbHour.Value;
                    int minute = (int) sbMinute.Value;
                    
                    dt = new DateTime(calCtl.Year,
                                      calCtl.Month,
                                      calCtl.Day,
                                      hour,
                                      minute,
                                      0);
                }

                return dt;
            }
            set 
            {
                initializing = true;
                if (value == DateTime.MinValue)
                {
                    cbNullDateTime.Active = false;
                    calBound.Sensitive = false;
                    sbHour.Sensitive = false;
                    sbMinute.Sensitive = false;
                    DateTime now = DateTime.Now;
                    calBound.Date = now;
                    sbHour.Value = now.Hour;
                    sbMinute.Value = now.Minute;
                }
                else
                {
                    calBound.Date = value;
                    sbHour.Value = value.Hour;
                    sbMinute.Value = value.Minute;
                    cbNullDateTime.Active = true;
                    calBound.Sensitive = true;
                    sbHour.Sensitive = true;
                    sbMinute.Sensitive = true;
                }
                initializing = false;
            }
        }

        /// <summary>
        /// Event handler to handle the situation when the value of the NullDateTime control
        /// has been toggled.
        /// </summary>
        /// <param name="sender">
        /// Reference to the object that initiated the signal.
        /// </param>
        /// <param name="e">
        /// Reference to the arguments of the event.
        /// </param>
        protected virtual void NullDateTimeToggled (object sender, System.EventArgs e)
        {
            if (cbNullDateTime.Active)
            {
                calBound.Sensitive = true;
                sbHour.Sensitive = true;
                sbMinute.Sensitive = true;
            }
            else
            {
                calBound.Sensitive = false;
                sbHour.Sensitive = false;
                sbMinute.Sensitive = false;
            }

            SetToContext();
            NotifyDateTimeChanged(e);
        }

        #region BoundControl implementation
        public string AttributeName 
        {
            get { return bcd.AttributeName; }
            set { bcd.AttributeName = value; }
        }
        
        public void ConnectControl ()
        {
            bcd.ConnectControl();
            connected = true;
        }
      
        
        public string ContextName {
            get { return bcd.ContextName; }
            set { bcd.ContextName = value; }
        }
        
        public string DomainName 
        {
            get { return bcd.DomainName; }
            set { bcd.DomainName = value; }
        }
        
        public void SetFromContext ()
        {
            object oDate = bcd.DomainValue;
            if (oDate != null)
            {
                Date = (DateTime) oDate;
            }
        }
      
        
        public void SetToContext ()
        {
            if (connected && ! initializing)
            {
                bcd.DomainValue = Date;
            }
        }

        #endregion

        /// <summary>
        /// Signal handler for a change to the current value of the calendar control.
        /// </summary>
        /// <param name="sender">
        /// Reference to the calendar control.
        /// </param>
        /// <param name="e">
        /// Reference to the event arguments.
        /// </param>
        protected virtual void CalendarDaySelected (object sender, System.EventArgs e)
        {
            SetToContext();
            NotifyDateTimeChanged(e);
        }

        /// <summary>
        /// Signal handler for a change to the current value of the calendar control.
        /// </summary>
        /// <param name="sender">
        /// Reference to the calendar control.
        /// </param>
        /// <param name="e">
        /// Reference to the event arguments.
        /// </param>
        protected virtual void CalendarDaySelectedDoubleClick (object sender, System.EventArgs e)
        {
            SetToContext();
            NotifyDateTimeChanged(e);
        }

        /// <summary>
        /// Signal handler for a change in the hour value.
        /// </summary>
        /// <param name="sender">
        /// Reference to the hour control
        /// </param>
        /// <param name="e">
        /// Reference to the event arguments.
        /// </param>
        protected virtual void HourValueChanged (object sender, System.EventArgs e)
        {
            SetToContext();
            NotifyDateTimeChanged(e);
        }

        /// <summary>
        /// Signal handler for a change to the minute control.
        /// </summary>
        /// <param name="sender">
        /// A reference to the minute control.
        /// </param>
        /// <param name="e">
        /// A reference to the event arguments.
        /// </param>
        protected virtual void MinuteValueChanged (object sender, System.EventArgs e)
        {
            SetToContext();
            NotifyDateTimeChanged(e);
        }
    }
}
