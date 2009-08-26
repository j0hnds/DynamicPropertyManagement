
using System;
using ControlWrappers;
using Gtk;

namespace PropertyManager
{
    
    /// <summary>
    /// Wrapper class to provide support for display of Form domain objects.
    /// </summary>
    public class FormListControl : ListControl
    {

        /// <summary>
        /// Constructs a new FormListControl wrapper object.
        /// </summary>
        /// <param name="widget">
        /// The TreeView to be wrapped.
        /// </param>
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
