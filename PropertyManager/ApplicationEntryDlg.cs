
using System;
using Gtk;
using DomainCore;
using ControlWrappers;

namespace PropertyManager
{
    
    /// <summary>
    /// Dialog to allow creation/editing of application object.
    /// </summary>
    public partial class ApplicationEntryDlg : DataBoundDialog 
    {
        /// <summary>
        /// Constructs a new ApplicationDlg object.
        /// </summary>
        public ApplicationEntryDlg()
        {
            this.Build();
        }

    }
}
