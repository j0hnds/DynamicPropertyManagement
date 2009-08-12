
using System;
using ControlWrappers;
using Gtk;

namespace PropertyManager
{
    
    
    public class FormListControl : ListControl
    {
        
        public FormListControl(Widget widget) :
            base(widget)
        {
            // Set up a single column
            TreeViewColumn tc = new TreeViewColumn();
            tc.Title = "Name";
            CellRenderer cell = new CellRendererText();
            tc.PackStart(cell, true);
            tc.AddAttribute(cell, "text", LABEL_CELL);
            ((TreeView) widget).AppendColumn(tc);
        }
    }
}
