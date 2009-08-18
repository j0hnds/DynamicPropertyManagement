
using System;
using System.Collections.Generic;
using DomainCore;
using Gtk;

namespace ControlWrappers
{
    
    
    [System.ComponentModel.ToolboxItem(true)]
    public partial class BoundComboBox : Gtk.Bin, BoundControl
    {
        private BoundControlDelegate bcd;
        private const int ID_COLUMN = 1;
        private const int LABEL_COLUMN = 0;
        private ListStore listStore;
        private string collectionName;
        private string labelAttributeName;
        private string valueAttributeName;
        
        public BoundComboBox()
        {
            this.Build();
            bcd = new BoundControlDelegate(this);
            bcd.ContextChanged += ContextChangeHandler;

            // Set up the list store for the combo box.
            listStore = new ListStore(GLib.GType.String, GLib.GType.Int64);

            cbBound.Model = listStore;
        }

        public event EventHandler Changed;

        public string CollectionName
        {
            get { return collectionName; }
            set { collectionName = value; }
        }

        public string LabelAttributeName
        {
            get { return labelAttributeName; }
            set { labelAttributeName = value; }
        }

        public string ValueAttributeName
        {
            get { return valueAttributeName; }
            set { valueAttributeName = value; }
        }
        
        private void ContextChangeHandler(string contextName, string itemName)
        {
            SetFromContext();
        }

        protected virtual void BoundComboValueChanged (object sender, System.EventArgs e)
        {
            SetToContext();
            PropagateChangedEvent(e);
        }

        private void PropagateChangedEvent(System.EventArgs e)
        {
            EventHandler handler = Changed;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        public long ActiveId
        {
            get 
            {
                long id = -1L;
                TreeIter iter = TreeIter.Zero;
                if (cbBound.GetActiveIter(out iter))
                {
                    id = (long) listStore.GetValue(iter, ID_COLUMN);
                }
    
                return id;
            }
        }

        public string ActiveLabel
        {
            get 
            {
                string label = null;
                
                TreeIter iter = TreeIter.Zero;
                if (cbBound.GetActiveIter(out iter))
                {
                    label = (string) listStore.GetValue(iter, LABEL_COLUMN);
                }
    
                return label;
            }
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

            // When the control is connected, we need to load up the
            // liststore for the combo box.
            PopulateComboBox();
        }

        private void PopulateComboBox()
        {
            DataContext ctx = bcd.Context;
            if (ctx != null)
            {
                List<Domain> lst = ctx.GetCollection(collectionName);
                foreach (Domain domain in lst)
                {
                    string label = (string) domain.GetValue(labelAttributeName);
                    long id = (long) domain.GetValue(valueAttributeName);

                    listStore.AppendValues(label, id);
                }
            }
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
            long id = (long) bcd.DomainValue;

            // Match this value to the ID value in the list store
            TreeIter iter = TreeIter.Zero;
            bool more = cbBound.Model.GetIterFirst(out iter);
            while (more)
            {
                long cbId = (long) cbBound.Model.GetValue(iter, ID_COLUMN);
                if (cbId == id)
                {
                    cbBound.SetActiveIter(iter);
                    break;
                }

                more = cbBound.Model.IterNext(ref iter);
            }
        }      
        
        public void SetToContext ()
        {
            TreeIter iter = TreeIter.Zero;
            long id = -1L;
            
            if (cbBound.GetActiveIter(out iter))
            {
                // Get the value from the model
                id = (long) cbBound.Model.GetValue(iter, ID_COLUMN);
            }

            bcd.DomainValue = id;
        }
        #endregion

    }
}
