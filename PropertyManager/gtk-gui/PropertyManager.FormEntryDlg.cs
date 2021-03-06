// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace PropertyManager {
    
    
    public partial class FormEntryDlg {
        
        private Gtk.Table table1;
        
        private Gtk.Label lblDescription;
        
        private Gtk.Label lblFormId;
        
        private ControlWrappers.BoundEntry txtDescription;
        
        private ControlWrappers.BoundEntry txtFormId;
        
        private Gtk.Button buttonCancel;
        
        private Gtk.Button buttonOk;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget PropertyManager.FormEntryDlg
            this.Name = "PropertyManager.FormEntryDlg";
            this.Title = Mono.Unix.Catalog.GetString("Form Details");
            this.WindowPosition = ((Gtk.WindowPosition)(4));
            this.HasSeparator = false;
            // Internal child PropertyManager.FormEntryDlg.VBox
            Gtk.VBox w1 = this.VBox;
            w1.Name = "dialog1_VBox";
            w1.BorderWidth = ((uint)(2));
            // Container child dialog1_VBox.Gtk.Box+BoxChild
            this.table1 = new Gtk.Table(((uint)(2)), ((uint)(2)), false);
            this.table1.Name = "table1";
            this.table1.RowSpacing = ((uint)(6));
            this.table1.ColumnSpacing = ((uint)(6));
            // Container child table1.Gtk.Table+TableChild
            this.lblDescription = new Gtk.Label();
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Xalign = 0F;
            this.lblDescription.LabelProp = Mono.Unix.Catalog.GetString("Description:");
            this.table1.Add(this.lblDescription);
            Gtk.Table.TableChild w2 = ((Gtk.Table.TableChild)(this.table1[this.lblDescription]));
            w2.TopAttach = ((uint)(1));
            w2.BottomAttach = ((uint)(2));
            w2.XOptions = ((Gtk.AttachOptions)(4));
            w2.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.lblFormId = new Gtk.Label();
            this.lblFormId.Name = "lblFormId";
            this.lblFormId.Xalign = 0F;
            this.lblFormId.LabelProp = Mono.Unix.Catalog.GetString("Form ID:");
            this.table1.Add(this.lblFormId);
            Gtk.Table.TableChild w3 = ((Gtk.Table.TableChild)(this.table1[this.lblFormId]));
            w3.XOptions = ((Gtk.AttachOptions)(4));
            w3.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.txtDescription = new ControlWrappers.BoundEntry();
            this.txtDescription.Events = ((Gdk.EventMask)(256));
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.IsEditable = true;
            this.txtDescription.ActivatesDefault = true;
            this.txtDescription.AttributeName = "Description";
            this.txtDescription.ContextName = "DialogContext";
            this.txtDescription.DomainName = "Form";
            this.table1.Add(this.txtDescription);
            Gtk.Table.TableChild w4 = ((Gtk.Table.TableChild)(this.table1[this.txtDescription]));
            w4.TopAttach = ((uint)(1));
            w4.BottomAttach = ((uint)(2));
            w4.LeftAttach = ((uint)(1));
            w4.RightAttach = ((uint)(2));
            w4.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.txtFormId = new ControlWrappers.BoundEntry();
            this.txtFormId.Events = ((Gdk.EventMask)(256));
            this.txtFormId.Name = "txtFormId";
            this.txtFormId.IsEditable = true;
            this.txtFormId.ActivatesDefault = true;
            this.txtFormId.AttributeName = "Id";
            this.txtFormId.ContextName = "DialogContext";
            this.txtFormId.DomainName = "Form";
            this.table1.Add(this.txtFormId);
            Gtk.Table.TableChild w5 = ((Gtk.Table.TableChild)(this.table1[this.txtFormId]));
            w5.LeftAttach = ((uint)(1));
            w5.RightAttach = ((uint)(2));
            w5.YOptions = ((Gtk.AttachOptions)(0));
            w1.Add(this.table1);
            Gtk.Box.BoxChild w6 = ((Gtk.Box.BoxChild)(w1[this.table1]));
            w6.Position = 0;
            w6.Expand = false;
            w6.Fill = false;
            // Internal child PropertyManager.FormEntryDlg.ActionArea
            Gtk.HButtonBox w7 = this.ActionArea;
            w7.Name = "dialog1_ActionArea";
            w7.Spacing = 6;
            w7.BorderWidth = ((uint)(5));
            w7.LayoutStyle = ((Gtk.ButtonBoxStyle)(4));
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.buttonCancel = new Gtk.Button();
            this.buttonCancel.CanDefault = true;
            this.buttonCancel.CanFocus = true;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseStock = true;
            this.buttonCancel.UseUnderline = true;
            this.buttonCancel.Label = "gtk-cancel";
            this.AddActionWidget(this.buttonCancel, -6);
            Gtk.ButtonBox.ButtonBoxChild w8 = ((Gtk.ButtonBox.ButtonBoxChild)(w7[this.buttonCancel]));
            w8.Expand = false;
            w8.Fill = false;
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.buttonOk = new Gtk.Button();
            this.buttonOk.CanDefault = true;
            this.buttonOk.CanFocus = true;
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.UseStock = true;
            this.buttonOk.UseUnderline = true;
            this.buttonOk.Label = "gtk-ok";
            this.AddActionWidget(this.buttonOk, -5);
            Gtk.ButtonBox.ButtonBoxChild w9 = ((Gtk.ButtonBox.ButtonBoxChild)(w7[this.buttonOk]));
            w9.Position = 1;
            w9.Expand = false;
            w9.Fill = false;
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.DefaultWidth = 400;
            this.DefaultHeight = 128;
            this.buttonOk.HasDefault = true;
            this.Show();
        }
    }
}
