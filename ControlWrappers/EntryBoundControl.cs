
using System;
using Gtk;
using DomainCore;

namespace ControlWrappers
{
    
    
    public class EntryBoundControl : BaseBoundControl
    {

        public EntryBoundControl(BoundDialog dlg, Widget widget, string attributeName) :
            base(dlg, widget, attributeName)
        {
        }

        public override void GetControlValue (Domain domain)
        {
            string txt = ((Entry) widget).Text;
            SetDomainValue(domain, txt);
        }           

        public override void SetControlValue (Domain domain)
        {
            string txt = GetDomainValue(domain);
            ((Entry) widget).Text = txt;
        }
    }
}
