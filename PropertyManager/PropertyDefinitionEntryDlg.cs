
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
