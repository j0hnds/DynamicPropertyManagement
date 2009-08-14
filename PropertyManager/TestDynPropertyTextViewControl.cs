
using System;
using Gtk;
using ControlWrappers;
using Pango;

namespace PropertyManager
{
    
    
    public class TestDynPropertyTextViewControl : BaseTextViewControl
    {
        
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
