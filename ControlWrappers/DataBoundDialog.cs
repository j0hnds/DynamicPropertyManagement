
using System;
using Gtk;
using DomainCore;

namespace ControlWrappers
{
    
    
    public class DataBoundDialog : Dialog, ContextContainer
    {

        public DataBoundDialog()
        {
        }

        private void ScanControls()
        {
            ScanControls(Child);
        }

        private void ScanControls(Widget w)
        {
            if (w is BoundControl)
            {
                ((BoundControl) w).ConnectControl();
            }
            else if (w is Container)
            {
                foreach (Widget widget in ((Container) w).AllChildren)
                {
                    ScanControls(widget);
                }
            }
        }

        #region ContextContainer implementation
        public DataContext GetContext (string name)
        {
            return Data[name] as DataContext;
        }

        public void SetContext (string name, DataContext context)
        {
            Data[name] = context;
        }
        #endregion

        protected virtual DataContext CreateDataContext()
        {
            DataContext context = new DataContext();

            SetContext("DialogContext", context);

            return context;
        }

        public virtual bool DoModal(Window parentWindow)
        {
            return DoModal(parentWindow, null);
        }

        public virtual bool DoModal(Window parentWindow, Domain domain)
        {
            bool ok = false;

            // Construct a data context and register it...
            DataContext context = CreateDataContext();

            ScanControls();

            if (domain != null)
            {
                context.AddObject(domain);
            }

            TransientFor = parentWindow;

            int response = Run();
            if (response == (int) Gtk.ResponseType.Ok)
            {
                ok = true;
            }
            else if (response == (int) Gtk.ResponseType.Cancel)
            {
                if (domain != null)
                {
                    domain.Revert();
                }
                ok = false;
            }

            Destroy();

            return ok;
        }
   
    }
}
