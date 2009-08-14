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
    
    
    public partial class TestDynPropDlg {
        
        private Gtk.HBox hbox1;
        
        private Gtk.VBox vbox2;
        
        private Gtk.Label lblPropertyInformation;
        
        private Gtk.ScrolledWindow scrolledwindow1;
        
        private Gtk.TextView tvPropertyInformation;
        
        private Gtk.VBox vbox3;
        
        private Gtk.Frame frmSimulatedEffectiveDate;
        
        private Gtk.Alignment GtkAlignment2;
        
        private Gtk.Table table1;
        
        private Gtk.HButtonBox buttonbox1;
        
        private Gtk.Button btnRevert;
        
        private Gtk.Button btnApply;
        
        private Gtk.Calendar calSimEffectiveDate;
        
        private Gtk.Label lblSimTime;
        
        private Gtk.Entry txtSimTime;
        
        private Gtk.Label GtkLabel2;
        
        private Gtk.HBox hbox2;
        
        private Gtk.Label lblEffectiveValue;
        
        private Gtk.Entry txtEffectiveValue;
        
        private Gtk.Button buttonCancel;
        
        private Gtk.Button buttonOk;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget PropertyManager.TestDynPropDlg
            this.Name = "PropertyManager.TestDynPropDlg";
            this.WindowPosition = ((Gtk.WindowPosition)(4));
            this.AllowShrink = true;
            this.HasSeparator = false;
            // Internal child PropertyManager.TestDynPropDlg.VBox
            Gtk.VBox w1 = this.VBox;
            w1.Name = "dialog1_VBox";
            w1.BorderWidth = ((uint)(2));
            // Container child dialog1_VBox.Gtk.Box+BoxChild
            this.hbox1 = new Gtk.HBox();
            this.hbox1.Name = "hbox1";
            this.hbox1.Spacing = 6;
            // Container child hbox1.Gtk.Box+BoxChild
            this.vbox2 = new Gtk.VBox();
            this.vbox2.Name = "vbox2";
            this.vbox2.Spacing = 6;
            // Container child vbox2.Gtk.Box+BoxChild
            this.lblPropertyInformation = new Gtk.Label();
            this.lblPropertyInformation.Name = "lblPropertyInformation";
            this.lblPropertyInformation.Xalign = 0F;
            this.lblPropertyInformation.LabelProp = Mono.Unix.Catalog.GetString("Property Information:");
            this.vbox2.Add(this.lblPropertyInformation);
            Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(this.vbox2[this.lblPropertyInformation]));
            w2.Position = 0;
            w2.Expand = false;
            w2.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.scrolledwindow1 = new Gtk.ScrolledWindow();
            this.scrolledwindow1.CanFocus = true;
            this.scrolledwindow1.Name = "scrolledwindow1";
            this.scrolledwindow1.ShadowType = ((Gtk.ShadowType)(1));
            // Container child scrolledwindow1.Gtk.Container+ContainerChild
            this.tvPropertyInformation = new Gtk.TextView();
            this.tvPropertyInformation.WidthRequest = 400;
            this.tvPropertyInformation.CanFocus = true;
            this.tvPropertyInformation.Name = "tvPropertyInformation";
            this.tvPropertyInformation.Editable = false;
            this.tvPropertyInformation.WrapMode = ((Gtk.WrapMode)(2));
            this.tvPropertyInformation.LeftMargin = 10;
            this.tvPropertyInformation.RightMargin = 10;
            this.scrolledwindow1.Add(this.tvPropertyInformation);
            this.vbox2.Add(this.scrolledwindow1);
            Gtk.Box.BoxChild w4 = ((Gtk.Box.BoxChild)(this.vbox2[this.scrolledwindow1]));
            w4.Position = 1;
            this.hbox1.Add(this.vbox2);
            Gtk.Box.BoxChild w5 = ((Gtk.Box.BoxChild)(this.hbox1[this.vbox2]));
            w5.Position = 0;
            w5.Expand = false;
            w5.Fill = false;
            // Container child hbox1.Gtk.Box+BoxChild
            this.vbox3 = new Gtk.VBox();
            this.vbox3.Name = "vbox3";
            this.vbox3.Spacing = 6;
            // Container child vbox3.Gtk.Box+BoxChild
            this.frmSimulatedEffectiveDate = new Gtk.Frame();
            this.frmSimulatedEffectiveDate.Name = "frmSimulatedEffectiveDate";
            // Container child frmSimulatedEffectiveDate.Gtk.Container+ContainerChild
            this.GtkAlignment2 = new Gtk.Alignment(0F, 0F, 1F, 1F);
            this.GtkAlignment2.Name = "GtkAlignment2";
            this.GtkAlignment2.LeftPadding = ((uint)(12));
            // Container child GtkAlignment2.Gtk.Container+ContainerChild
            this.table1 = new Gtk.Table(((uint)(3)), ((uint)(2)), false);
            this.table1.Name = "table1";
            this.table1.RowSpacing = ((uint)(6));
            this.table1.ColumnSpacing = ((uint)(6));
            this.table1.BorderWidth = ((uint)(6));
            // Container child table1.Gtk.Table+TableChild
            this.buttonbox1 = new Gtk.HButtonBox();
            this.buttonbox1.Name = "buttonbox1";
            // Container child buttonbox1.Gtk.ButtonBox+ButtonBoxChild
            this.btnRevert = new Gtk.Button();
            this.btnRevert.CanFocus = true;
            this.btnRevert.Name = "btnRevert";
            this.btnRevert.UseStock = true;
            this.btnRevert.UseUnderline = true;
            this.btnRevert.Label = "gtk-revert-to-saved";
            this.buttonbox1.Add(this.btnRevert);
            Gtk.ButtonBox.ButtonBoxChild w6 = ((Gtk.ButtonBox.ButtonBoxChild)(this.buttonbox1[this.btnRevert]));
            w6.Expand = false;
            w6.Fill = false;
            // Container child buttonbox1.Gtk.ButtonBox+ButtonBoxChild
            this.btnApply = new Gtk.Button();
            this.btnApply.CanFocus = true;
            this.btnApply.Name = "btnApply";
            this.btnApply.UseStock = true;
            this.btnApply.UseUnderline = true;
            this.btnApply.Label = "gtk-apply";
            this.buttonbox1.Add(this.btnApply);
            Gtk.ButtonBox.ButtonBoxChild w7 = ((Gtk.ButtonBox.ButtonBoxChild)(this.buttonbox1[this.btnApply]));
            w7.Position = 1;
            w7.Expand = false;
            w7.Fill = false;
            this.table1.Add(this.buttonbox1);
            Gtk.Table.TableChild w8 = ((Gtk.Table.TableChild)(this.table1[this.buttonbox1]));
            w8.TopAttach = ((uint)(2));
            w8.BottomAttach = ((uint)(3));
            w8.RightAttach = ((uint)(2));
            w8.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.calSimEffectiveDate = new Gtk.Calendar();
            this.calSimEffectiveDate.CanFocus = true;
            this.calSimEffectiveDate.Name = "calSimEffectiveDate";
            this.calSimEffectiveDate.DisplayOptions = ((Gtk.CalendarDisplayOptions)(35));
            this.table1.Add(this.calSimEffectiveDate);
            Gtk.Table.TableChild w9 = ((Gtk.Table.TableChild)(this.table1[this.calSimEffectiveDate]));
            w9.RightAttach = ((uint)(2));
            w9.XOptions = ((Gtk.AttachOptions)(4));
            w9.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.lblSimTime = new Gtk.Label();
            this.lblSimTime.Name = "lblSimTime";
            this.lblSimTime.Xalign = 0F;
            this.lblSimTime.LabelProp = Mono.Unix.Catalog.GetString("Time:");
            this.table1.Add(this.lblSimTime);
            Gtk.Table.TableChild w10 = ((Gtk.Table.TableChild)(this.table1[this.lblSimTime]));
            w10.TopAttach = ((uint)(1));
            w10.BottomAttach = ((uint)(2));
            w10.XOptions = ((Gtk.AttachOptions)(4));
            w10.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.txtSimTime = new Gtk.Entry();
            this.txtSimTime.CanFocus = true;
            this.txtSimTime.Name = "txtSimTime";
            this.txtSimTime.IsEditable = true;
            this.txtSimTime.InvisibleChar = '●';
            this.table1.Add(this.txtSimTime);
            Gtk.Table.TableChild w11 = ((Gtk.Table.TableChild)(this.table1[this.txtSimTime]));
            w11.TopAttach = ((uint)(1));
            w11.BottomAttach = ((uint)(2));
            w11.LeftAttach = ((uint)(1));
            w11.RightAttach = ((uint)(2));
            w11.XOptions = ((Gtk.AttachOptions)(4));
            w11.YOptions = ((Gtk.AttachOptions)(4));
            this.GtkAlignment2.Add(this.table1);
            this.frmSimulatedEffectiveDate.Add(this.GtkAlignment2);
            this.GtkLabel2 = new Gtk.Label();
            this.GtkLabel2.Name = "GtkLabel2";
            this.GtkLabel2.LabelProp = Mono.Unix.Catalog.GetString("<b>Simulated Effective Date</b>");
            this.GtkLabel2.UseMarkup = true;
            this.frmSimulatedEffectiveDate.LabelWidget = this.GtkLabel2;
            this.vbox3.Add(this.frmSimulatedEffectiveDate);
            Gtk.Box.BoxChild w14 = ((Gtk.Box.BoxChild)(this.vbox3[this.frmSimulatedEffectiveDate]));
            w14.Position = 0;
            w14.Expand = false;
            w14.Fill = false;
            // Container child vbox3.Gtk.Box+BoxChild
            this.hbox2 = new Gtk.HBox();
            this.hbox2.Name = "hbox2";
            this.hbox2.Spacing = 6;
            // Container child hbox2.Gtk.Box+BoxChild
            this.lblEffectiveValue = new Gtk.Label();
            this.lblEffectiveValue.Name = "lblEffectiveValue";
            this.lblEffectiveValue.LabelProp = Mono.Unix.Catalog.GetString("Effective Value:");
            this.hbox2.Add(this.lblEffectiveValue);
            Gtk.Box.BoxChild w15 = ((Gtk.Box.BoxChild)(this.hbox2[this.lblEffectiveValue]));
            w15.Position = 0;
            w15.Expand = false;
            w15.Fill = false;
            // Container child hbox2.Gtk.Box+BoxChild
            this.txtEffectiveValue = new Gtk.Entry();
            this.txtEffectiveValue.CanFocus = true;
            this.txtEffectiveValue.Name = "txtEffectiveValue";
            this.txtEffectiveValue.IsEditable = false;
            this.txtEffectiveValue.InvisibleChar = '●';
            this.hbox2.Add(this.txtEffectiveValue);
            Gtk.Box.BoxChild w16 = ((Gtk.Box.BoxChild)(this.hbox2[this.txtEffectiveValue]));
            w16.Position = 1;
            this.vbox3.Add(this.hbox2);
            Gtk.Box.BoxChild w17 = ((Gtk.Box.BoxChild)(this.vbox3[this.hbox2]));
            w17.Position = 1;
            w17.Expand = false;
            w17.Fill = false;
            this.hbox1.Add(this.vbox3);
            Gtk.Box.BoxChild w18 = ((Gtk.Box.BoxChild)(this.hbox1[this.vbox3]));
            w18.Position = 1;
            w18.Expand = false;
            w18.Fill = false;
            w1.Add(this.hbox1);
            Gtk.Box.BoxChild w19 = ((Gtk.Box.BoxChild)(w1[this.hbox1]));
            w19.Position = 0;
            w19.Expand = false;
            w19.Fill = false;
            // Internal child PropertyManager.TestDynPropDlg.ActionArea
            Gtk.HButtonBox w20 = this.ActionArea;
            w20.Name = "dialog1_ActionArea";
            w20.Spacing = 6;
            w20.BorderWidth = ((uint)(5));
            w20.LayoutStyle = ((Gtk.ButtonBoxStyle)(4));
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.buttonCancel = new Gtk.Button();
            this.buttonCancel.CanDefault = true;
            this.buttonCancel.CanFocus = true;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseStock = true;
            this.buttonCancel.UseUnderline = true;
            this.buttonCancel.Label = "gtk-cancel";
            this.AddActionWidget(this.buttonCancel, -6);
            Gtk.ButtonBox.ButtonBoxChild w21 = ((Gtk.ButtonBox.ButtonBoxChild)(w20[this.buttonCancel]));
            w21.Expand = false;
            w21.Fill = false;
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.buttonOk = new Gtk.Button();
            this.buttonOk.CanDefault = true;
            this.buttonOk.CanFocus = true;
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.UseStock = true;
            this.buttonOk.UseUnderline = true;
            this.buttonOk.Label = "gtk-ok";
            this.AddActionWidget(this.buttonOk, -5);
            Gtk.ButtonBox.ButtonBoxChild w22 = ((Gtk.ButtonBox.ButtonBoxChild)(w20[this.buttonOk]));
            w22.Position = 1;
            w22.Expand = false;
            w22.Fill = false;
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.DefaultWidth = 702;
            this.DefaultHeight = 375;
            this.Show();
            this.btnRevert.Clicked += new System.EventHandler(this.RevertSimulatedDateClicked);
            this.btnApply.Clicked += new System.EventHandler(this.ApplySimulatedDateClicked);
        }
    }
}
