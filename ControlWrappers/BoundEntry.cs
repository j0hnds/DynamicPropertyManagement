
using System;
using Gtk;

namespace ControlWrappers
{
    
    
    [System.ComponentModel.ToolboxItem(true)]
    public partial class BoundEntry : Bin, BoundControl
    {
        private string attributeName;
        
        public BoundEntry()
        {
            this.Build();
        }

        private Entry Entry
        {
            get { return Child as Entry; }
        }

        public string AttributeName
        {
            get { return attributeName; }
            set { attributeName = value; }
        }

        public void SetFromDomain(DomainCore.Domain domain)
        {
            object val = domain.GetValue(attributeName);
            if (val != null)
            {
                Entry.Text = val.ToString();
            }
            else
            {
                Entry.Text = "";
            }
        }

        public void SetToDomain(DomainCore.Domain domain)
        {
            string tValue = Entry.Text;
            if (tValue.Length > 0)
            {
                domain.SetValue(attributeName, tValue);
            }
            else
            {
                domain.SetValue(attributeName, null);
            }
        }
    }
}
