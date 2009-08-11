
using System;
using DomainCore;
using ControlWrappers;

namespace PropertyManager
{
    
    
    public partial class ApplicationEntryDlg : BoundDialog //Gtk.Dialog
    {
        private Domain domain;
        
        public ApplicationEntryDlg()
        {
            this.Build();

            new EntryBoundControl(this, txtApplicationName, "Name");
        }

    }
}
