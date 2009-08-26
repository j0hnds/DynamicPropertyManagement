
using System;
using Gtk;
using DomainCore;

namespace ControlWrappers
{
    
    /// <summary>
    /// A Custom Gtk Dialog box that understands how to manage controls that
    /// are data bound.
    /// </summary>
    public class DataBoundDialog : Dialog, ContextContainer
    {

        /// <summary>
        /// Constructs a new DataBoundDialog object.
        /// </summary>
        public DataBoundDialog()
        {
        }

        /// <summary>
        /// Helper method that scans the widget hierarchy for data bound
        /// controls.
        /// </summary>
        private void ScanControls()
        {
            ScanControls(Child);
        }

        /// <summary>
        /// Helper method that scans the widget hierarchy for data bound controls starting
        /// at the specified widget.
        /// </summary>
        /// <param name="w">
        /// The widget at which to start the scan of the hierarchy.
        /// </param>
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

        public void SetContext (DataContext context)
        {
            Data[context.Name] = context;
        }
        #endregion

        /// <summary>
        /// An overrideable method that creates a data context and registers it with
        /// the container.
        /// </summary>
        /// <remarks>
        /// The name of the default data context is <c>DialogContext</c>.
        /// </remarks>
        /// <returns>
        /// Reference to the newly created data context.
        /// </returns>
        protected virtual DataContext CreateDataContext()
        {
            DataContext context = new DataContext("DialogContext");

            SetContext(context);

            return context;
        }

        /// <summary>
        /// Displays the dialog as a child of the specified parent window.
        /// </summary>
        /// <param name="parentWindow">
        /// The owner of the modal dialog.
        /// </param>
        /// <returns>
        /// <c>true</c> if the end-user clicked the OK button to knock the dialog
        /// down. <c>false</c> if the end-user clicked the Cancel button.
        /// </returns>
        public virtual bool DoModal(Window parentWindow)
        {
            return DoModal(parentWindow, null);
        }

        /// <summary>
        /// Displays the dialog as a child of the specified parent window.
        /// </summary>
        /// <param name="parentWindow">
        /// The owner of the modal dialog.
        /// </param>
        /// <param name="domain">
        /// The domain object whose information is to be displayed in the dialog.
        /// </param>
        /// <returns>
        /// <c>true</c> if the end-user clicked the OK button to knock the dialog
        /// down. <c>false</c> if the end-user clicked the Cancel button.
        /// </returns>
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
