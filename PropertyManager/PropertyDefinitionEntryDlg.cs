
using System;
using ControlWrappers;
using DomainCore;
using DAOCore;

namespace PropertyManager
{
    
    /// <summary>
    /// Dialog class to support addition/modification of Property Definition objects.
    /// </summary>
    public partial class PropertyDefinitionEntryDlg : DataBoundDialog
    {

        /// <summary>
        /// Constructs a new PropertyDefinitionEntryDlg.
        /// </summary>
        public PropertyDefinitionEntryDlg()
        {
            this.Build();

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
