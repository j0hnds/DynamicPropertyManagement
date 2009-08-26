
using System;
using System.Collections.Generic;
using Gtk;
using DomainCore;
using ControlWrappers;

namespace PropertyManager
{
    /// <summary>
    /// Enumeration of the different levels of the Property Definition Tree.
    /// </summary>
    public enum PropertyDefinitionLevels
    {
        None,
        Category,
        Property
    }

    /// <summary>
    /// Wrapper class to provide application-specific support for displaying
    /// properties.
    /// </summary>
    public class PropertyListControl : BaseTreeControl
    {
        /// <summary>
        /// The model of this TreeView.
        /// </summary>
        private TreeStore treeStore;

        /// <summary>
        /// Constructs a new PropertyListControl wrapper.
        /// </summary>
        /// <param name="widget">
        /// The TreeView widget to wrap.
        /// </param>
        public PropertyListControl(Widget widget) :
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
        /// The level of the currently selected row in the TreeView.
        /// </value>
        public PropertyDefinitionLevels SelectedLevel
        {
            get
            {
                PropertyDefinitionLevels level = PropertyDefinitionLevels.None;
                
                TreeModel model = null;
                TreeIter iter = TreeIter.Zero;

                if (GetSelected(out model, out iter))
                {
                    TreeIter parentIter = TreeIter.Zero;
                    if (model.IterParent(out parentIter, iter))
                    {
                        level = PropertyDefinitionLevels.Property;
                    }
                    else
                    {
                        level = PropertyDefinitionLevels.Category;
                    }
                }

                return level;
            }
        }

        /// <summary>
        /// Adds a new domain object to the TreeView.
        /// </summary>
        /// <param name="domain">
        /// The domain object to add.
        /// </param>
        public void AddDomain(Domain domain)
        {
            object category = domain.GetValue("Category");

            // Find the correct category to add the new domain into
            TreeIter iter = TreeIter.Zero;
            bool more = treeStore.GetIterFirst(out iter);
            while (more)
            {
                string label = (string) treeStore.GetValue(iter, LABEL_CELL);
                if (label.Equals(category))
                {
                    // Found the right category
                    TreeIter propIter = TreeIter.Zero;
                    propIter = treeStore.AppendValues(iter,
                                                      RenderLabel(domain),
                                                      domain.IdAttribute.Value,
                                                      domain.GetType().Name);
                    SelectRow(propIter);
                    break;
                }
                more = treeStore.IterNext(ref iter);
            }

            if (! more)
            {
                // This means that we didn't find the category; add
                // a category and property
                TreeIter catIter = treeStore.AppendValues(Render(domain, "CategoryLabel"),
                                                          domain.IdAttribute.Value,
                                                          domain.GetType().Name);
                TreeIter propIter = treeStore.AppendValues(catIter,
                                                           RenderLabel(domain),
                                                           domain.IdAttribute.Value,
                                                           domain.GetType().Name);
                SelectRow(propIter);
            }
        }

        /// <summary>
        /// Populates the TreeView with the contents of the list.
        /// </summary>
        /// <param name="properties">
        /// The PropertyDefinition objects to populate list TreeView with.
        /// </param>
        public void Populate(List<Domain> properties)
        {
            treeStore.Clear();

            object currentCategory = null;
            TreeIter catIter = TreeIter.Zero;

            foreach (Domain domain in properties)
            {
                object category = domain.GetValue("Category");
                if (! category.Equals(currentCategory))
                {
                    log.DebugFormat("Adding Category: {0}", Render(domain, "CategoryLabel"));
                    catIter = treeStore.AppendValues(Render(domain, "CategoryLabel"),
                                                     domain.IdAttribute.Value,
                                                     domain.GetType().Name);
                    currentCategory = category;
                }
                log.DebugFormat("Adding property: {0}", RenderLabel(domain));
                treeStore.AppendValues(catIter,
                                       RenderLabel(domain),
                                       domain.IdAttribute.Value,
                                       domain.GetType().Name);
            }
        }

        /// <summary>
        /// Updates the label of the currently selected object.
        /// </summary>
        public void UpdateSelectedLabel()
        {
            TreeModel model = null;
            TreeIter iter = TreeIter.Zero;
            Domain domain = GetSelectedDomain();

            if (((TreeView) widget).Selection.GetSelected(out model, out iter))
            {
                TreeIter parentIter = TreeIter.Zero;
                if (model.IterParent(out parentIter, iter))
                {
                    model.SetValue(iter, LABEL_CELL, RenderLabel(domain));
                }
                else
                {
                    model.SetValue(iter, LABEL_CELL, Render(domain, "CategoryLabel"));
                }
            }
        }
    }
}
