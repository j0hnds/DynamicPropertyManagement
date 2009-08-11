
using System;
using System.Collections.Generic;
using DomainCore;
using Gtk;

namespace ControlWrappers
{
    
    
    public class ComboBoxBoundControl : BaseBoundControl
    {
        private string displayAttributeName;
        private string valueAttributeName;
        private ListStore listStore;
        
        public ComboBoxBoundControl(BoundDialog dlg, Widget widget, string attributeName, List<Domain> valueList, string displayAttributeName, string valueAttributeName) :
            base(dlg, widget, attributeName)
        {
            this.displayAttributeName = displayAttributeName;
            this.valueAttributeName = valueAttributeName;

            listStore = new ListStore(GLib.GType.String, GLib.GType.Int64);
            foreach (Domain domain in valueList)
            {
                object displayValue = domain.GetValue(displayAttributeName);
                object valueValue = domain.GetValue(valueAttributeName);

                log.DebugFormat("Display Value = {0}", displayValue);
                log.DebugFormat("Value Value = {0}", valueValue);

                listStore.AppendValues(displayValue, valueValue);
            }

            ((ComboBox) widget).Model = listStore;
        }

        public override void SetControlValue(Domain domain)
        {
            long domainValue = Convert.ToInt64(GetRawDomainValue(domain));

            TreeIter iter = TreeIter.Zero;
            bool more = listStore.GetIterFirst(out iter);
            while (more)
            {
                long listValue = (long) listStore.GetValue(iter, 1);
                if (listValue == domainValue)
                {
                    ((ComboBox) widget).SetActiveIter(iter);
                    break;
                }
                more = listStore.IterNext(ref iter);
            }
        }

        public override void GetControlValue(Domain domain)
        {
            TreeIter iter = TreeIter.Zero;

            long listValue = 0L;
            if (((ComboBox) widget).GetActiveIter(out iter))
            {
                listValue = (long) listStore.GetValue(iter, 1);
            }

            SetDomainValue(domain, listValue);
        }
    }
}
