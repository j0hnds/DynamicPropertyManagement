
using System;
using System.Collections.Generic;
using DomainCore;
using Gtk;

namespace ControlWrappers
{
    
    /// <summary>
    /// Custom Gtk control which provides domain data binding to a combo box.
    /// </summary>
    [System.ComponentModel.ToolboxItem(true)]
    public partial class BoundComboBox : Gtk.Bin, BoundControl
    {
        /// <summary>
        /// A delegate to handle data binding duties.
        /// </summary>
        private BoundControlDelegate bcd;
        /// <summary>
        /// The index of the model column that holds the id of the selectable
        /// object.
        /// </summary>
        private const int ID_COLUMN = 1;
        /// <summary>
        /// The index of the model column that holds the label of the selectable
        /// object.
        /// </summary>
        private const int LABEL_COLUMN = 0;
        /// <summary>
        /// The model for the combo box.
        /// </summary>
        private ListStore listStore;
        /// <summary>
        /// The name of the collection to use to populate the combo box.
        /// </summary>
        private string collectionName;
        /// <summary>
        /// The name of the attribute on the collection items that identify the value to
        /// use for the label column of the model.
        /// </summary>
        private string labelAttributeName;
        /// <summary>
        /// The name of the attribute on the collection items that identify the id value
        /// to use for the id column of the model.
        /// </summary>
        private string valueAttributeName;

        /// <summary>
        /// Constructs a new BoundComboBox.
        /// </summary>
        public BoundComboBox()
        {
            this.Build();
            bcd = new BoundControlDelegate(this);
            bcd.ContextChanged += ContextChangeHandler;

            // Set up the list store for the combo box.
            listStore = new ListStore(GLib.GType.String, GLib.GType.Int64);

            cbBound.Model = listStore;
        }

        /// <summary>
        /// Event fired when the selected value of the combo box has changed.
        /// </summary>
        public event EventHandler Changed;

        /// <value>
        /// The name of the attribute on the collection items that identify the value to
        /// use for the label column of the model.
        /// </value>
        public string CollectionName
        {
            get { return collectionName; }
            set { collectionName = value; }
        }

        /// <value>
        /// The name of the attribute on the collection items that identify the value to
        /// use for the label column of the model.
        /// </value>
        public string LabelAttributeName
        {
            get { return labelAttributeName; }
            set { labelAttributeName = value; }
        }

        /// <value>
        /// The name of the attribute on the collection items that identify the id value
        /// to use for the id column of the model.
        /// </value>
        public string ValueAttributeName
        {
            get { return valueAttributeName; }
            set { valueAttributeName = value; }
        }

        /// <summary>
        /// Signal handler for context change.
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

        /// <summary>
        /// Signal handler for the selected value to change.
        /// </summary>
        /// <param name="sender">
        /// The combo box control
        /// </param>
        /// <param name="e">
        /// Referece to the event arguments.
        /// </param>
        protected virtual void BoundComboValueChanged (object sender, System.EventArgs e)
        {
            SetToContext();
            PropagateChangedEvent(e);
        }

        /// <summary>
        /// Helper method to notify subscribers to the Change event.
        /// </summary>
        /// <param name="e">
        /// Reference to the event arguments.
        /// </param>
        private void PropagateChangedEvent(System.EventArgs e)
        {
            EventHandler handler = Changed;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <value>
        /// Readonly property to retrieve the ID of the currently selected
        /// combo box item. -1L if nothing is selected.
        /// </value>
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

        /// <value>
        /// Readonly property to retrieve the label of the currently selected
        /// combo box item. <c>null</c> if nothing is selected.
        /// </value>
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
            object oId = bcd.DomainValue;
            if (oId != null)
            {
                long id = (long) oId;
    
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
