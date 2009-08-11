
using System;
using DomainCore;
using Gtk;

namespace ControlWrappers
{
    
    
    public class TextViewBoundControl : BaseBoundControl
    {

        public TextViewBoundControl(BoundDialog dlg, Widget widget, string attributeName) :
            base(dlg, widget, attributeName)
        {
        }

        public override void SetControlValue (Domain domain)
        {
            ((TextView) widget).Buffer.Text = GetDomainValue(domain);
        }

        public override void GetControlValue (Domain domain)
        {
            SetDomainValue(domain, ((TextView) widget).Buffer.Text);
        }           
    }
}
