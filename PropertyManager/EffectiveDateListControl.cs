
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

                List<Domain> evs = dynProperty.GetCollection("EffectiveValues");
                Domain ev = evs[selIndices[0]];
                if (selIndices.Length > 1)
                {
                    List<Domain> vcs = ev.GetCollection("ValueCriteria");
                    selectedDomain = vcs[selIndices[1]];
                }
                else
                {
                    selectedDomain = ev;
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
                    TreeIter parentIter = TreeIter.Zero;
                    if (model.IterParent(out parentIter, iter))
                    {
                        level = EffectiveDateLevels.ValueCriteria;
                    }
                    else
                    {
                        level =  EffectiveDateLevels.EffectiveDate;
                    }
                }

                return level;
            }
        }

        public void Populate(Domain dynamicProperty)
        {
            treeStore.Clear();

            List<Domain> effectiveValues = dynamicProperty.GetCollection("EffectiveValues");
            foreach (Domain effectiveValue in effectiveValues)
            {
                TreeIter evIter = TreeIter.Zero;
                evIter = treeStore.AppendValues(RenderLabel(effectiveValue),
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
