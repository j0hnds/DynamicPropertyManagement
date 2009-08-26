
using System;
using Gtk;
using Pango;
using ControlWrappers;

namespace PropertyManager
{
    
    /// <summary>
    /// The wrapper class for the primary text display on the application.
    /// </summary>
    public class MainTextViewControl : BaseTextViewControl
    {

        /// <summary>
        /// Constructs a new MainTextViewControl object.
        /// </summary>
        /// <param name="widget">
        /// The TextView widget to wrap.
        /// </param>
        public MainTextViewControl(Widget widget) :
            base(widget)
        {
            TextTag tag = new TextTag("Caption");
            tag.Weight = Weight.Bold;
            buffer.TagTable.Add(tag);

            tag = new TextTag("Monospaced");
            tag.Family = "Monospace";
            buffer.TagTable.Add(tag);
        }
    }
}
