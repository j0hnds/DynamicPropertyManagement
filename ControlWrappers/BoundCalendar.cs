
using System;

namespace ControlWrappers
{
    
    
    [System.ComponentModel.ToolboxItem(true)]
    public partial class BoundCalendar : Gtk.Bin, BoundControl
    {
        private BoundControlDelegate bcd;
        
        public BoundCalendar()
        {
            this.Build();
            bcd = new BoundControlDelegate(this);
            bcd.ContextChanged += ContextChangeHandler;
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
                    calBound.Sensitive = true;
                    sbHour.Sensitive = true;
                    sbMinute.Sensitive = true;
                }
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
            Date = (DateTime) bcd.DomainValue;
        }
      
        
        public void SetToContext ()
        {
            bcd.DomainValue = Date;
        }
        #endregion

    }
}
