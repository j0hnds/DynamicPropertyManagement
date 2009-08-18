
using System;

namespace ControlWrappers
{
    
    
    [System.ComponentModel.ToolboxItem(true)]
    public partial class BoundCalendar : Gtk.Bin, BoundControl
    {
        private BoundControlDelegate bcd;
        private bool connected = false;
        private bool initializing = false;

        public event EventHandler DateTimeChanged;
        
        public BoundCalendar()
        {
            this.Build();
            bcd = new BoundControlDelegate(this);
            bcd.ContextChanged += ContextChangeHandler;
        }

        private void NotifyDateTimeChanged(EventArgs e)
        {
            EventHandler handler = DateTimeChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void ContextChangeHandler(string contextName, string itemName)
        {
            SetFromContext();
        }

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

        protected virtual void CalendarDaySelected (object sender, System.EventArgs e)
        {
            SetToContext();
            NotifyDateTimeChanged(e);
        }

        protected virtual void CalendarDaySelectedDoubleClick (object sender, System.EventArgs e)
        {
            SetToContext();
            NotifyDateTimeChanged(e);
        }

        protected virtual void HourValueChanged (object sender, System.EventArgs e)
        {
            SetToContext();
            NotifyDateTimeChanged(e);
        }

        protected virtual void MinuteValueChanged (object sender, System.EventArgs e)
        {
            SetToContext();
            NotifyDateTimeChanged(e);
        }
    }
}
