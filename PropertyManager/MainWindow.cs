using System;
using System.Collections.Generic;
using DomainCore;
using Gtk;
using log4net;
using Antlr.StringTemplate;
using PropertyManager;

public partial class MainWindow: Gtk.Window
{
    private MainTextViewControl mainTextViewCtl;
    private ILog log;
    
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
        ListStore store = new ListStore(GLib.GType.String, GLib.GType.Int64);
        tvApplications.Model = store;
        
        // Set up the columns
        TreeViewColumn tc = new TreeViewColumn();
        tc.Title = "Name";
        CellRenderer cell = new CellRendererText();
        tc.PackStart(cell, true);
        tc.AddAttribute(cell, "text", 0);
        tvApplications.AppendColumn(tc);

        // Get the DAO for the applications
        DomainDAO dao = DomainFactory.GetDAO("Application");
        List<Domain> applications = dao.Get();
        foreach (Domain app in applications)
        {
            string appName = (string) app.GetValue("Name");
            store.AppendValues(appName, Convert.ToInt64(app.GetValue("Id")));
        }
            
    }

    protected virtual void ApplicationCursorChanged (object sender, System.EventArgs e)
    {
        // Get the selected item from the tree view
        TreeModel model = null;
        TreeIter iter;
        
        if (tvApplications.Selection.GetSelected(out model, out iter))
        {
            // Something was selected; what was it?
            string name = (string) model.GetValue(iter, 0);
            log.DebugFormat("Name = {0}", name);
            long id = (long) model.GetValue(iter, 1);
            log.DebugFormat("Name = {0}", id);

            DomainDAO dao = DomainFactory.GetDAO("Application");
            Domain app = dao.GetObject(id);

            StringTemplateGroup group = new StringTemplateGroup("myGroup", "/home/siehd/Projects/DynamicPropertyManagement/PropertyManager/Templates");
            StringTemplate tpt = group.GetInstanceOf("ApplicationSummary");
            tpt.SetAttribute("app", app.AttributeValues);
            string summary = tpt.ToString();
            log.DebugFormat("Populated Template: [{0}]", summary);
            mainTextViewCtl.Render(summary);
        }
    }


}