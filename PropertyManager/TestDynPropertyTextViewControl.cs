
using System;
using Gtk;
using ControlWrappers;
using Pango;

namespace PropertyManager
{
    
    /// <summary>
    /// Wrapper class for textual test information.
    /// </summary>
    public class TestDynPropertyTextViewControl : BaseTextViewControl
    {

        /// <summary>
        /// Constructs a new TestDynPropertyTextViewControl wrapper.
        /// </summary>
        /// <param name="widget">
        /// The TextView widget to wrap.
        /// </param>
        public TestDynPropertyTextViewControl(Widget widget) :
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
