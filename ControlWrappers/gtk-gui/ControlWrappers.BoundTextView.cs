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
    
    
    public partial class BoundTextView {
        
        private Gtk.ScrolledWindow scrolledwindow1;
        
        private Gtk.TextView tvBound;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget ControlWrappers.BoundTextView
            Stetic.BinContainer.Attach(this);
            this.Name = "ControlWrappers.BoundTextView";
            // Container child ControlWrappers.BoundTextView.Gtk.Container+ContainerChild
            this.scrolledwindow1 = new Gtk.ScrolledWindow();
            this.scrolledwindow1.CanFocus = true;
            this.scrolledwindow1.Name = "scrolledwindow1";
            this.scrolledwindow1.ShadowType = ((Gtk.ShadowType)(1));
            // Container child scrolledwindow1.Gtk.Container+ContainerChild
            this.tvBound = new Gtk.TextView();
            this.tvBound.CanFocus = true;
            this.tvBound.Name = "tvBound";
            this.scrolledwindow1.Add(this.tvBound);
            this.Add(this.scrolledwindow1);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.Hide();
            this.tvBound.PasteClipboard += new System.EventHandler(this.PasteClipboardOccurred);
            this.tvBound.InsertAtCursor += new Gtk.InsertAtCursorHandler(this.InsertAtCursorOccurred);
            this.tvBound.CutClipboard += new System.EventHandler(this.CutClipboardOccurred);
            this.tvBound.DeleteFromCursor += new Gtk.DeleteFromCursorHandler(this.DeleteFromCursorOccurred);
            this.tvBound.Backspace += new System.EventHandler(this.BackspaceOccurred);
            this.tvBound.KeyReleaseEvent += new Gtk.KeyReleaseEventHandler(this.KeyReleaseEventOccurred);
        }
    }
}