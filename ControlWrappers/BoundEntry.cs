
using System;
using Gtk;

namespace ControlWrappers
{
    
    /// <summary>
    /// A custom Gtk control to provide data binding to an Entry control.
    /// </summary>
    [System.ComponentModel.ToolboxItem(true)]
    public partial class BoundEntry : Bin, BoundControl
    {
        /// <summary>
        /// A reference to a delegate to handle the data binding chores.
        /// </summary>
        private BoundControlDelegate bcd;

        /// <summary>
        /// This event is fired when the value of the entry has changed.
        /// </summary>
        public event EventHandler Changed;

        /// <summary>
        /// Constructs a new BoundEntry control.
        /// </summary>
        public BoundEntry()
        {
            this.Build();
            bcd = new BoundControlDelegate(this);
            bcd.ContextChanged += ContextChangeHandler;
        }

        /// <summary>
        /// Signal handler to deal with ContextChanged events from the BoundControlDelegate.
        /// </summary>
        /// <param name="contextName">
        /// The name of the data context that changed.
        /// </param>
        /// <param name="itemName">
        /// The name of the item on the data context that changed.
        /// </param>
        private void ContextChangeHandler(string contextName, string itemName)
        {
            SetFromContext();
        }

        /// <value>
        /// <c>true</c> if the contents of the control can be modified.
        /// </value>
        public bool IsEditable
        {
            get { return txtEntry.IsEditable; }
            set { txtEntry.IsEditable = value; }
        }

        #region BoundControl implementation
        public string AttributeName
        {
            get { return bcd.AttributeName; }
            set { bcd.AttributeName = value; }
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

        public void SetFromContext()
        {
            object val = bcd.DomainValue;
            if (val != null)
            {
                txtEntry.Text = val.ToString();
            }
            else
            {
                txtEntry.Text = "";
            }
        }

        public void SetToContext()
        {
            string tValue = txtEntry.Text;
            if (tValue.Length > 0)
            {
                bcd.DomainValue = tValue;
            }
            else
            {
                bcd.DomainValue = null;
            }
        }

        public void ConnectControl ()
        {
            bcd.ConnectControl();
        }

        #endregion

        /// <summary>
        /// Signal handler for Entry value changes.
        /// </summary>
        /// <param name="sender">
        /// Reference to the Entry control.
        /// </param>
        /// <param name="e">
        /// Reference to the event arguments.
        /// </param>
        protected virtual void TextEntryChanged (object sender, System.EventArgs e)
        {
            SetToContext();
            NotifyEntryValueChanged(e);
        }

        /// <summary>
        /// Helper method to notify the subscribers to the value changed event that
        /// a change has occurred.
        /// </summary>
        /// <param name="e">
        /// Reference to the event arguments.
        /// </param>
        private void NotifyEntryValueChanged(EventArgs e)
        {
            EventHandler handler = Changed;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <value>
        /// The textual value contained in the control.
        /// </value>
        public string Text
        {
            get
            {
                return txtEntry.Text;
            }

            set
            {
                txtEntry.Text = value;
                SetToContext();
            }
        }
    }
}
