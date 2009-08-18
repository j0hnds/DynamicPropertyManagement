
using System;
using Gtk;

namespace ControlWrappers
{
    
    
    [System.ComponentModel.ToolboxItem(true)]
    public partial class BoundEntry : Bin, BoundControl
    {
        private BoundControlDelegate bcd;

        public event EventHandler Changed;
        
        public BoundEntry()
        {
            this.Build();
            bcd = new BoundControlDelegate(this);
            bcd.ContextChanged += ContextChangeHandler;
        }

        private void ContextChangeHandler(string contextName, string itemName)
        {
            SetFromContext();
        }

        public bool IsEditable
        {
            get { return txtEntry.IsEditable; }
            set { txtEntry.IsEditable = value; }
        }

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

        #region BoundControl implementation
        public void ConnectControl ()
        {
            bcd.ConnectControl();
        }

        #endregion

        protected virtual void TextEntryChanged (object sender, System.EventArgs e)
        {
            SetToContext();
            NotifyEntryValueChanged(e);
        }

        private void NotifyEntryValueChanged(EventArgs e)
        {
            EventHandler handler = Changed;

            if (handler != null)
            {
                handler(this, e);
            }
        }

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
