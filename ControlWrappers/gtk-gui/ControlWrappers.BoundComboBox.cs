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
    
    
    public partial class BoundComboBox {
        
        private Gtk.VBox vbox3;
        
        private Gtk.ComboBox cbBound;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget ControlWrappers.BoundComboBox
            Stetic.BinContainer.Attach(this);
            this.Name = "ControlWrappers.BoundComboBox";
            // Container child ControlWrappers.BoundComboBox.Gtk.Container+ContainerChild
            this.vbox3 = new Gtk.VBox();
            this.vbox3.Name = "vbox3";
            this.vbox3.Spacing = 6;
            // Container child vbox3.Gtk.Box+BoxChild
            this.cbBound = Gtk.ComboBox.NewText();
            this.cbBound.Name = "cbBound";
            this.vbox3.Add(this.cbBound);
            Gtk.Box.BoxChild w1 = ((Gtk.Box.BoxChild)(this.vbox3[this.cbBound]));
            w1.Position = 0;
            w1.Expand = false;
            w1.Fill = false;
            this.Add(this.vbox3);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.Hide();
            this.cbBound.Changed += new System.EventHandler(this.BoundComboValueChanged);
        }
    }
}
