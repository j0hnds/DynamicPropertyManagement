
using System;
using Gtk;
using Pango;
using ControlWrappers;

namespace PropertyManager
{
    
    
    public class MainTextViewControl : BaseTextViewControl
    {
        
        public MainTextViewControl(Widget widget) :
            base(widget)
        {
            TextTag tag = new TextTag("Caption");
            tag.Weight = Weight.Bold;
            buffer.TagTable.Add(tag);
        }
    }
}
