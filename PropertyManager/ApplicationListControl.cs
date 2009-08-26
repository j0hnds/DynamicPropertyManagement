
using System;
using Gtk;
using ControlWrappers;

namespace PropertyManager
{
    
    /// <summary>
    /// This class provides a wrapper around a TreeView control that
    /// acts as a list. 
    /// </summary>
    public class ApplicationListControl : ListControl
    {

        /// <summary>
        /// Constructs a new ApplicationListControl object.
        /// </summary>
        /// <param name="widget">
        /// The TreeView widget to wrap.
        /// </param>
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
