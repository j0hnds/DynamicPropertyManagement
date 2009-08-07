
using System;
using Gtk;
using DomainCore;
using System.Collections.Generic;

namespace ControlWrappers
{
    
    
    public class ListControl : BaseTreeControl
    {

        private ListStore listStore;
        
        public ListControl(Widget widget) :
            base(widget)
        {
            listStore = new ListStore(GLib.GType.String, GLib.GType.Int64, GLib.GType.String);

            ((TreeView) widget).Model = listStore;
        }

        public void UpdateSelectedLabel()
        {
            TreeModel model = null;
            TreeIter iter = TreeIter.Zero;

            if (((TreeView) widget).Selection.GetSelected(out model, out iter))
            {
                Domain domain = GetDomain(model, iter);

                model.SetValue(iter, LABEL_CELL, RenderLabel(domain));
            }
        }

        public void AddDomain(Domain domain)
        {
            listStore.AppendValues(RenderLabel(domain),
                                   domain.IdAttribute.Value,
                                   domain.GetType().Name);
        }

        public void Populate(List<Domain> domains)
        {
            listStore.Clear();

            foreach (Domain domain in domains)
            {
                AddDomain(domain);
            }
        }

        public void SelectDomain(long selectId)
        {
            TreeIter iter = TreeIter.Zero;
            
            bool ok = listStore.GetIterFirst(out iter);
            while (ok)
            {
                long domainId = (long) listStore.GetValue(iter, ID_CELL);
                if (domainId == selectId)
                {
                    SelectRow(iter);
                    break;
                }
                ok = listStore.IterNext(ref iter);
            }
        }

        public void SelectDomainByLabel(Domain domain)
        {
            string selectLabel = RenderLabel(domain);

            TreeIter iter = TreeIter.Zero;

            bool ok = listStore.GetIterFirst(out iter);
            while (ok)
            {
                string label = (string) listStore.GetValue(iter, LABEL_CELL);
                if (label.Equals(selectLabel))
                {
                    SelectRow(iter);
                    break;
                }
                ok = listStore.IterNext(ref iter);
            }
        }
    }
}
