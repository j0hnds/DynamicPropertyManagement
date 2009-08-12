using System;
using System.Configuration;
using System.Collections.Generic;
using DomainCore;
using Gtk;
using log4net;
using PropertyManager;
using STUtils;

public partial class MainWindow: Gtk.Window
{
    private const string DISPLAY_SQL_CFG = "DisplaySQL";
    private const string UPDATE_DB_CFG = "UpdateDB";
    
    private MainTextViewControl mainTextViewCtl;
    private ILog log;
    private ApplicationListControl applicationListCtl;
    private PropertyListControl propertyListCtl;
    private FormListControl formListCtl;
    private DynPropertyListControl dynPropertyListCtl;
    
    public MainWindow (): base (Gtk.WindowType.Toplevel)
    {
        Build ();

        log = LogManager.GetLogger(typeof(MainWindow));
        
        mainTextViewCtl = new MainTextViewControl(mainTextView);
    }

    public void SetUpApplication()
    {
        SetUpApplicationTree();
        SetUpPropertyDefinitionTree();
        SetUpFormTree();
        SetUpDynamicPropertyTree();
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

    protected void SetUpPropertyDefinitionTree()
    {
        propertyListCtl = new PropertyListControl(tvPropertyDefinitions);

        DomainDAO dao = DomainFactory.GetDAO("PropertyDefinition");
        List<Domain> properties = dao.Get();
        propertyListCtl.Populate(properties);
    }

    protected void SetUpDynamicPropertyTree()
    {
        dynPropertyListCtl = new DynPropertyListControl(tvDynamicProperties);

        DomainDAO dao = DomainFactory.GetDAO("Application");
        List<Domain> applications = dao.Get();
        dynPropertyListCtl.Populate(applications);
    }

    protected void SetUpApplicationTree()
    {
        applicationListCtl = new ApplicationListControl(tvApplications);

        // Get the DAO for the applications
        DomainDAO dao = DomainFactory.GetDAO("Application");
        List<Domain> applications = dao.Get();
        applicationListCtl.Populate(applications);
            
    }

    protected void SetUpFormTree()
    {
        formListCtl = new FormListControl(tvForms);

        DomainDAO dao = DomainFactory.GetDAO("Form");
        List<Domain> forms = dao.Get();
        formListCtl.Populate(forms);
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

    protected virtual void PropertyDefinitionCursorChanged (object sender, System.EventArgs e)
    {
        Domain domain = propertyListCtl.GetSelectedDomain();
        
        switch (propertyListCtl.SelectedLevel)
        {
        case PropertyDefinitionLevels.Category:
            mainTextViewCtl.Render(DomainRenderer.Render(domain, "CategorySummary"));
            break;

        case PropertyDefinitionLevels.Property:
            mainTextViewCtl.Render(DomainRenderer.Render(domain, "Summary"));
            break;

        case PropertyDefinitionLevels.None:
            break;
        }
        
        HandlePropertyDefinitionToolBarSensitivity();
    }

    protected virtual void DynamicPropertyCursorChanged (object sender, System.EventArgs e)
    {
        Domain domain = dynPropertyListCtl.GetSelectedDomain();

        switch (dynPropertyListCtl.SelectedLevel)
        {
        case DynamicPropertyLevels.Application:
            mainTextViewCtl.Render(DomainRenderer.Render(domain, "Summary"));
            break;

        case DynamicPropertyLevels.Category:
            mainTextViewCtl.Render(DomainRenderer.Render(domain, "CategorySummary"));
            break;

        case DynamicPropertyLevels.Property:
            mainTextViewCtl.Render(DomainRenderer.Render(domain, "Summary"));
            break;
        }

        HandleDynamicPropertyToolBarSensitivity();
    }


    protected virtual void FormCursorChanged (object sender, System.EventArgs e)
    {
        Domain domain = formListCtl.GetSelectedDomain();
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

    private void HandlePropertyDefinitionToolBarSensitivity()
    {
        switch (propertyListCtl.SelectedLevel)
        {
        case PropertyDefinitionLevels.Category:
            AddAction.Sensitive = true;
            RemoveAction.Sensitive = false;
            PropertiesAction.Sensitive = false;
            break;

        case PropertyDefinitionLevels.Property:
            AddAction.Sensitive = true;
            RemoveAction.Sensitive = true;
            PropertiesAction.Sensitive = true;
            break;

        case PropertyDefinitionLevels.None:
            AddAction.Sensitive = false;
            RemoveAction.Sensitive = false;
            PropertiesAction.Sensitive = false;
            break;
        }
    }

    private void HandleDynamicPropertyToolBarSensitivity()
    {
        switch (dynPropertyListCtl.SelectedLevel)
        {
        case DynamicPropertyLevels.Application:
            AddAction.Sensitive = true;
            RemoveAction.Sensitive = false;
            PropertiesAction.Sensitive = false;
            break;

        case DynamicPropertyLevels.Category:
            AddAction.Sensitive = true;
            RemoveAction.Sensitive = false;
            PropertiesAction.Sensitive = false;
            break;

        case DynamicPropertyLevels.None:
            AddAction.Sensitive = false;
            RemoveAction.Sensitive = false;
            PropertiesAction.Sensitive = false;
            break;

        case DynamicPropertyLevels.Property:
            AddAction.Sensitive = true;
            RemoveAction.Sensitive = true;
            PropertiesAction.Sensitive = true;
            break;
        }
    }

    private void HandleFormToolBarSensitivity()
    {
        bool somethingSelected = formListCtl.IsSelected;

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

        case 1: // Forms page
            HandleFormToolBarSensitivity();
            break;

        case 2: // PropertyDefinition page
            HandlePropertyDefinitionToolBarSensitivity();
            break;

        case 3: // Dynamic Properties page
            HandleDynamicPropertyToolBarSensitivity();
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
            if (ConfigurationManager.AppSettings[DISPLAY_SQL_CFG].Equals("true"))
            {
                BufferDisplayDlg bdDlg = new BufferDisplayDlg();
                bdDlg.DoModal(this, domain);
            }
            if (ConfigurationManager.AppSettings[UPDATE_DB_CFG].Equals("true"))
            {
                domain.Save();
            }

            applicationListCtl.AddDomain(domain);
        }
    }

    private void AddPropertyDefinition()
    {
        // Create a new Application domain
        Domain selDomain = propertyListCtl.GetSelectedDomain();
        
        Domain domain = DomainFactory.Create("PropertyDefinition");
        domain.SetValue("Category", selDomain.GetValue("Category"));

        PropertyDefinitionEntryDlg dlg = new PropertyDefinitionEntryDlg();

        if (dlg.DoModal(this, domain))
        {
            log.Info("OK pressed on PropertyDefinitionEntryDlg");
            if (ConfigurationManager.AppSettings[DISPLAY_SQL_CFG].Equals("true"))
            {
                BufferDisplayDlg bdDlg = new BufferDisplayDlg();
                bdDlg.DoModal(this, domain);
            }
            if (ConfigurationManager.AppSettings[UPDATE_DB_CFG].Equals("true"))
            {
                domain.Save();
            }

            propertyListCtl.AddDomain(domain);
        }
    }

    private void AddForm()
    {
        log.Debug("Adding new form");
        // Create a new Form domain
        Domain domain = DomainFactory.Create("Form");

        FormEntryDlg dlg = new FormEntryDlg();

        if (dlg.DoModal(this, domain))
        {
            log.Info("OK pressed on FormEntryDlg");
            if (ConfigurationManager.AppSettings[DISPLAY_SQL_CFG].Equals("true"))
            {
                BufferDisplayDlg bdDlg = new BufferDisplayDlg();
                bdDlg.DoModal(this, domain);
            }
            if (ConfigurationManager.AppSettings[UPDATE_DB_CFG].Equals("true"))
            {
                domain.Save();
            }

            formListCtl.AddDomain(domain);
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
            if (domain.Dirty)
            {
                if (ConfigurationManager.AppSettings[DISPLAY_SQL_CFG].Equals("true"))
                {
                    BufferDisplayDlg bdDlg = new BufferDisplayDlg();
                    bdDlg.DoModal(this, domain);
                }
                if (ConfigurationManager.AppSettings[UPDATE_DB_CFG].Equals("true"))
                {
                    domain.Save();
                }
                applicationListCtl.UpdateSelectedLabel();
            }
        }
        log.DebugFormat("Application Name: {0}", domain.GetValue("Name"));
    }

    private void EditPropertyDefinition()
    {
        // Need to get the selected domain
        Domain domain = propertyListCtl.GetSelectedDomain();

        PropertyDefinitionEntryDlg dlg = new PropertyDefinitionEntryDlg();

        if (dlg.DoModal(this, domain))
        {
            log.Info("OK pressed on PropertyDefinitionEntryDlg");
            if (domain.Dirty)
            {
                if (ConfigurationManager.AppSettings[DISPLAY_SQL_CFG].Equals("true"))
                {
                    BufferDisplayDlg bdDlg = new BufferDisplayDlg();
                    bdDlg.DoModal(this, domain);
                }
                if (ConfigurationManager.AppSettings[UPDATE_DB_CFG].Equals("true"))
                {
                    domain.Save();
                }
                propertyListCtl.UpdateSelectedLabel();
            }
        }
    }

    private void EditForm()
    {
        log.Debug("Editing form");
        // Need to get the selected domain
        Domain domain = formListCtl.GetSelectedDomain();

        FormEntryDlg dlg = new FormEntryDlg();

        if (dlg.DoModal(this, domain))
        {
            log.Info("OK pressed on FormEntryDlg");
            if (domain.Dirty)
            {
                if (ConfigurationManager.AppSettings[DISPLAY_SQL_CFG].Equals("true"))
                {
                    BufferDisplayDlg bdDlg = new BufferDisplayDlg();
                    bdDlg.DoModal(this, domain);
                }
                if (ConfigurationManager.AppSettings[UPDATE_DB_CFG].Equals("true"))
                {
                    domain.Save();
                }
                formListCtl.UpdateSelectedLabel();
            }
        }
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

                    if (ConfigurationManager.AppSettings[DISPLAY_SQL_CFG].Equals("true"))
                    {
                        BufferDisplayDlg bdDlg = new BufferDisplayDlg();
                        bdDlg.DoModal(this, domain);
                    }
                    // Now, delete the object.
                    if (ConfigurationManager.AppSettings[UPDATE_DB_CFG].Equals("true"))
                    {
                        domain.Save();
                    }
                    applicationListCtl.RemoveSelected();
                }
            }
        }
    }

    private void RemovePropertyDefinition()
    {
        // Need to get the selected domain
        Domain domain = propertyListCtl.GetSelectedDomain();

        if (domain != null)
        {
            // Have the user verify that we really want to remove the
            // selected object.
            MessageDialog dlg = new MessageDialog(this,
                                                  DialogFlags.DestroyWithParent,
                                                  MessageType.Question,
                                                  ButtonsType.YesNo,
                                                  string.Format("Are you sure you wish to remove property '{0}'?",
                                                                domain.GetValue("Name")));
            int result = dlg.Run();
            dlg.Destroy();
            
            if (result == ResponseType.Yes.value__)
            {
                log.InfoFormat("User chose to remove property '{0}'",
                               domain.GetValue("Name"));
                if (! domain.NewObject)
                {
                    domain.ForDelete = true;

                    if (ConfigurationManager.AppSettings[DISPLAY_SQL_CFG].Equals("true"))
                    {
                        BufferDisplayDlg bdDlg = new BufferDisplayDlg();
                        bdDlg.DoModal(this, domain);
                    }
                    // Now, delete the object.
                    if (ConfigurationManager.AppSettings[UPDATE_DB_CFG].Equals("true"))
                    {
                        domain.Save();
                    }
                    propertyListCtl.RemoveSelected();
                }
            }
        }
    }

    private void RemoveForm()
    {
        log.Debug("Removing form");
        // Need to get the selected domain
        Domain domain = formListCtl.GetSelectedDomain();

        if (domain != null)
        {
            // Have the user verify that we really want to remove the
            // selected object.
            MessageDialog dlg = new MessageDialog(this,
                                                  DialogFlags.DestroyWithParent,
                                                  MessageType.Question,
                                                  ButtonsType.YesNo,
                                                  string.Format("Are you sure you wish to remove form '{0}'?",
                                                                domain.GetValue("Description")));
            int result = dlg.Run();
            dlg.Destroy();
            
            if (result == ResponseType.Yes.value__)
            {
                log.InfoFormat("User chose to remove form '{0}'",
                               domain.GetValue("Description"));
                if (! domain.NewObject)
                {
                    domain.ForDelete = true;

                    if (ConfigurationManager.AppSettings[DISPLAY_SQL_CFG].Equals("true"))
                    {
                        BufferDisplayDlg bdDlg = new BufferDisplayDlg();
                        bdDlg.DoModal(this, domain);
                    }
                    // Now, delete the object.
                    if (ConfigurationManager.AppSettings[UPDATE_DB_CFG].Equals("true"))
                    {
                        domain.Save();
                    }
                    formListCtl.RemoveSelected();
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

        case 1: // Forms page
            AddForm();
            break;

        case 2: // PropertyDefinition page;
            AddPropertyDefinition();
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

        case 1: // Form page
            RemoveForm();
            break;

        case 2: // PropertyDefinition page
            RemovePropertyDefinition();
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

        case 1: // Form page
            EditForm();
            break;

        case 2: // PropertyDefinition page
            EditPropertyDefinition();
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