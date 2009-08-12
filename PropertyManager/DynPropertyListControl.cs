
using System;
using System.Collections.Generic;
using Gtk;
using ControlWrappers;
using DomainCore;

namespace PropertyManager
{
    public enum DynamicPropertyLevels 
    {
        None,
        Application,
        Category,
        Property
    }
    
    public class DynPropertyListControl : BaseTreeControl
    {
        private TreeStore treeStore;
        
        public DynPropertyListControl(Widget widget) :
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

        public DynamicPropertyLevels SelectedLevel
        {
            get
            {
                DynamicPropertyLevels level = DynamicPropertyLevels.None;
                
                TreeModel model = null;
                TreeIter iter = TreeIter.Zero;

                if (GetSelected(out model, out iter))
                {
                    TreeIter parentIter = TreeIter.Zero;
                    if (model.IterParent(out parentIter, iter))
                    {
                        // Selected row has a parent; does it have two parents?
                        TreeIter grandParentIter = TreeIter.Zero;
                        if (model.IterParent(out grandParentIter, parentIter))
                        {
                            // Selected row has grandparent, must be property.
                            level = DynamicPropertyLevels.Property;
                        }
                        else
                        {
                            // No grandparent, must be category level.
                            level = DynamicPropertyLevels.Category;
                        }
                    }
                    else
                    {
                        // No parent; must be application level...
                        level = DynamicPropertyLevels.Application;
                    }
                }

                return level;
            }
        }

        public void Populate(List<Domain> applications)
        {
            treeStore.Clear();

            DomainDAO dao = DomainFactory.GetDAO("DynamicProperty");

            TreeIter appIter = TreeIter.Zero;
            foreach (Domain app in applications)
            {
                string appName = app.GetValue("Name") as string;
                List<Domain> dProps = dao.Get(appName);
                appIter = treeStore.AppendValues(RenderLabel(app),
                                                 app.IdAttribute.Value,
                                                 app.GetType().Name);

                object currentCategory = null;
                TreeIter catIter = TreeIter.Zero;
                foreach (Domain prop in dProps)
                {
                    object category = prop.GetValue("Category");
                    if (! category.Equals(currentCategory))
                    {
                        catIter = treeStore.AppendValues(appIter,
                                                         Render(prop, "CategoryLabel"),
                                                         prop.IdAttribute.Value,
                                                         prop.GetType().Name);
                        currentCategory = category;
                    }

                    treeStore.AppendValues(catIter,
                                           RenderLabel(prop),
                                           prop.IdAttribute.Value,
                                           prop.GetType().Name);
                }
            }
        }
    }
}
