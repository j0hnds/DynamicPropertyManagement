
using System;
using System.Collections.Generic;
using Gtk;
using DomainCore;
using ControlWrappers;

namespace PropertyManager
{
    /// <summary>
    /// Enumeration of the different levels that can be selected in the
    /// effective value tree.
    /// </summary>
    public enum EffectiveDateLevels 
    {
        None,
        TopLevel,
        EffectiveDate,
        ValueCriteria
    }
    

    /// <summary>
    /// Wrapper class to provide custom functionality for the
    /// effective value TreeView.
    /// </summary>
    public class EffectiveDateListControl : BaseTreeControl
    {
        /// <summary>
        /// The model for this TreeView.
        /// </summary>
        private TreeStore treeStore;

        /// <summary>
        /// Constructs a new EffectiveDateListControl wrapper object.
        /// </summary>
        /// <param name="widget">
        /// The TreeView to be wrapped.
        /// </param>
        public EffectiveDateListControl(Widget widget) :
            base(widget)
        {
            treeStore = new TreeStore(GLib.GType.String, GLib.GType.Int64, GLib.GType.String);

            // Set up a single column
            TreeViewColumn tc = new TreeViewColumn();
            tc.Title = "Name";
            CellRenderer cell = new CellRendererText();
            tc.PackStart(cell, true);
            tc.AddAttribute(cell, "text", LABEL_CELL);
            ((TreeView) widget).AppendColumn(tc);

            ((TreeView) widget).Model = treeStore;
        }

        /// <value>
        /// The DynamicProperty at the root of this TreeView.
        /// </value>
        public Domain DynamicProperty
        {
            get { return ((TreeView) widget).Data["Domain"] as Domain; }
            set { ((TreeView) widget).Data["Domain"] = value; }
        }

        /// <summary>
        /// Retrieves the domain object that is represented by the currently
        /// selected row in the TreeView.
        /// </summary>
        /// <returns>
        /// The domain object represented by the currently selected row.
        /// </returns>
        public override Domain GetSelectedDomain()
        {
            Domain selectedDomain = null;
            Domain dynProperty = DynamicProperty;

            TreeIter iter = TreeIter.Zero;

            if (((TreeView) widget).Selection.GetSelected(out iter))
            {
                TreePath path = treeStore.GetPath(iter);
                int[] selIndices = path.Indices;

                log.DebugFormat("Length of indices: {0}, {1}", selIndices.Length, selIndices);

                List<Domain> evs = dynProperty.GetCollection("EffectiveValues");
                Domain ev = evs[selIndices[1]];
                if (selIndices.Length > 2)
                {
                    List<Domain> vcs = ev.GetCollection("ValueCriteria");
                    selectedDomain = vcs[selIndices[2]];
                }
                else
                {
                    selectedDomain = ev;
                }

            }
            return selectedDomain;
        }

        /// <summary>
        /// Retrieves the domain object that is the parent of the currently
        /// selected row in the TreeView.
        /// </summary>
        /// <returns>
        /// The domain object that is the parent of the selected row.
        /// </returns>
        public virtual Domain GetSelectedDomainParent()
        {
            Domain selectedDomain = null;
            Domain dynProperty = DynamicProperty;

            TreeModel model = null;
            TreeIter iter = TreeIter.Zero;

            if (((TreeView) widget).Selection.GetSelected(out model, out iter))
            {
                TreeIter parentIter = TreeIter.Zero;
                if (model.IterParent(out parentIter, iter))
                {
                    TreePath path = treeStore.GetPath(parentIter);
                    
                    int[] selIndices = path.Indices;
                    
                    List<Domain> evs = dynProperty.GetCollection("EffectiveValues");
                    Domain ev = evs[selIndices[1]];
                    if (selIndices.Length > 2)
                    {
                        List<Domain> vcs = ev.GetCollection("ValueCriteria");
                        selectedDomain = vcs[selIndices[2]];
                    }
                    else
                    {
                        selectedDomain = ev;
                    }
                }
            }

            return selectedDomain;
        }

        /// <value>
        /// The currently selected level of the TreeView.
        /// </value>
        public EffectiveDateLevels SelectedLevel
        {
            get
            {
                EffectiveDateLevels level = EffectiveDateLevels.None;
                
                TreeModel model = null;
                TreeIter iter = TreeIter.Zero;

                if (GetSelected(out model, out iter))
                {
                    // Something is selected.
                    TreeIter parentIter = TreeIter.Zero;
                    if (model.IterParent(out parentIter, iter))
                    {
                        // That something has a parent
                        TreeIter grandParentIter = TreeIter.Zero;
                        if (model.IterParent(out grandParentIter, parentIter))
                        {
                            // The selected item has a grandparent
                            level = EffectiveDateLevels.ValueCriteria;
                        }
                        else
                        {
                            // No grandparent
                            level = EffectiveDateLevels.EffectiveDate;
                        }
                    }
                    else
                    {
                        level =  EffectiveDateLevels.TopLevel;
                    }
                }
                else
                {
                    level = EffectiveDateLevels.None;
                }

                return level;
            }
        }

        /// <summary>
        /// Adds a new effective date domain object to the TreeView.
        /// </summary>
        /// <param name="effectiveDate">
        /// The EffectiveValue domain object to add to the TreeView.
        /// </param>
        public void AddEffectiveDate(Domain effectiveDate)
        {
            // Get the iterator for the top of the tree...
            TreeIter topIter = TreeIter.Zero;
            
            if (treeStore.GetIterFirst(out topIter))
            {
                TreeIter edIter = treeStore.AppendValues(topIter,
                                                         RenderLabel(effectiveDate),
                                                         effectiveDate.IdAttribute.Value,
                                                         effectiveDate.GetType().Name);

                SelectRow(edIter);
            }
        }

        /// <summary>
        /// Adds a ValueCriteria domain object to the TreeView.
        /// </summary>
        /// <param name="valueCriteria">
        /// The ValueCriteria domain object to add.
        /// </param>
        public void AddValueCriteria(Domain valueCriteria)
        {
            // Get the iterator of the select item (effective date)
            TreeModel model = null;
            TreeIter edIter = TreeIter.Zero;
            
            if (GetSelected(out model, out edIter))
            {
                TreeIter vcIter = treeStore.AppendValues(edIter,
                                                         RenderLabel(valueCriteria),
                                                         valueCriteria.IdAttribute.Value,
                                                         valueCriteria.GetType().Name);

                SelectRow(vcIter);
            }
        }

        /// <summary>
        /// Populates the tree with the contents of the specified DynamicProperty.
        /// </summary>
        /// <param name="dynamicProperty">
        /// The DynamicProperty from which to populate the TreeView.
        /// </param>
        public void Populate(Domain dynamicProperty)
        {
            treeStore.Clear();

            TreeIter topIter = treeStore.AppendValues("Effective Values", -1L, "Nothing");

            List<Domain> effectiveValues = dynamicProperty.GetCollection("EffectiveValues");
            foreach (Domain effectiveValue in effectiveValues)
            {
                TreeIter evIter = TreeIter.Zero;
                evIter = treeStore.AppendValues(topIter,
                                                RenderLabel(effectiveValue),
                                                effectiveValue.IdAttribute.Value,
                                                effectiveValue.GetType().Name);

                List<Domain> valueCriterias = effectiveValue.GetCollection("ValueCriteria");
                foreach (Domain vc in valueCriterias)
                {
                    treeStore.AppendValues(evIter,
                                           RenderLabel(vc),
                                           vc.IdAttribute.Value,
                                           vc.GetType().Name);
                }
            }
        }

        /// <summary>
        /// Updates the labels for the selected TreeView item.
        /// </summary>
        public void UpdateSelected()
        {
            log.Debug("UpdateSelected...");
            TreeModel model = null;
            TreeIter iter = TreeIter.Zero;
            Domain domain = GetSelectedDomain();

            if (((TreeView) widget).Selection.GetSelected(out model, out iter))
            {
                log.Debug("Update Selected; found selection");
                model.SetValue(iter, LABEL_CELL, RenderLabel(domain));
            }
        }
    }
}
