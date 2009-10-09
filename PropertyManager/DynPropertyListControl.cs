
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

        private TreeIter FindApplicationIter(long applicationId)
        {
            TreeIter foundIter = TreeIter.Zero;
            TreeIter iter = TreeIter.Zero;
            bool more = treeStore.GetIterFirst(out iter);
            while (more)
            {
                // Get the application Id of the current row
                long id = (long) treeStore.GetValue(iter, ID_CELL);
                if (applicationId == id)
                {
                    foundIter = iter;
                    break;
                }
                
                more = treeStore.IterNext(ref iter);
            }
            
            return foundIter;
        }

        private TreeIter FindCategoryIter(TreeIter appIter, string category)
        {
            log.InfoFormat("The category we are looking for is = '{0}'", category);

            TreeIter foundIter = TreeIter.Zero;
            TreeIter iter = TreeIter.Zero;
            bool more = treeStore.IterChildren(out iter, appIter);
            while (more)
            {
                // Get the application Id of the current row
                string lblCategory = (string) treeStore.GetValue(iter, LABEL_CELL);
                log.InfoFormat("The category label we are testing is = '{0}'", lblCategory);
                if (lblCategory.Equals(category))
                {
                    foundIter = iter;
                    break;
                }
                
                more = treeStore.IterNext(ref iter);
            }
            
            return foundIter;
        }

        public void AddDomain(Domain domain)
        {
            // Get the iter for the owner of this new domain object.
            long applicationId = (long) domain.GetValue("ApplicationId");
            long propertyId = (long) domain.GetValue("PropertyId");

            DomainDAO dao = DomainFactory.GetDAO("PropertyDefinition");
            Domain propDef = dao.GetObject(propertyId);

            string category = (string) propDef.GetValue("Category");

            domain.SetValue("Category", category);
            domain.SetValue("PropertyName", propDef.GetValue("Name"));

            TreeIter appIter = FindApplicationIter(applicationId);
            if (! appIter.Equals(TreeIter.Zero))
            {
                // Found the application, now find the category to
                // hook up to...
                TreeIter catIter = FindCategoryIter(appIter, category);
                TreeIter propIter = TreeIter.Zero;
                if (! catIter.Equals(TreeIter.Zero))
                {
                    propIter = treeStore.AppendValues(catIter,
                                           RenderLabel(domain),
                                           domain.IdAttribute.Value,
                                           domain.GetType().Name);
                }
                else
                {
                    // The category isn't there; need to create one.
                    catIter = treeStore.AppendValues(appIter,
                                                     Render(domain, "CategoryLabel"),
                                                     domain.IdAttribute.Value,
                                                     domain.GetType().Name);
                    
                    propIter = treeStore.AppendValues(catIter,
                                           RenderLabel(domain),
                                           domain.IdAttribute.Value,
                                           domain.GetType().Name);
                }

                // Now highlight the specified row
                SelectRow(propIter);
            }
            else
            {
                log.ErrorFormat("Unable to find application Id ({0}) to add domain", applicationId);
            }
            
        }

    }
}
