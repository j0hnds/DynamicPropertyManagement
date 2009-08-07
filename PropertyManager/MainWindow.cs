using System;
using System.Collections.Generic;
using DomainCore;
using Gtk;
using log4net;
using PropertyManager;
using STUtils;

public partial class MainWindow: Gtk.Window
{
    private MainTextViewControl mainTextViewCtl;
    private ILog log;
    private ApplicationListControl applicationListCtl;
    
    public MainWindow (): base (Gtk.WindowType.Toplevel)
    {
        Build ();

        log = LogManager.GetLogger(typeof(MainWindow));
        
        mainTextViewCtl = new MainTextViewControl(mainTextView);
    }

    public void SetUpApplication()
    {
        SetUpApplicationTree();
    }
    
    protected void OnDeleteEvent (object sender, DeleteEventArgs a)
    {
        Application.Quit ();
        a.RetVal = true;
    }

    protected virtual void on_file_quit (object sender, System.EventArgs e)
    {
        Application.Quit();
    }

    protected void SetUpApplicationTree()
    {
        applicationListCtl = new ApplicationListControl(tvApplications);
//        ListStore store = new ListStore(GLib.GType.String, GLib.GType.Int64, GLib.GType.String);
//        tvApplications.Model = store;
//        
//        // Set up the columns
//        TreeViewColumn tc = new TreeViewColumn();
//        tc.Title = "Name";
//        CellRenderer cell = new CellRendererText();
//        tc.PackStart(cell, true);
//        tc.AddAttribute(cell, "text", 0);
//        tvApplications.AppendColumn(tc);

        // Get the DAO for the applications
        DomainDAO dao = DomainFactory.GetDAO("Application");
        List<Domain> applications = dao.Get();
//        foreach (Domain app in applications)
//        {
////            string appName = (string) app.GetValue("Name");
//            string appName = DomainRenderer.Render(app, "Label");
//            store.AppendValues(appName, Convert.ToInt64(app.GetValue("Id")), app.GetType().Name);
//        }
        applicationListCtl.Populate(applications);
            
    }

    protected virtual void ApplicationCursorChanged (object sender, System.EventArgs e)
    {
        Domain domain = applicationListCtl.GetSelectedDomain();
        if (domain != null)
        {
            mainTextViewCtl.Render(DomainRenderer.Render(domain, "Summary"));
        }
//        // Get the selected item from the tree view
//        TreeModel model = null;
//        TreeIter iter;
//        
//        if (tvApplications.Selection.GetSelected(out model, out iter))
//        {
//            // Something was selected; what was it?
////            string name = (string) model.GetValue(iter, 0);
//            long id = (long) model.GetValue(iter, 1);
// 
//            DomainDAO dao = DomainFactory.GetDAO("Application");
//            Domain app = dao.GetObject(id);
//
//            mainTextViewCtl.Render(DomainRenderer.Render(app, "Summary"));
//        }

        HandleToolBarSensitivity();
    }

    private void HandleApplicationToolBarSensitivity()
    {
        bool somethingSelected = applicationListCtl.IsSelected;

        AddAction.Sensitive = somethingSelected;
        RemoveAction.Sensitive = somethingSelected;
        PropertiesAction.Sensitive = somethingSelected;
    }

    private void HandleNoopToolBarSensitivity()
    {
        AddAction.Sensitive = false;
        RemoveAction.Sensitive = false;
        PropertiesAction.Sensitive = false;
    }

    private void HandleToolBarSensitivity()
    {
        // Determine the current notebook page...
        int currentPage = nbSelections.CurrentPage;
        switch (currentPage)
        {
        case 0: // Applications page
            HandleApplicationToolBarSensitivity();
            break;

        default: // Any other page
            HandleNoopToolBarSensitivity();
            log.WarnFormat("Unknown notebook page selected: {0}", currentPage);
            break;
        }
    }

    protected virtual void AddItemAction (object sender, System.EventArgs e)
    {
        log.Debug("Add a new item to the current item");
    }

    protected virtual void RemoveItemAction (object sender, System.EventArgs e)
    {
        log.Debug("Remove the selected item.");
    }

    protected virtual void ItemPropertyAction (object sender, System.EventArgs e)
    {
        log.Debug("Edit the selected item.");
    }

    protected virtual void SwitchPageAction (object o, Gtk.SwitchPageArgs args)
    {
        HandleToolBarSensitivity();
    }


}