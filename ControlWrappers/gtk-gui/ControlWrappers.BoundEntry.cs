// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace ControlWrappers {
    
    
    public partial class BoundEntry {
        
        private Gtk.Entry txtEntry;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget ControlWrappers.BoundEntry
            Stetic.BinContainer.Attach(this);
            this.Name = "ControlWrappers.BoundEntry";
            // Container child ControlWrappers.BoundEntry.Gtk.Container+ContainerChild
            this.txtEntry = new Gtk.Entry();
            this.txtEntry.CanFocus = true;
            this.txtEntry.Name = "txtEntry";
            this.txtEntry.IsEditable = true;
            this.txtEntry.InvisibleChar = '●';
            this.Add(this.txtEntry);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.Hide();
            this.txtEntry.Changed += new System.EventHandler(this.TextEntryChanged);
        }
    }
}
