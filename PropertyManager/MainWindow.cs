using System;
using System.Configuration;
using System.Collections.Generic;
using DomainCore;
using Gtk;
using log4net;
using PropertyManager;
using STUtils;

/// <summary>
/// The main window for the PropertyManager application.
/// </summary>
public partial class MainWindow: Gtk.Window
{
    /// <summary>
    /// Name of configuration item to determine if SQL should be displayed.
    /// </summary>
    private const string DISPLAY_SQL_CFG = "DisplaySQL";
    /// <summary>
    /// Name of configuration item to determine if data base should actually be updated.
    /// </summary>
    private const string UPDATE_DB_CFG = "UpdateDB";

    /// <summary>
    /// Wrapper class for the main summary text area.
    /// </summary>
    private MainTextViewControl mainTextViewCtl;
    /// <summary>
    /// The logger for this class.
    /// </summary>
    private ILog log;
    /// <summary>
    /// Wrapper class for the application list control.
    /// </summary>
    private ApplicationListControl applicationListCtl;
    /// <summary>
    /// Wrapper class for the property list control
    /// </summary>
    private PropertyListControl propertyListCtl;
    /// <summary>
    /// Wrapper class for the form list control.
    /// </summary>
    private FormListControl formListCtl;
    /// <summary>
    /// Wrapper class for the dynamic property list control.
    /// </summary>
    private DynPropertyListControl dynPropertyListCtl;

    /// <summary>
    /// Constructs a new MainWindow object.
    /// </summary>
    public MainWindow (): base (Gtk.WindowType.Toplevel)
    {
        Build ();

        log = LogManager.GetLogger(typeof(MainWindow));
        
        mainTextViewCtl = new MainTextViewControl(mainTextView);
    }

    /// <summary>
    /// Sets up the initial contents of the application.
    /// </summary>
    /// <remarks>
    /// This is separated from the initial application display because we need
    /// to be logged in prior to displaying the data.
    /// </remarks>
    public void SetUpApplication()
    {
        SetUpApplicationTree();
        SetUpPropertyDefinitionTree();
        SetUpFormTree();
        SetUpDynamicPropertyTree();
    }

    /// <summary>
    /// Signal handler for Delete Event.
    /// </summary>
    /// <param name="sender">
    /// The Delete action.
    /// </param>
    /// <param name="a">
    /// The event arguments.
    /// </param>
    protected void OnDeleteEvent (object sender, DeleteEventArgs a)
    {
        Application.Quit ();
        a.RetVal = true;
    }

    /// <summary>
    /// Signal handler for the Quit action.
    /// </summary>
    /// <param name="sender">
    /// The Quit action.
    /// </param>
    /// <param name="e">
    /// The event arguments.
    /// </param>
    protected virtual void on_file_quit (object sender, System.EventArgs e)
    {
        Application.Quit();
    }

    /// <summary>
    /// Sets up the Property Definition tree.
    /// </summary>
    protected void SetUpPropertyDefinitionTree()
    {
        propertyListCtl = new PropertyListControl(tvPropertyDefinitions);

        DomainDAO dao = DomainFactory.GetDAO("PropertyDefinition");
        List<Domain> properties = dao.Get();
        propertyListCtl.Populate(properties);
    }

    /// <summary>
    /// Sets up the Dynamic Property tree.
    /// </summary>
    protected void SetUpDynamicPropertyTree()
    {
        dynPropertyListCtl = new DynPropertyListControl(tvDynamicProperties);

        DomainDAO dao = DomainFactory.GetDAO("Application");
        List<Domain> applications = dao.Get();
        dynPropertyListCtl.Populate(applications);
    }

    /// <summary>
    /// Sets up the Application tree.
    /// </summary>
    protected void SetUpApplicationTree()
    {
        applicationListCtl = new ApplicationListControl(tvApplications);

        // Get the DAO for the applications
        DomainDAO dao = DomainFactory.GetDAO("Application");
        List<Domain> applications = dao.Get();
        applicationListCtl.Populate(applications);
            
    }

    /// <summary>
    /// Sets up the Form tree.
    /// </summary>
    protected void SetUpFormTree()
    {
        formListCtl = new FormListControl(tvForms);

        DomainDAO dao = DomainFactory.GetDAO("Form");
        List<Domain> forms = dao.Get();
        formListCtl.Populate(forms);
    }

    /// <summary>
    /// Signal handler for cursor change in Application list.
    /// </summary>
    /// <param name="sender">
    /// The application list control
    /// </param>
    /// <param name="e">
    /// The event arguments.
    /// </param>
    protected virtual void ApplicationCursorChanged (object sender, System.EventArgs e)
    {
        Domain domain = applicationListCtl.GetSelectedDomain();
        if (domain != null)
        {
            mainTextViewCtl.Render(DomainRenderer.Render(domain, "Summary"));
        }

        HandleToolBarSensitivity();
    }

    /// <summary>
    /// Signal handler for cursor change in Property Definition tree.
    /// </summary>
    /// <param name="sender">
    /// The property definition tree control
    /// </param>
    /// <param name="e">
    /// The event arguments.
    /// </param>
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

    /// <summary>
    /// Signal handler for cursor change in Dynamic Property tree control.
    /// </summary>
    /// <param name="sender">
    /// The Dynamic Property tree control
    /// </param>
    /// <param name="e">
    /// The event arguments.
    /// </param>
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


    /// <summary>
    /// Signal handler for cursor change in Form tree control.
    /// </summary>
    /// <param name="sender">
    /// The form tree control
    /// </param>
    /// <param name="e">
    /// The event arguments.
    /// </param>
    protected virtual void FormCursorChanged (object sender, System.EventArgs e)
    {
        Domain domain = formListCtl.GetSelectedDomain();
        if (domain != null)
        {
            mainTextViewCtl.Render(DomainRenderer.Render(domain, "Summary"));
        }

        HandleToolBarSensitivity();
    }

    /// <summary>
    /// Helper method to handle the sensitivity of the toolbar items with respect
    /// to a change in the Application tree control.
    /// </summary>
    private void HandleApplicationToolBarSensitivity()
    {
        bool somethingSelected = applicationListCtl.IsSelected;

        AddAction.Sensitive = true;
        RemoveAction.Sensitive = somethingSelected;
        PropertiesAction.Sensitive = somethingSelected;
        ExecuteAction.Sensitive = false;
    }

    /// <summary>
    /// Helper method to handle the sensitivity of tool bar items with
    /// respect to a change in selection on the Property Definition tree.
    /// </summary>
    private void HandlePropertyDefinitionToolBarSensitivity()
    {
        switch (propertyListCtl.SelectedLevel)
        {
        case PropertyDefinitionLevels.Category:
            AddAction.Sensitive = true;
            RemoveAction.Sensitive = false;
            PropertiesAction.Sensitive = false;
            ExecuteAction.Sensitive = false;
            break;

        case PropertyDefinitionLevels.Property:
            AddAction.Sensitive = true;
            RemoveAction.Sensitive = true;
            PropertiesAction.Sensitive = true;
            ExecuteAction.Sensitive = false;
            break;

        case PropertyDefinitionLevels.None:
            AddAction.Sensitive = true;
            RemoveAction.Sensitive = false;
            PropertiesAction.Sensitive = false;
            ExecuteAction.Sensitive = false;
            break;
        }
    }

    /// <summary>
    /// Helper method to handle the sensitivity of tool bar items with respect
    /// to a change in selection in the Dynamic Property tree control.
    /// </summary>
    private void HandleDynamicPropertyToolBarSensitivity()
    {
        switch (dynPropertyListCtl.SelectedLevel)
        {
        case DynamicPropertyLevels.Application:
            AddAction.Sensitive = true;
            RemoveAction.Sensitive = false;
            PropertiesAction.Sensitive = false;
            ExecuteAction.Sensitive = false;
            break;

        case DynamicPropertyLevels.Category:
            AddAction.Sensitive = true;
            RemoveAction.Sensitive = false;
            PropertiesAction.Sensitive = false;
            ExecuteAction.Sensitive = false;
            break;

        case DynamicPropertyLevels.None:
            AddAction.Sensitive = false;
            RemoveAction.Sensitive = false;
            PropertiesAction.Sensitive = false;
            ExecuteAction.Sensitive = false;
            break;

        case DynamicPropertyLevels.Property:
            AddAction.Sensitive = true;
            RemoveAction.Sensitive = true;
            PropertiesAction.Sensitive = true;
            ExecuteAction.Sensitive = true;
            break;
        }
    }

    /// <summary>
    /// Helper method to handle the sensitivity of tool bar items with respect
    /// to selection changes in the Form tree control.
    /// </summary>
    private void HandleFormToolBarSensitivity()
    {
        bool somethingSelected = formListCtl.IsSelected;

        AddAction.Sensitive = true;
        RemoveAction.Sensitive = somethingSelected;
        PropertiesAction.Sensitive = somethingSelected;
        ExecuteAction.Sensitive = false;
    }

    /// <summary>
    /// Helper method to handle the default sensitivity of tool bar items.
    /// </summary>
    private void HandleNoopToolBarSensitivity()
    {
        AddAction.Sensitive = true;
        RemoveAction.Sensitive = false;
        PropertiesAction.Sensitive = false;
        ExecuteAction.Sensitive = false;
    }

    /// <summary>
    /// Helper method to generally handle the sensitivity of tool bar items.
    /// </summary>
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

    /// <summary>
    /// Adds a new application.
    /// </summary>
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

    /// <summary>
    /// Adds a new Property Definition.
    /// </summary>
    private void AddPropertyDefinition()
    {
        // Create a new Application domain
        Domain selDomain = propertyListCtl.GetSelectedDomain();
        
        Domain domain = DomainFactory.Create("PropertyDefinition");
        if (selDomain != null)
        {
            domain.SetValue("Category", selDomain.GetValue("Category"));
        }

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

    /// <summary>
    /// Adds a new Form
    /// </summary>
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

    /// <summary>
    /// Adds a new Dynamic Property.
    /// </summary>
    private void AddDynamicProperty()
    {
        log.Debug("Adding new Dynamic Property");
        // Create a new Form domain
        Domain domain = DomainFactory.Create("DynamicProperty");

        DynPropEntryDlg dlg = new DynPropEntryDlg();

        if (dlg.DoModal(this, domain))
        {
            log.Info("OK pressed on DynPropEntryDlg");
            if (ConfigurationManager.AppSettings[DISPLAY_SQL_CFG].Equals("true"))
            {
                BufferDisplayDlg bdDlg = new BufferDisplayDlg();
                bdDlg.DoModal(this, domain);
            }
            if (ConfigurationManager.AppSettings[UPDATE_DB_CFG].Equals("true"))
            {
                domain.Save();
            }

            // formListCtl.AddDomain(domain);
        }
    }

    /// <summary>
    /// Edits the selected application.
    /// </summary>
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

    /// <summary>
    /// Edits the selected Property Definition.
    /// </summary>
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

    /// <summary>
    /// Edits the selected Form
    /// </summary>
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

    /// <summary>
    /// Edits the selected Dynamic Property.
    /// </summary>
    private void EditDynamicProperty()
    {
        log.Debug("Editing dynamic property");
        // Need to get the selected domain
        Domain domain = dynPropertyListCtl.GetSelectedDomain();

        DynPropEntryDlg dlg = new DynPropEntryDlg();

        if (dlg.DoModal(this, domain))
        {
            log.Info("OK pressed on DynPropEntryDlg");
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

    /// <summary>
    /// Removes the selected application.
    /// </summary>
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

    /// <summary>
    /// Removes the selected Property Definition.
    /// </summary>
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

    /// <summary>
    /// Removes the selected form.
    /// </summary>
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

    /// <summary>
    /// Removes the selected Dynamic Property.
    /// </summary>
    private void RemoveDynamicProperty()
    {
        log.Debug("Removing Dynamic Property");
        // Need to get the selected domain
        Domain domain = dynPropertyListCtl.GetSelectedDomain();

        if (domain != null)
        {
            // Have the user verify that we really want to remove the
            // selected object.
            MessageDialog dlg = new MessageDialog(this,
                                                  DialogFlags.DestroyWithParent,
                                                  MessageType.Question,
                                                  ButtonsType.YesNo,
                                                  string.Format("Are you sure you wish to remove Property '{0}'?",
                                                                domain.GetValue("PropertyName")));
            int result = dlg.Run();
            dlg.Destroy();
            
            if (result == ResponseType.Yes.value__)
            {
                log.InfoFormat("User chose to remove Dynamic Property '{0}'",
                               domain.GetValue("PropertyName"));
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
                    dynPropertyListCtl.RemoveSelected();
                }
            }
        }
    }

    /// <summary>
    /// Signal handler for activation of Add Item Action.
    /// </summary>
    /// <param name="sender">
    /// The Add Item action.
    /// </param>
    /// <param name="e">
    /// The event arguments.
    /// </param>
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

        case 3: // Dynamic Property page
            AddDynamicProperty();
            break;

        default: // Any other page
            log.WarnFormat("Unknown notebook page selected: {0}", currentPage);
            break;
        }
    }

    /// <summary>
    /// Signal handler for activation of Remove Item action.
    /// </summary>
    /// <param name="sender">
    /// The Remove Item action.
    /// </param>
    /// <param name="e">
    /// The event arguments.
    /// </param>
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

        case 3: // Dynamic Property page
            RemoveDynamicProperty();
            break;

        default: // Any other page
            log.WarnFormat("Unknown notebook page selected: {0}", currentPage);
            break;
        }
    }

    /// <summary>
    /// Signal handler for the activation of the Item Property action.
    /// </summary>
    /// <param name="sender">
    /// The Item Property action.
    /// </param>
    /// <param name="e">
    /// The event arguments.
    /// </param>
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

        case 3: // Dynamic Property page
            EditDynamicProperty();
            break;

        default: // Any other page
            log.WarnFormat("Unknown notebook page selected: {0}", currentPage);
            break;
        }
    }

    /// <summary>
    /// Signal handler for a notebook switch page action.
    /// </summary>
    /// <param name="o">
    /// The notebook widget.
    /// </param>
    /// <param name="args">
    /// The event arguments.
    /// </param>
    protected virtual void SwitchPageAction (object o, Gtk.SwitchPageArgs args)
    {
        HandleToolBarSensitivity();
    }

    /// <summary>
    /// Signal handler for activation of Test action.
    /// </summary>
    /// <param name="sender">
    /// The Test action
    /// </param>
    /// <param name="e">
    /// The event arguments.
    /// </param>
    protected virtual void TestDynamicPropertyAction (object sender, System.EventArgs e)
    {
        Domain domain = dynPropertyListCtl.GetSelectedDomain();
        
        TestDynPropDlg dlg = new TestDynPropDlg();

        dlg.DoModal(this, domain);
    }


}