
using System;
using ControlWrappers;

namespace PropertyManager
{
    
    
    public partial class FormEntryDlg : DataBoundDialog // Gtk.Dialog
    {
        
        public FormEntryDlg()
        {
            this.Build();

//            new EntryBoundControl(this, txtFormId, "Id");
//            new EntryBoundControl(this, txtDescription, "Description");
        }
    }
}
