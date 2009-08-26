
using System;

namespace ControlWrappers
{
    
    /// <summary>
    /// A custom Gtk control to construct a data bound time control.
    /// </summary>
    [System.ComponentModel.ToolboxItem(true)]
    public partial class BoundTime : Gtk.Bin
    {
        /// <summary>
        /// Constructs a new BoundTime control.
        /// </summary>
        public BoundTime()
        {
            this.Build();
        }

        /// <value>
        /// The time value represented by this control.
        /// </value>
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
