
using System;
using log4net;

namespace ControlWrappers
{
    
    
    [System.ComponentModel.ToolboxItem(true)]
    public partial class BoundTextView : Gtk.Bin, BoundControl
    {
        private BoundControlDelegate bcd;
        private ILog log;
        
        public BoundTextView()
        {
            this.Build();
            log = LogManager.GetLogger(GetType().Name);
            bcd = new BoundControlDelegate(this);
            bcd.ContextChanged += ContextChangeHandler;
            WrapMode = Gtk.WrapMode.Word;
        }

        public Gtk.WrapMode WrapMode
        {
            get { return tvBound.WrapMode; }
            set { tvBound.WrapMode = value; }
        }
        
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

        protected virtual void PasteClipboardOccurred (object sender, System.EventArgs e)
        {
            log.Debug("PasteClipboardOccurred");
            SetToContext();
        }

        protected virtual void InsertAtCursorOccurred (object o, Gtk.InsertAtCursorArgs args)
        {
            log.Debug("InsertAtCursorOccurred");
            SetToContext();
        }

        protected virtual void CutClipboardOccurred (object sender, System.EventArgs e)
        {
            log.Debug("CutClipboardOccurred");
            SetToContext();
        }

        protected virtual void DeleteFromCursorOccurred (object o, Gtk.DeleteFromCursorArgs args)
        {
            log.Debug("DeleteFromCursorOccurred");
            SetToContext();
        }

        protected virtual void BackspaceOccurred (object sender, System.EventArgs e)
        {
            log.Debug("BackspaceOccurred");
            SetToContext();
        }

        protected virtual void KeyReleaseEventOccurred (object o, Gtk.KeyReleaseEventArgs args)
        {
            log.Debug("KeyReleaseEventOccurred");
            SetToContext();
        }    
    }
}
