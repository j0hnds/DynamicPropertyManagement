using System;
using Gtk;

public partial class MainWindow: Gtk.Window
{   
    public MainWindow (): base (Gtk.WindowType.Toplevel)
    {
        Build ();

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
        ListStore store = new ListStore(GLib.GType.String, GLib.GType.Object);
        tvApplications.Model = store;
        
        // Set up the columns
        TreeViewColumn tc = new TreeViewColumn();
        tc.Title = "Name";
        CellRenderer cell = new CellRendererText();
        tc.PackStart(cell, true);
        tc.AddAttribute(cell, "text", 0);
        tvApplications.AppendColumn(tc);

        // Now, just add a few fake items
        store.AppendValues("Item 1", "Item 1");
        store.AppendValues("Item 2", "Item 2");
        Console.Out.WriteLine("Setup the control");
            
    }


}