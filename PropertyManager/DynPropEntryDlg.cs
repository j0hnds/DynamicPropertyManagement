
using System;
using System.Collections.Generic;
using Gtk;
using DomainCore;
using ControlWrappers;

namespace PropertyManager
{
    
    
    public partial class DynPropEntryDlg : BoundDialog // Gtk.Dialog
    {
        
        public DynPropEntryDlg() 
        {
            this.Build();

            DomainDAO appDao = DomainFactory.GetDAO("Application");

            List<Domain> applications = appDao.Get();
            
            DomainDAO propDao = DomainFactory.GetDAO("PropertyDefinition");

            List<Domain>properties = propDao.Get();

            // Set up the binding controls
            new ComboBoxBoundControl(this, cbApplication, "ApplicationId", applications, "Name", "Id");
            new ComboBoxBoundControl(this, cbProperty, "PropertyId", properties, "Name", "Id");
            new EntryBoundControl(this, txtQualifier, "Qualifier");
            new EntryBoundControl(this, txtDefaultValue, "DefaultValue");

            // Set up the form combo-box
            SetUpForms();
        }

        private void SetUpForms()
        {
            DomainDAO dao = DomainFactory.GetDAO("Form");
            List<Domain> valueList = dao.Get();
            ListStore listStore = new ListStore(GLib.GType.String, GLib.GType.Int64);
            foreach (Domain domain in valueList)
            {
                object displayValue = domain.GetValue("Description");
                object valueValue = domain.GetValue("Id");

                listStore.AppendValues(displayValue, valueValue);
            }

            cbForm.Model = listStore;
        }

        protected virtual void FormSelectionChanged (object sender, System.EventArgs e)
        {
            TreeModel model = cbForm.Model;
            TreeIter iter = TreeIter.Zero;

            if (cbForm.GetActiveIter(out iter))
            {
                long formId = (long) model.GetValue(iter, 1);
                txtQualifier.Text = formId.ToString();
            }
        }
    }
}
