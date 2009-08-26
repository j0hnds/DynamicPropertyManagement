
using System;
using Gtk;
using ControlWrappers;

namespace PropertyManager
{
    
    /// <summary>
    /// Wrapper class to create a TextView that displays its text
    /// in Monospace font.
    /// </summary>
    public class MonospacedTextViewControl : BaseTextViewControl
    {

        /// <summary>
        /// Constructs a new MonospacedTextViewControl wrapper.
        /// </summary>
        /// <param name="widget">
        /// The TextView widget to be wrapped.
        /// </param>
        public MonospacedTextViewControl(Widget widget) :
            base(widget)
        {
            TextTag tag = new TextTag("Monospaced");
            tag.Family = "Monospace";
            buffer.TagTable.Add(tag);
        }
    }
}
