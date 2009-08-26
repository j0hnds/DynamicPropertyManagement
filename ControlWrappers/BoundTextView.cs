
using System;
using log4net;

namespace ControlWrappers
{
    
    /// <summary>
    /// A custom Gtk control to provide data binding support to a TextView control.
    /// </summary>
    [System.ComponentModel.ToolboxItem(true)]
    public partial class BoundTextView : Gtk.Bin, BoundControl
    {
        /// <summary>
        /// The delegate to handle all the data binding chores.
        /// </summary>
        private BoundControlDelegate bcd;
        /// <summary>
        /// The log to use for reporting items to the log.
        /// </summary>
        private ILog log;

        /// <summary>
        /// Constructs a new BoundTextView control.
        /// </summary>
        public BoundTextView()
        {
            this.Build();
            log = LogManager.GetLogger(GetType().Name);
            bcd = new BoundControlDelegate(this);
            bcd.ContextChanged += ContextChangeHandler;
            WrapMode = Gtk.WrapMode.Word;
        }

        /// <value>
        /// The wrapping mode to use for the TextView control.
        /// </value>
        public Gtk.WrapMode WrapMode
        {
            get { return tvBound.WrapMode; }
            set { tvBound.WrapMode = value; }
        }

        /// <summary>
        /// Signal handler for context changed events broadcast by the delegate.
        /// </summary>
        /// <param name="contextName">
        /// The name of the context that has changed.
        /// </param>
        /// <param name="itemName">
        /// The name of the item on the data context that has changed.
        /// </param>
        private void ContextChangeHandler(string contextName, string itemName)
        {
            SetFromContext();
        }
        
        #region BoundControl implementation
        public string AttributeName 
        {
            get { return bcd.AttributeName; }
            set { bcd.AttributeName = value; }
        }

        public void ConnectControl ()
        {
            bcd.ConnectControl();
        }

        public string ContextName 
        {
            get { return bcd.ContextName; }
            set { bcd.ContextName = value; }
        }

        public string DomainName 
        {
            get { return bcd.DomainName; }
            set { bcd.DomainName = value; }
        }

        public void SetFromContext ()
        {
            string domainValue = (string) bcd.DomainValue;

            if (domainValue == null)
            {
                domainValue = "";
            }

            tvBound.Buffer.Text = domainValue;
        }


        public void SetToContext ()
        {
            string ctlValue = tvBound.Buffer.Text;
            if (ctlValue.Length > 0)
            {
                bcd.DomainValue = ctlValue;
            }
            else
            {
                bcd.DomainValue = null;
            }
        }

        #endregion

        /// <summary>
        /// Signal handler for PasteFromClipboard on the TextView control.
        /// </summary>
        /// <param name="sender">
        /// Reference to the TextView control
        /// </param>
        /// <param name="e">
        /// Reference to the event arguments.
        /// </param>
        protected virtual void PasteClipboardOccurred (object sender, System.EventArgs e)
        {
            log.Debug("PasteClipboardOccurred");
            SetToContext();
        }

        /// <summary>
        /// Signal handler for InsertAtCursor on the TextView control.
        /// </summary>
        /// <param name="o">
        /// Reference to the TextView control
        /// </param>
        /// <param name="args">
        /// Reference to the event arguments.
        /// </param>
        protected virtual void InsertAtCursorOccurred (object o, Gtk.InsertAtCursorArgs args)
        {
            log.Debug("InsertAtCursorOccurred");
            SetToContext();
        }

        /// <summary>
        /// Signal handler for CutClipboard on the TextView control.
        /// </summary>
        /// <param name="sender">
        /// Reference to the TextView control
        /// </param>
        /// <param name="e">
        /// Reference to the event arguments.
        /// </param>
        protected virtual void CutClipboardOccurred (object sender, System.EventArgs e)
        {
            log.Debug("CutClipboardOccurred");
            SetToContext();
        }

        /// <summary>
        /// Signal handler for DeleteFromCursor on the TextView control.
        /// </summary>
        /// <param name="o">
        /// Reference to the TextView control
        /// </param>
        /// <param name="args">
        /// Reference to the event arguments.
        /// </param>
        protected virtual void DeleteFromCursorOccurred (object o, Gtk.DeleteFromCursorArgs args)
        {
            log.Debug("DeleteFromCursorOccurred");
            SetToContext();
        }

        /// <summary>
        /// Signal handler for BackspaceOccurred on the TextView control.
        /// </summary>
        /// <param name="sender">
        /// Reference to the TextView control
        /// </param>
        /// <param name="e">
        /// Reference to the event arguments.
        /// </param>
        protected virtual void BackspaceOccurred (object sender, System.EventArgs e)
        {
            log.Debug("BackspaceOccurred");
            SetToContext();
        }

        /// <summary>
        /// Signal handler for KeyReleaseEvent on the TextView control.
        /// </summary>
        /// <param name="o">
        /// Reference to the TextView control
        /// </param>
        /// <param name="args">
        /// Reference to the event arguments.
        /// </param>
        protected virtual void KeyReleaseEventOccurred (object o, Gtk.KeyReleaseEventArgs args)
        {
            log.Debug("KeyReleaseEventOccurred");
            SetToContext();
        }    
    }
}
