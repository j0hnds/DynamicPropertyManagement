
using System;
using DomainCore;
using Gtk;
using STUtils;

namespace ControlWrappers
{
    
    /// <summary>
    /// Abstract base class for tree controls.
    /// </summary>
    /// <remarks>
    /// Provides a number of useful methods to facilitate the use of the tree
    /// control with domain objects.
    /// </remarks>
    public abstract class BaseTreeControl : BaseControl
    {

        /// <summary>
        /// The index of the label within the model of the tree view.
        /// </summary>
        protected const int LABEL_CELL = 0;
        /// <summary>
        /// The index of the domain identifier within the model of the tree view.
        /// </summary>
        protected const int ID_CELL = 1;
        /// <summary>
        /// The index of name of the domain class within the model of the tree view.
        /// </summary>
        protected const int DOMAIN_NAME_CELL = 2;

        /// <summary>
        /// Constructs a new BaseTreeControl object.
        /// </summary>
        /// <param name="widget">
        /// Reference to the widget to be wrapped.
        /// </param>
        public BaseTreeControl(Widget widget) :
            base(widget)
        {
        }

        /// <summary>
        /// Renders the label to be used for the specified domain object.
        /// </summary>
        /// <param name="domain">
        /// The domain object from which the label information is to be determined.
        /// </param>
        /// <returns>
        /// The label to be used for this domain object.
        /// </returns>
        public string RenderLabel(Domain domain)
        {
            return Render(domain, "Label");
        }

        /// <summary>
        /// Renders information associated with the specified domain object.
        /// </summary>
        /// <remarks>
        /// Uses the <c>renderType</c> parameter to determine what type of information
        /// is to be rendered for the domain. In general, the <c>renderType</c> parameter
        /// is the name of a template in the StringTemplate macro file.
        /// </remarks>
        /// <param name="domain">
        /// The domain object to be rendered.
        /// </param>
        /// <param name="renderType">
        /// The rendering type to be applied to the domain object.
        /// </param>
        /// <returns>
        /// The string rendering of the domain object. The content of the string is dictated
        /// by the <c>renderType</c> parameter and the values within the domain object itself.
        /// </returns>
        public string Render(Domain domain, string renderType)
        {
            return DomainRenderer.Render(domain, renderType);
        }

        /// <summary>
        /// Selects the tree item specified by the iter and makes sure that the
        /// selected item is visible within the tree.
        /// </summary>
        /// <param name="iter">
        /// Reference to a tree iter identifying the tree item to be selected.
        /// </param>
        public void SelectRow(TreeIter iter)
        {
            TreeView tv = (TreeView) widget;
            // Get the path that is the equivalent to the iter
            TreeModel model = tv.Model;
            TreePath path = model.GetPath(iter);

            // Expand the tree to the specified path
            tv.ExpandToPath(path);
            // Scroll to the path to make it visible
            tv.ScrollToCell(path, tv.GetColumn(0), false, 0, 0);
            // Select the cell
            tv.Selection.SelectIter(iter);
        }

        /// <summary>
        /// Determines the selected item in the wrapped tree.
        /// </summary>
        /// <param name="model">
        /// The model of the tree is returned in this parameter.
        /// </param>
        /// <param name="iter">
        /// A TreeIter of the selected tree item is returned in this parameter.
        /// </param>
        /// <returns>
        /// <c>true</c> if there was a selected item in the tree; <c>false</c> otherwise.
        /// </returns>
        public bool GetSelected(out TreeModel model, out TreeIter iter)
        {
            model = null;
            iter = TreeIter.Zero;
            return ((TreeView) widget).Selection.GetSelected(out model, out iter);
        }

        /// <value>
        /// <c>true</c> if there is at least one item selected in the wrapped
        /// tree control.
        /// </value>
        public bool IsSelected
        {
            get
            {
                bool selected = false;

                int count = ((TreeView) widget).Selection.CountSelectedRows();

                selected = count > 0;

                return selected;
            }
        }

        /// <summary>
        /// Removes the currently selected tree element from the model.
        /// </summary>
        public virtual void RemoveSelected()
        {
            TreeModel model = null;
            TreeIter iter = TreeIter.Zero;

            if (((TreeView) widget).Selection.GetSelected(out model, out iter))
            {
                ((TreeStore) model).Remove(ref iter);
            }
        }

        /// <summary>
        /// Returns the domain object associated with the specified tree model and
        /// iter.
        /// </summary>
        /// <remarks>
        /// The domain will be retrieved from the backing store by looking up the appropriate
        /// DAO via the name of the domain object and the id of the domain stored in the
        /// model entry.
        /// </remarks>
        /// <param name="model">
        /// The tree model containin the domain information.
        /// </param>
        /// <param name="iter">
        /// The tree iter pointing to the tree item for which the domain is to be
        /// retrieved.
        /// </param>
        /// <returns>
        /// The domain object associated with the selected tree node.
        /// </returns>
        protected Domain GetDomain(TreeModel model, TreeIter iter)
        {
            Domain domain = null;

            // Get the ID of the domain object
            long id = (long) model.GetValue(iter, ID_CELL);

            // Get the name of the domain class 
            string className = (string) model.GetValue(iter, DOMAIN_NAME_CELL);

            // Get the domain from the backing store
            DomainDAO dao = DomainFactory.GetDAO(className);
            domain = dao.GetObject(id);

            return domain;
        }

        /// <summary>
        /// Returns a reference to the domain object associated with the currently
        /// selected tree node.
        /// </summary>
        /// <returns>
        /// Reference to the domain object associated with the currently selected tree node.
        /// </returns>
        public virtual Domain GetSelectedDomain()
        {
            Domain domain = null;

            TreeModel model = null;
            TreeIter iter = TreeIter.Zero;

            if (((TreeView) widget).Selection.GetSelected(out model, out iter))
            {
                domain = GetDomain(model, iter);
            }

            return domain;
        }
        
    }
}
