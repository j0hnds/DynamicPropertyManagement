
using System;
using DomainCore;
using Gtk;
using STUtils;

namespace ControlWrappers
{
    
    
    public class BaseTreeControl : BaseControl
    {

        protected const int LABEL_CELL = 0;
        protected const int ID_CELL = 1;
        protected const int DOMAIN_NAME_CELL = 2;
        
        public BaseTreeControl(Widget widget) :
            base(widget)
        {
        }

        public string RenderLabel(Domain domain)
        {
            // return DomainRenderer.Render(domain, "Label");
            return Render(domain, "Label");
        }

        public string Render(Domain domain, string renderType)
        {
            return DomainRenderer.Render(domain, renderType);
        }

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

        public bool GetSelected(out TreeModel model, out TreeIter iter)
        {
            model = null;
            iter = TreeIter.Zero;
            return ((TreeView) widget).Selection.GetSelected(out model, out iter);
        }

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

        public virtual void RemoveSelected()
        {
            TreeModel model = null;
            TreeIter iter = TreeIter.Zero;

            if (((TreeView) widget).Selection.GetSelected(out model, out iter))
            {
                ((TreeStore) model).Remove(ref iter);
            }
        }

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

        public Domain GetSelectedDomain()
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
