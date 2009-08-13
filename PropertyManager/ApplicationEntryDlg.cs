
using System;
using Gtk;
using DomainCore;
using ControlWrappers;

namespace PropertyManager
{
    
    
    public partial class ApplicationEntryDlg : BoundDialog //Gtk.Dialog
    {
        public ApplicationEntryDlg()
        {
            this.Build();
        }

        protected void DomainToControls(Widget w, Domain domain)
        {
            if (w is BoundControl)
            {
                ((BoundControl) w).SetFromDomain(domain);
            }
            else if (w is Container)
            {
                foreach (Widget widget in ((Container) w).AllChildren)
                {
                    DomainToControls(widget, domain);
                }
            }
                
        }
        
        protected override void DomainToControls(Domain domain)
        {
            DomainToControls(Child, domain);
        }

        protected void ControlsToDomain(Widget w, Domain domain)
        {
            if (w is BoundControl)
            {
                ((BoundControl) w).SetToDomain(domain);
            }
            else if (w is Container)
            {
                foreach (Widget widget in ((Container) w).AllChildren)
                {
                    ControlsToDomain(widget, domain);
                }
            }
        }

        protected override void ControlsToDomain(Domain domain)
        {
            ControlsToDomain(Child, domain);
        }

    }
}
