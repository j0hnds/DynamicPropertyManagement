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
    
    
    public partial class BoundTime {
        
        private Gtk.HBox hbox1;
        
        private Gtk.Label lblHour;
        
        private Gtk.SpinButton sbHour;
        
        private Gtk.Label lblMinute;
        
        private Gtk.SpinButton sbMinute;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget ControlWrappers.BoundTime
            Stetic.BinContainer.Attach(this);
            this.Name = "ControlWrappers.BoundTime";
            // Container child ControlWrappers.BoundTime.Gtk.Container+ContainerChild
            this.hbox1 = new Gtk.HBox();
            this.hbox1.Name = "hbox1";
            this.hbox1.Spacing = 6;
            // Container child hbox1.Gtk.Box+BoxChild
            this.lblHour = new Gtk.Label();
            this.lblHour.Name = "lblHour";
            this.lblHour.LabelProp = Mono.Unix.Catalog.GetString("H:");
            this.hbox1.Add(this.lblHour);
            Gtk.Box.BoxChild w1 = ((Gtk.Box.BoxChild)(this.hbox1[this.lblHour]));
            w1.Position = 0;
            w1.Expand = false;
            w1.Fill = false;
            // Container child hbox1.Gtk.Box+BoxChild
            this.sbHour = new Gtk.SpinButton(0, 23, 1);
            this.sbHour.CanFocus = true;
            this.sbHour.Name = "sbHour";
            this.sbHour.Adjustment.PageIncrement = 10;
            this.sbHour.ClimbRate = 1;
            this.sbHour.Numeric = true;
            this.hbox1.Add(this.sbHour);
            Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(this.hbox1[this.sbHour]));
            w2.Position = 1;
            w2.Expand = false;
            w2.Fill = false;
            // Container child hbox1.Gtk.Box+BoxChild
            this.lblMinute = new Gtk.Label();
            this.lblMinute.Name = "lblMinute";
            this.lblMinute.LabelProp = Mono.Unix.Catalog.GetString("M:");
            this.hbox1.Add(this.lblMinute);
            Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(this.hbox1[this.lblMinute]));
            w3.Position = 2;
            w3.Expand = false;
            w3.Fill = false;
            // Container child hbox1.Gtk.Box+BoxChild
            this.sbMinute = new Gtk.SpinButton(0, 59, 1);
            this.sbMinute.CanFocus = true;
            this.sbMinute.Name = "sbMinute";
            this.sbMinute.Adjustment.PageIncrement = 10;
            this.sbMinute.ClimbRate = 1;
            this.sbMinute.Numeric = true;
            this.hbox1.Add(this.sbMinute);
            Gtk.Box.BoxChild w4 = ((Gtk.Box.BoxChild)(this.hbox1[this.sbMinute]));
            w4.Position = 3;
            w4.Expand = false;
            w4.Fill = false;
            this.Add(this.hbox1);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.Hide();
        }
    }
}
