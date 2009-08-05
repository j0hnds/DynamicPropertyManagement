// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------



public partial class MainWindow {
    
    private Gtk.UIManager UIManager;
    
    private Gtk.Action FileAction;
    
    private Gtk.Action QuitAction;
    
    private Gtk.VBox vbox1;
    
    private Gtk.MenuBar menubar1;
    
    private Gtk.HPaned hpaned1;
    
    private Gtk.Notebook notebook1;
    
    private Gtk.ScrolledWindow scrolledwindow3;
    
    private Gtk.TreeView tvApplications;
    
    private Gtk.Label label1;
    
    private Gtk.ScrolledWindow scrolledwindow4;
    
    private Gtk.TreeView tvForms;
    
    private Gtk.Label label2;
    
    private Gtk.ScrolledWindow scrolledwindow5;
    
    private Gtk.TreeView tvPropertyDefinitions;
    
    private Gtk.Label label3;
    
    private Gtk.ScrolledWindow scrolledwindow6;
    
    private Gtk.TreeView tvDynamicProperties;
    
    private Gtk.Label label4;
    
    private Gtk.ScrolledWindow scrolledwindow2;
    
    private Gtk.TextView mainTextView;
    
    private Gtk.Statusbar statusbar1;
    
    protected virtual void Build() {
        Stetic.Gui.Initialize(this);
        // Widget MainWindow
        this.UIManager = new Gtk.UIManager();
        Gtk.ActionGroup w1 = new Gtk.ActionGroup("Default");
        this.FileAction = new Gtk.Action("FileAction", Mono.Unix.Catalog.GetString("_File"), null, null);
        this.FileAction.ShortLabel = Mono.Unix.Catalog.GetString("_File");
        w1.Add(this.FileAction, null);
        this.QuitAction = new Gtk.Action("QuitAction", Mono.Unix.Catalog.GetString("_Quit"), null, "gtk-quit");
        this.QuitAction.ShortLabel = Mono.Unix.Catalog.GetString("_Quit");
        w1.Add(this.QuitAction, null);
        this.UIManager.InsertActionGroup(w1, 0);
        this.AddAccelGroup(this.UIManager.AccelGroup);
        this.WidthRequest = 640;
        this.HeightRequest = 480;
        this.Name = "MainWindow";
        this.Title = Mono.Unix.Catalog.GetString("Dynamic Property Manager");
        this.WindowPosition = ((Gtk.WindowPosition)(4));
        // Container child MainWindow.Gtk.Container+ContainerChild
        this.vbox1 = new Gtk.VBox();
        this.vbox1.Name = "vbox1";
        this.vbox1.Spacing = 6;
        // Container child vbox1.Gtk.Box+BoxChild
        this.UIManager.AddUiFromString("<ui><menubar name='menubar1'><menu name='FileAction' action='FileAction'><menuitem name='QuitAction' action='QuitAction'/></menu></menubar></ui>");
        this.menubar1 = ((Gtk.MenuBar)(this.UIManager.GetWidget("/menubar1")));
        this.menubar1.Name = "menubar1";
        this.vbox1.Add(this.menubar1);
        Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(this.vbox1[this.menubar1]));
        w2.Position = 0;
        w2.Expand = false;
        w2.Fill = false;
        // Container child vbox1.Gtk.Box+BoxChild
        this.hpaned1 = new Gtk.HPaned();
        this.hpaned1.CanFocus = true;
        this.hpaned1.Name = "hpaned1";
        this.hpaned1.Position = 250;
        // Container child hpaned1.Gtk.Paned+PanedChild
        this.notebook1 = new Gtk.Notebook();
        this.notebook1.CanFocus = true;
        this.notebook1.Events = ((Gdk.EventMask)(1082116));
        this.notebook1.Name = "notebook1";
        this.notebook1.CurrentPage = 0;
        this.notebook1.Scrollable = true;
        // Container child notebook1.Gtk.Notebook+NotebookChild
        this.scrolledwindow3 = new Gtk.ScrolledWindow();
        this.scrolledwindow3.CanFocus = true;
        this.scrolledwindow3.Events = ((Gdk.EventMask)(1082116));
        this.scrolledwindow3.Name = "scrolledwindow3";
        this.scrolledwindow3.ShadowType = ((Gtk.ShadowType)(1));
        // Container child scrolledwindow3.Gtk.Container+ContainerChild
        this.tvApplications = new Gtk.TreeView();
        this.tvApplications.CanFocus = true;
        this.tvApplications.Events = ((Gdk.EventMask)(33540));
        this.tvApplications.Name = "tvApplications";
        this.tvApplications.EnableSearch = false;
        this.tvApplications.HeadersVisible = false;
        this.scrolledwindow3.Add(this.tvApplications);
        this.notebook1.Add(this.scrolledwindow3);
        // Notebook tab
        this.label1 = new Gtk.Label();
        this.label1.Name = "label1";
        this.label1.LabelProp = Mono.Unix.Catalog.GetString("Application");
        this.notebook1.SetTabLabel(this.scrolledwindow3, this.label1);
        this.label1.ShowAll();
        // Container child notebook1.Gtk.Notebook+NotebookChild
        this.scrolledwindow4 = new Gtk.ScrolledWindow();
        this.scrolledwindow4.CanFocus = true;
        this.scrolledwindow4.Name = "scrolledwindow4";
        this.scrolledwindow4.ShadowType = ((Gtk.ShadowType)(1));
        // Container child scrolledwindow4.Gtk.Container+ContainerChild
        this.tvForms = new Gtk.TreeView();
        this.tvForms.CanFocus = true;
        this.tvForms.Name = "tvForms";
        this.scrolledwindow4.Add(this.tvForms);
        this.notebook1.Add(this.scrolledwindow4);
        Gtk.Notebook.NotebookChild w6 = ((Gtk.Notebook.NotebookChild)(this.notebook1[this.scrolledwindow4]));
        w6.Position = 1;
        // Notebook tab
        this.label2 = new Gtk.Label();
        this.label2.Name = "label2";
        this.label2.LabelProp = Mono.Unix.Catalog.GetString("Form");
        this.notebook1.SetTabLabel(this.scrolledwindow4, this.label2);
        this.label2.ShowAll();
        // Container child notebook1.Gtk.Notebook+NotebookChild
        this.scrolledwindow5 = new Gtk.ScrolledWindow();
        this.scrolledwindow5.CanFocus = true;
        this.scrolledwindow5.Name = "scrolledwindow5";
        this.scrolledwindow5.ShadowType = ((Gtk.ShadowType)(1));
        // Container child scrolledwindow5.Gtk.Container+ContainerChild
        this.tvPropertyDefinitions = new Gtk.TreeView();
        this.tvPropertyDefinitions.CanFocus = true;
        this.tvPropertyDefinitions.Name = "tvPropertyDefinitions";
        this.scrolledwindow5.Add(this.tvPropertyDefinitions);
        this.notebook1.Add(this.scrolledwindow5);
        Gtk.Notebook.NotebookChild w8 = ((Gtk.Notebook.NotebookChild)(this.notebook1[this.scrolledwindow5]));
        w8.Position = 2;
        // Notebook tab
        this.label3 = new Gtk.Label();
        this.label3.Name = "label3";
        this.label3.LabelProp = Mono.Unix.Catalog.GetString("Property Definitions");
        this.notebook1.SetTabLabel(this.scrolledwindow5, this.label3);
        this.label3.ShowAll();
        // Container child notebook1.Gtk.Notebook+NotebookChild
        this.scrolledwindow6 = new Gtk.ScrolledWindow();
        this.scrolledwindow6.CanFocus = true;
        this.scrolledwindow6.Name = "scrolledwindow6";
        this.scrolledwindow6.ShadowType = ((Gtk.ShadowType)(1));
        // Container child scrolledwindow6.Gtk.Container+ContainerChild
        this.tvDynamicProperties = new Gtk.TreeView();
        this.tvDynamicProperties.CanFocus = true;
        this.tvDynamicProperties.Name = "tvDynamicProperties";
        this.scrolledwindow6.Add(this.tvDynamicProperties);
        this.notebook1.Add(this.scrolledwindow6);
        Gtk.Notebook.NotebookChild w10 = ((Gtk.Notebook.NotebookChild)(this.notebook1[this.scrolledwindow6]));
        w10.Position = 3;
        // Notebook tab
        this.label4 = new Gtk.Label();
        this.label4.Name = "label4";
        this.label4.LabelProp = Mono.Unix.Catalog.GetString("Dynamic Properties");
        this.notebook1.SetTabLabel(this.scrolledwindow6, this.label4);
        this.label4.ShowAll();
        this.hpaned1.Add(this.notebook1);
        Gtk.Paned.PanedChild w11 = ((Gtk.Paned.PanedChild)(this.hpaned1[this.notebook1]));
        w11.Resize = false;
        // Container child hpaned1.Gtk.Paned+PanedChild
        this.scrolledwindow2 = new Gtk.ScrolledWindow();
        this.scrolledwindow2.CanFocus = true;
        this.scrolledwindow2.Name = "scrolledwindow2";
        this.scrolledwindow2.ShadowType = ((Gtk.ShadowType)(1));
        // Container child scrolledwindow2.Gtk.Container+ContainerChild
        this.mainTextView = new Gtk.TextView();
        this.mainTextView.CanFocus = true;
        this.mainTextView.Name = "mainTextView";
        this.mainTextView.Editable = false;
        this.mainTextView.WrapMode = ((Gtk.WrapMode)(2));
        this.mainTextView.LeftMargin = 5;
        this.mainTextView.RightMargin = 5;
        this.scrolledwindow2.Add(this.mainTextView);
        this.hpaned1.Add(this.scrolledwindow2);
        this.vbox1.Add(this.hpaned1);
        Gtk.Box.BoxChild w14 = ((Gtk.Box.BoxChild)(this.vbox1[this.hpaned1]));
        w14.Position = 1;
        // Container child vbox1.Gtk.Box+BoxChild
        this.statusbar1 = new Gtk.Statusbar();
        this.statusbar1.Name = "statusbar1";
        this.statusbar1.Spacing = 6;
        this.vbox1.Add(this.statusbar1);
        Gtk.Box.BoxChild w15 = ((Gtk.Box.BoxChild)(this.vbox1[this.statusbar1]));
        w15.Position = 2;
        w15.Expand = false;
        w15.Fill = false;
        this.Add(this.vbox1);
        if ((this.Child != null)) {
            this.Child.ShowAll();
        }
        this.DefaultWidth = 640;
        this.DefaultHeight = 480;
        this.Show();
        this.DeleteEvent += new Gtk.DeleteEventHandler(this.OnDeleteEvent);
        this.QuitAction.Activated += new System.EventHandler(this.on_file_quit);
        this.tvApplications.CursorChanged += new System.EventHandler(this.ApplicationCursorChanged);
    }
}
