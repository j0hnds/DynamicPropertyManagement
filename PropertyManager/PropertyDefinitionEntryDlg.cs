
using System;
using ControlWrappers;
using DomainCore;
using DAOCore;

namespace PropertyManager
{
    
    
    public partial class PropertyDefinitionEntryDlg : DataBoundDialog // Gtk.Dialog
    {

        public PropertyDefinitionEntryDlg()
        {
            this.Build();

//            DomainDAO dao = DomainFactory.GetDAO("DataType");
//
//            new EntryBoundControl(this, txtCategories, "Category");
//            new EntryBoundControl(this, txtName, "Name");
//            new ComboBoxBoundControl(this, cbDataType, "DataType", dao.Get(), "Name", "Id");
//            new TextViewBoundControl(this, tvDescription, "Description");
        }

        protected override DataContext CreateDataContext ()
        {
            DataContext context = base.CreateDataContext();

            DomainDAO dao = DomainFactory.GetDAO("DataType");
            context.AddObject("DataTypes", dao.Get());            

            return context;
        }           
    }
}
