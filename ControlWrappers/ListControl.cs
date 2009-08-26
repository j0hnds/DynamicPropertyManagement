
using System;
using Gtk;
using DomainCore;
using System.Collections.Generic;

namespace ControlWrappers
{
    
    /// <summary>
    /// A Wrapper class for a TreeView that provides relatively simple
    /// list functionality.
    /// </summary>
    /// <remarks>
    /// This class provides higher-level methods for interacting with domain
    /// objects.
    /// </remarks>
    public class ListControl : BaseTreeControl
    {

        /// <summary>
        /// The model for the list control.
        /// </summary>
        private ListStore listStore;

        /// <summary>
        /// Constructs a new ListControl wrapper object for the specified widget.
        /// </summary>
        /// <param name="widget">
        /// The TreeView control to be wrapped.
        /// </param>
        public ListControl(Widget widget) :
            base(widget)
        {
            listStore = new ListStore(GLib.GType.String, GLib.GType.Int64, GLib.GType.String);

            ((TreeView) widget).Model = listStore;
        }

        /// <summary>
        /// Updates the label for the currently selected list item.
        /// </summary>
        /// <remarks>
        /// One would use this method if changes had been made to the underlying domain
        /// object to keep the display in sync with the current state of the domain object.
        /// </remarks>
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

        /// <summary>
        /// Adds a domain object to the list control.
        /// </summary>
        /// <remarks>
        /// This method takes care of generating the label and other information for
        /// the list control as well as providing enough information to later reference the
        /// domain object.
        /// </remarks>
        /// <param name="domain">
        /// A reference to the domain object.
        /// </param>
        public void AddDomain(Domain domain)
        {
            listStore.AppendValues(RenderLabel(domain),
                                   domain.IdAttribute.Value,
                                   domain.GetType().Name);
        }

        /// <summary>
        /// Populates the list control with the objects specified in the collection.
        /// </summary>
        /// <param name="domains">
        /// List of domain objects to populate the list.
        /// </param>
        public void Populate(List<Domain> domains)
        {
            listStore.Clear();

            foreach (Domain domain in domains)
            {
                AddDomain(domain);
            }
        }

        /// <summary>
        /// Selects a row in the TreeView that is associated with a domain object that
        /// matches the specified id.
        /// </summary>
        /// <param name="selectId">
        /// The ID of the domain object to be selected.
        /// </param>
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

        /// <summary>
        /// Selects a row in the TreeView that matches the label that would be used
        /// by the specified domain object.
        /// </summary>
        /// <param name="domain">
        /// Domain object to select.
        /// </param>
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

        /// <summary>
        /// Removes the currently selected domain object from the TreeView.
        /// </summary>
        public override void RemoveSelected()
        {
            TreeModel model = null;
            TreeIter iter = TreeIter.Zero;

            if (((TreeView) widget).Selection.GetSelected(out model, out iter))
            {
                ((ListStore) model).Remove(ref iter);
            }
        }

    }
}
