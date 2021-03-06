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
    
    
    public partial class PropertyDefinitionEntryDlg {
        
        private Gtk.Table table1;
        
        private ControlWrappers.BoundComboBox cbDataType;
        
        private Gtk.Label lblCategory;
        
        private Gtk.Label lblDataType;
        
        private Gtk.Label lblDescription;
        
        private Gtk.Label lblName;
        
        private ControlWrappers.BoundTextView tvDescription;
        
        private ControlWrappers.BoundEntry txtCategories;
        
        private ControlWrappers.BoundEntry txtName;
        
        private Gtk.Button buttonCancel;
        
        private Gtk.Button buttonOk;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget PropertyManager.PropertyDefinitionEntryDlg
            this.Name = "PropertyManager.PropertyDefinitionEntryDlg";
            this.Title = Mono.Unix.Catalog.GetString("Property Definition Details");
            this.WindowPosition = ((Gtk.WindowPosition)(4));
            this.Resizable = false;
            this.AllowGrow = false;
            this.HasSeparator = false;
            // Internal child PropertyManager.PropertyDefinitionEntryDlg.VBox
            Gtk.VBox w1 = this.VBox;
            w1.Name = "dialog1_VBox";
            w1.BorderWidth = ((uint)(2));
            // Container child dialog1_VBox.Gtk.Box+BoxChild
            this.table1 = new Gtk.Table(((uint)(6)), ((uint)(2)), false);
            this.table1.Name = "table1";
            this.table1.RowSpacing = ((uint)(6));
            this.table1.ColumnSpacing = ((uint)(6));
            this.table1.BorderWidth = ((uint)(4));
            // Container child table1.Gtk.Table+TableChild
            this.cbDataType = new ControlWrappers.BoundComboBox();
            this.cbDataType.Events = ((Gdk.EventMask)(256));
            this.cbDataType.Name = "cbDataType";
            this.cbDataType.CollectionName = "DataTypes";
            this.cbDataType.LabelAttributeName = "Name";
            this.cbDataType.ValueAttributeName = "Id";
            this.cbDataType.AttributeName = "DataType";
            this.cbDataType.ContextName = "DialogContext";
            this.cbDataType.DomainName = "PropertyDefinition";
            this.table1.Add(this.cbDataType);
            Gtk.Table.TableChild w2 = ((Gtk.Table.TableChild)(this.table1[this.cbDataType]));
            w2.TopAttach = ((uint)(2));
            w2.BottomAttach = ((uint)(3));
            w2.LeftAttach = ((uint)(1));
            w2.RightAttach = ((uint)(2));
            w2.XOptions = ((Gtk.AttachOptions)(4));
            w2.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.lblCategory = new Gtk.Label();
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Xalign = 0F;
            this.lblCategory.LabelProp = Mono.Unix.Catalog.GetString("Category:");
            this.table1.Add(this.lblCategory);
            Gtk.Table.TableChild w3 = ((Gtk.Table.TableChild)(this.table1[this.lblCategory]));
            w3.XOptions = ((Gtk.AttachOptions)(4));
            w3.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.lblDataType = new Gtk.Label();
            this.lblDataType.Name = "lblDataType";
            this.lblDataType.Xalign = 0F;
            this.lblDataType.LabelProp = Mono.Unix.Catalog.GetString("Data Type:");
            this.table1.Add(this.lblDataType);
            Gtk.Table.TableChild w4 = ((Gtk.Table.TableChild)(this.table1[this.lblDataType]));
            w4.TopAttach = ((uint)(2));
            w4.BottomAttach = ((uint)(3));
            w4.XOptions = ((Gtk.AttachOptions)(4));
            w4.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.lblDescription = new Gtk.Label();
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Xalign = 0F;
            this.lblDescription.LabelProp = Mono.Unix.Catalog.GetString("Description:");
            this.table1.Add(this.lblDescription);
            Gtk.Table.TableChild w5 = ((Gtk.Table.TableChild)(this.table1[this.lblDescription]));
            w5.TopAttach = ((uint)(3));
            w5.BottomAttach = ((uint)(4));
            w5.RightAttach = ((uint)(2));
            w5.XOptions = ((Gtk.AttachOptions)(4));
            w5.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.lblName = new Gtk.Label();
            this.lblName.Name = "lblName";
            this.lblName.Xalign = 0F;
            this.lblName.LabelProp = Mono.Unix.Catalog.GetString("Name:");
            this.table1.Add(this.lblName);
            Gtk.Table.TableChild w6 = ((Gtk.Table.TableChild)(this.table1[this.lblName]));
            w6.TopAttach = ((uint)(1));
            w6.BottomAttach = ((uint)(2));
            w6.XOptions = ((Gtk.AttachOptions)(4));
            w6.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.tvDescription = new ControlWrappers.BoundTextView();
            this.tvDescription.Events = ((Gdk.EventMask)(256));
            this.tvDescription.Name = "tvDescription";
            this.tvDescription.AttributeName = "Description";
            this.tvDescription.ContextName = "DialogContext";
            this.tvDescription.DomainName = "PropertyDefinition";
            this.table1.Add(this.tvDescription);
            Gtk.Table.TableChild w7 = ((Gtk.Table.TableChild)(this.table1[this.tvDescription]));
            w7.TopAttach = ((uint)(4));
            w7.BottomAttach = ((uint)(6));
            w7.RightAttach = ((uint)(2));
            w7.XOptions = ((Gtk.AttachOptions)(4));
            w7.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.txtCategories = new ControlWrappers.BoundEntry();
            this.txtCategories.Events = ((Gdk.EventMask)(256));
            this.txtCategories.Name = "txtCategories";
            this.txtCategories.IsEditable = true;
            this.txtCategories.ActivatesDefault = true;
            this.txtCategories.AttributeName = "Category";
            this.txtCategories.ContextName = "DialogContext";
            this.txtCategories.DomainName = "PropertyDefinition";
            this.table1.Add(this.txtCategories);
            Gtk.Table.TableChild w8 = ((Gtk.Table.TableChild)(this.table1[this.txtCategories]));
            w8.LeftAttach = ((uint)(1));
            w8.RightAttach = ((uint)(2));
            w8.XOptions = ((Gtk.AttachOptions)(4));
            w8.YOptions = ((Gtk.AttachOptions)(4));
            // Container child table1.Gtk.Table+TableChild
            this.txtName = new ControlWrappers.BoundEntry();
            this.txtName.Events = ((Gdk.EventMask)(256));
            this.txtName.Name = "txtName";
            this.txtName.IsEditable = true;
            this.txtName.ActivatesDefault = true;
            this.txtName.AttributeName = "Name";
            this.txtName.ContextName = "DialogContext";
            this.txtName.DomainName = "PropertyDefinition";
            this.table1.Add(this.txtName);
            Gtk.Table.TableChild w9 = ((Gtk.Table.TableChild)(this.table1[this.txtName]));
            w9.TopAttach = ((uint)(1));
            w9.BottomAttach = ((uint)(2));
            w9.LeftAttach = ((uint)(1));
            w9.RightAttach = ((uint)(2));
            w9.XOptions = ((Gtk.AttachOptions)(4));
            w9.YOptions = ((Gtk.AttachOptions)(4));
            w1.Add(this.table1);
            Gtk.Box.BoxChild w10 = ((Gtk.Box.BoxChild)(w1[this.table1]));
            w10.Position = 0;
            w10.Expand = false;
            w10.Fill = false;
            // Internal child PropertyManager.PropertyDefinitionEntryDlg.ActionArea
            Gtk.HButtonBox w11 = this.ActionArea;
            w11.Name = "dialog1_ActionArea";
            w11.Spacing = 6;
            w11.BorderWidth = ((uint)(5));
            w11.LayoutStyle = ((Gtk.ButtonBoxStyle)(4));
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.buttonCancel = new Gtk.Button();
            this.buttonCancel.CanDefault = true;
            this.buttonCancel.CanFocus = true;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseStock = true;
            this.buttonCancel.UseUnderline = true;
            this.buttonCancel.Label = "gtk-cancel";
            this.AddActionWidget(this.buttonCancel, -6);
            Gtk.ButtonBox.ButtonBoxChild w12 = ((Gtk.ButtonBox.ButtonBoxChild)(w11[this.buttonCancel]));
            w12.Expand = false;
            w12.Fill = false;
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.buttonOk = new Gtk.Button();
            this.buttonOk.CanDefault = true;
            this.buttonOk.CanFocus = true;
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.UseStock = true;
            this.buttonOk.UseUnderline = true;
            this.buttonOk.Label = "gtk-ok";
            this.AddActionWidget(this.buttonOk, -5);
            Gtk.ButtonBox.ButtonBoxChild w13 = ((Gtk.ButtonBox.ButtonBoxChild)(w11[this.buttonOk]));
            w13.Position = 1;
            w13.Expand = false;
            w13.Fill = false;
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.DefaultWidth = 245;
            this.DefaultHeight = 264;
            this.buttonOk.HasDefault = true;
            this.Show();
        }
    }
}
