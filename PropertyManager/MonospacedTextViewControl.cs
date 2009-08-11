
using System;
using Gtk;
using ControlWrappers;

namespace PropertyManager
{
    
    
    public class MonospacedTextViewControl : BaseTextViewControl
    {
        
        public MonospacedTextViewControl(Widget widget) :
            base(widget)
        {
            TextTag tag = new TextTag("Monospaced");
            tag.Family = "Monospace";
            buffer.TagTable.Add(tag);
        }
    }
}
