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

        // Get the DAO for the applications
        DomainDAO dao = DomainFactory.GetDAO("Application");
        List<Domain> applications = dao.Get();
        applicationListCtl.Populate(applications);
            
    }

    protected virtual void ApplicationCursorChanged (object sender, System.EventArgs e)
    {
        Domain domain = applicationListCtl.GetSelectedDomain();
        if (domain != null)
        {
            mainTextViewCtl.Render(DomainRenderer.Render(domain, "Summary"));
        }

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

    private void AddApplication()
    {
        // Create a new Application domain
        Domain domain = DomainFactory.Create("Application");

        ApplicationEntryDlg dlg = new ApplicationEntryDlg();

        if (dlg.DoModal(this, domain))
        {
            log.Info("OK pressed on ApplicationEntryDlg");
        }
    }

    private void EditApplication()
    {
        // Need to get the selected domain
        Domain domain = applicationListCtl.GetSelectedDomain();

        ApplicationEntryDlg dlg = new ApplicationEntryDlg();

        if (dlg.DoModal(this, domain))
        {
            log.Info("OK pressed on ApplicationEntryDlg");
        }
        log.DebugFormat("Application Name: {0}", domain.GetValue("Name"));
    }

    private void RemoveApplication()
    {
        // Need to get the selected domain
        Domain domain = applicationListCtl.GetSelectedDomain();

        if (domain != null)
        {
            // Have the user verify that we really want to remove the
            // selected object.
            MessageDialog dlg = new MessageDialog(this,
                                                  DialogFlags.DestroyWithParent,
                                                  MessageType.Question,
                                                  ButtonsType.YesNo,
                                                  string.Format("Are you sure you wish to remove application '{0}'?",
                                                                domain.GetValue("Name")));
            int result = dlg.Run();
            dlg.Destroy();
            
            if (result == ResponseType.Yes.value__)
            {
                log.InfoFormat("User chose to remove application '{0}'",
                               domain.GetValue("Name"));
                if (! domain.NewObject)
                {
                    domain.ForDelete = true;

                    // Now, delete the object.
                }
            }
        }
    }

    protected virtual void AddItemAction (object sender, System.EventArgs e)
    {
        log.Debug("Add a new item to the current item");
        int currentPage = nbSelections.CurrentPage;
        switch (currentPage)
        {
        case 0: // Application page
            AddApplication();
            break;

        default: // Any other page
            log.WarnFormat("Unknown notebook page selected: {0}", currentPage);
            break;
        }
    }

    protected virtual void RemoveItemAction (object sender, System.EventArgs e)
    {
        log.Debug("Remove the selected item.");
        int currentPage = nbSelections.CurrentPage;
        switch (currentPage)
        {
        case 0: // Application page
            RemoveApplication();
            break;

        default: // Any other page
            log.WarnFormat("Unknown notebook page selected: {0}", currentPage);
            break;
        }
    }

    protected virtual void ItemPropertyAction (object sender, System.EventArgs e)
    {
        log.Debug("Edit the selected item.");
        int currentPage = nbSelections.CurrentPage;
        switch (currentPage)
        {
        case 0: // Application page
            EditApplication();
            break;

        default: // Any other page
            log.WarnFormat("Unknown notebook page selected: {0}", currentPage);
            break;
        }
    }

    protected virtual void SwitchPageAction (object o, Gtk.SwitchPageArgs args)
    {
        HandleToolBarSensitivity();
    }


}