
using System;
using System.Collections.Generic;
using Gtk;
using ControlWrappers;
using DomainCore;

namespace PropertyManager
{
    /// <summary>
    /// An enumeration of the different levels in the Dynamic Property
    /// control tree.
    /// </summary>
    public enum DynamicPropertyLevels 
    {
        None,
        Application,
        Category,
        Property
    }

    /// <summary>
    /// A specialized TreeView control to handle the display and operation of the
    /// Dynamic Property TreeView control.
    /// </summary>
    public class DynPropertyListControl : BaseTreeControl
    {
        /// <summary>
        /// The model used for this tree.
        /// </summary>
        private TreeStore treeStore;

        /// <summary>
        /// Constructs a new DynPropertyListControl wrapper.
        /// </summary>
        /// <param name="widget">
        /// The TreeView control to be wrapped.
        /// </param>
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

        /// <value>
        /// The level of the tree that is currently selected.
        /// </value>
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

        /// <summary>
        /// Populates the TreeView with the list of DynamicProperty domain objects.
        /// </summary>
        /// <param name="applications">
        /// List of DynamicProperty objects.
        /// </param>
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

        public virtual Domain GetSelectedDomainParent()
        {
            Domain domain = null;

            TreeModel model = null;
            TreeIter iter = TreeIter.Zero;

            if (((TreeView) widget).Selection.GetSelected(out model, out iter))
            {
                TreeIter parentIter = TreeIter.Zero;
                if (model.IterParent(out parentIter, iter))
                {
                    domain = GetDomain(model, parentIter);
                }
            }

            return domain;
        }

        public virtual Domain GetSelectedDomainGrandParent()
        {
            Domain domain = null;

            TreeModel model = null;
            TreeIter iter = TreeIter.Zero;

            if (((TreeView) widget).Selection.GetSelected(out model, out iter))
            {
                TreeIter parentIter = TreeIter.Zero;
                if (model.IterParent(out parentIter, iter))
                {
                    TreeIter grandParentIter = TreeIter.Zero;
                    if (model.IterParent(out grandParentIter, parentIter))
                    {
                        domain = GetDomain(model, grandParentIter);
                    }
                }
            }

            return domain;
        }

    }
}
