
using System;
using System.Collections.Generic;
using Gtk;
using DomainCore;
using ControlWrappers;

namespace PropertyManager
{
    public enum EffectiveDateLevels 
    {
        None,
        TopLevel,
        EffectiveDate,
        ValueCriteria
    }
    
    
    public class EffectiveDateListControl : BaseTreeControl
    {
        private TreeStore treeStore;
        
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

        public Domain DynamicProperty
        {
            get { return ((TreeView) widget).Data["Domain"] as Domain; }
            set { ((TreeView) widget).Data["Domain"] = value; }
        }

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
