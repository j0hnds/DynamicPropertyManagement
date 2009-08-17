
using System;

namespace ControlWrappers
{
    
    
    [System.ComponentModel.ToolboxItem(true)]
    public partial class BoundTime : Gtk.Bin
    {
        
        public BoundTime()
        {
            this.Build();
        }

        public DateTime Time
        {
            get
            {
                int hour = (int) sbHour.Value;
                int minute = (int) sbMinute.Value;

                DateTime dtNow = DateTime.Now;

                return new DateTime(dtNow.Year,
                                    dtNow.Month,
                                    dtNow.Day,
                                    hour,
                                    minute,
                                    0);
            }
            set
            {
                sbHour.Value = value.Hour;
                sbMinute.Value = value.Minute;
            }
        }
    }
}
