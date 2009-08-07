
using System;
using Gtk;
using ControlWrappers;

namespace PropertyManager
{
    
    
    public class ApplicationListControl : ListControl
    {
        
        public ApplicationListControl(Widget widget) :
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
