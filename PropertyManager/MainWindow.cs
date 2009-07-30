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

//    public void OnADifferentEvent (object o, Gtk.ButtonPressEventArgs args)
//    {
//        Console.Out.WriteLine("Button press event");
//        if (args.Event.Button == 3)
//        {
//            int y = (int) args.Event.Y;
//            int x = (int)args.Event.X;
//            uint time = args.Event.Time;
//            TreePath path = null;
//            tvApplications.GetPathAtPos(x,y, out path);
//            if (path != null)
//            {
//                Console.Out.WriteLine("Hello, world");
//            }
//            args.RetVal = 1;
//        }
//                
//    }

//    protected virtual void on_cursor_changed_tvApplications (object sender, System.EventArgs e)
//    {
//        Console.Out.WriteLine("Cursor Changed");
//    }

//    protected virtual void OnButtonPressApplications (object o, Gtk.ButtonPressEventArgs args)
//    {
//        args.RetVal = true;
//        Console.Out.WriteLine("OnButtonPress");
//    }

}