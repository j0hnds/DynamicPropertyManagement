
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Gtk;
using DomainCore;
using ControlWrappers;
using log4net;
using CronUtils;

namespace PropertyManager
{

    /// <summary>
    /// Dialog to allow creation/modification of DynamicProperty information.
    /// </summary>
    public partial class DynPropEntryDlg : DataBoundDialog
    {
        /// <summary>
        /// Wrapper class for the effective date tree.
        /// </summary>
        private EffectiveDateListControl evListCtl;

        /// <summary>
        /// The logger class.
        /// </summary>
        private ILog log;

        /// <summary>
        /// Constructs a new DynPropEntryDlg dialog object.
        /// </summary>
        public DynPropEntryDlg() 
        {
            this.Build();

            log = LogManager.GetLogger(GetType());

            evListCtl = new EffectiveDateListControl(tvEffectiveValues);
        }

        /// <summary>
        /// Signal handler for ContextChanged event.
        /// </summary>
        /// <param name="contextName">
        /// The name of the data context that changed.
        /// </param>
        /// <param name="itemName">
        /// The name of the item in the data context that changed.
        /// </param>
        private void ContextChangeHandler(string contextName, string itemName)
        {
            log.DebugFormat("ContextChangeHandler({0}, {1})", contextName, itemName);
            
            // Called when the contents of a context has changed.
            if ("DialogContext".Equals(contextName))
            {
                Domain domain = GetContext("DialogContext").GetDomain("DynamicProperty");
                // Load the Effective Value list control
                evListCtl.Populate(domain);

                evListCtl.DynamicProperty = domain;
            }
            else if ("ValueCriteriaCtx".Equals(contextName))
            {
                Domain domain = GetContext("ValueCriteriaCtx").GetDomain("ValueCriteria");

                string rawCriteria = (string) domain.GetValue("RawCriteria");
                if (rawCriteria == null || rawCriteria.Length == 0)
                {
                    rawCriteria = "* * * * *";
                }
                
                // Here is where we set up the cron editor; we always
                // do this when a new ValueCriteria is set up.
                CronSpecification cs = new CronSpecification(rawCriteria);

                tvMinutes.SpecificationList = cs.Minutes;
                tvHours.SpecificationList = cs.Hours;
                tvDays.SpecificationList = cs.Days;
                tvMonths.SpecificationList = cs.Months;
                tvDaysOfWeek.SpecificationList = cs.DaysOfWeek;
            }
        }

        /// <summary>
        /// Signal handler for a Cursor Change in the Effective Value tree.
        /// </summary>
        /// <param name="sender">
        /// The Effective Value tree control
        /// </param>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected virtual void EffectiveValueCursorChanged (object sender, System.EventArgs e)
        {
            switch (evListCtl.SelectedLevel)
            {
            case EffectiveDateLevels.EffectiveDate:
                nbValuePages.CurrentPage = 1;
                LoadSelectedEffectiveDateValues();
                btnAddItem.Sensitive = true;
                btnRemove.Sensitive = true;
                break;

            case EffectiveDateLevels.ValueCriteria:
                nbValuePages.CurrentPage = 2;
                LoadSelectedValueCriteriaValues();
                btnAddItem.Sensitive = false;
                btnRemove.Sensitive = true;
                break;

            case EffectiveDateLevels.TopLevel:
                nbValuePages.CurrentPage = 0;
                btnAddItem.Sensitive = true;
                btnRemove.Sensitive = false;
                break;

            case EffectiveDateLevels.None:
                nbValuePages.CurrentPage = 0;
                btnAddItem.Sensitive = false;
                btnRemove.Sensitive = false;
                break;
            }
        }

        /// <summary>
        /// Updates the EffectiveDateCtx with the selected EffectiveValue domain
        /// object.
        /// </summary>
        private void LoadSelectedEffectiveDateValues()
        {
            Domain domain = evListCtl.GetSelectedDomain();

            // Set the EffectiveValue domain object on the appropriate
            // context
            GetContext("EffectiveDateCtx").AddObject(domain);
        }

        /// <summary>
        /// Updates the ValueCriteriaCtx with the selected ValueCriteria domain object.
        /// </summary>
        private void LoadSelectedValueCriteriaValues()
        {
            Domain domain = evListCtl.GetSelectedDomain();

            GetContext("ValueCriteriaCtx").AddObject(domain);
        }

        protected override DataContext CreateDataContext ()
        {
            // Create and register the base DialogContext
            DataContext context = base.CreateDataContext();

            // Set up the collections we need for the various
            // combo boxes on the form.
            DomainDAO appDao = DomainFactory.GetDAO("Application");

            List<Domain> applications = appDao.Get();

            context.AddObject("Applications", applications);
            
            DomainDAO propDao = DomainFactory.GetDAO("PropertyDefinition");

            List<Domain> properties = propDao.Get();

            context.AddObject("PropertyDefinitions", properties);

            DomainDAO dao = DomainFactory.GetDAO("Form");

            List<Domain> valueList = dao.Get();

            context.AddObject("Forms", valueList);

            // Create a context for use by the Effective Date Editor
            SetContext(new DataContext("EffectiveDateCtx"));

            DataContext vcContext = new DataContext("ValueCriteriaCtx");
            // Create a context for use by the Value Criteria Editor
            SetContext(vcContext);

            // Register interest in when the contexts change...
            context.ContextChanged += ContextChangeHandler;
            vcContext.ContextChanged += ContextChangeHandler;
            
            return context;
        }

        /// <summary>
        /// Signal handler for selection of a form item in the combo box.
        /// </summary>
        /// <param name="sender">
        /// The form combo box.
        /// </param>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected virtual void OnCbFormChanged (object sender, System.EventArgs e)
        {
            long id = cbForm.ActiveId;

            txtQualifier.Text = id.ToString();
        }

        /// <summary>
        /// Signal handler for when the start date time has been changed.
        /// </summary>
        /// <param name="sender">
        /// The bound date time control.
        /// </param>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected virtual void StartDateChanged (object sender, System.EventArgs e)
        {
            // Update the currently selected label on the tree
            evListCtl.UpdateSelected();
        }

        /// <summary>
        /// Signal handler for when the end date time has changed.
        /// </summary>
        /// <param name="sender">
        /// The bound date time control
        /// </param>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected virtual void EndDateChanged (object sender, System.EventArgs e)
        {
            evListCtl.UpdateSelected();
        }

        /// <summary>
        /// Signal handler for a change to the Value Criteria Value.
        /// </summary>
        /// <param name="sender">
        /// The value entry field.
        /// </param>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected virtual void VCValueChanged (object sender, System.EventArgs e)
        {
            evListCtl.UpdateSelected();
        }

        /// <summary>
        /// Signal handler for a change in the Minutes cron editor.
        /// </summary>
        /// <param name="sender">
        /// The cron editor control
        /// </param>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected virtual void MinutesEditorChanged (object sender, System.EventArgs e)
        {
            ArrayList spec = tvMinutes.SpecificationList;

            string rawCriteria = txtCriteria.Text;
            CronSpecification cs = new CronSpecification(rawCriteria);

            cs.Minutes = spec;

            txtCriteria.Text = cs.ToString();

            evListCtl.UpdateSelected();
        }

        /// <summary>
        /// Signal handler for a change in the hours cron editor.
        /// </summary>
        /// <param name="sender">
        /// The hour cron editor control
        /// </param>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected virtual void HoursEditorChanged (object sender, System.EventArgs e)
        {
            ArrayList spec = tvHours.SpecificationList;

            string rawCriteria = txtCriteria.Text;
            CronSpecification cs = new CronSpecification(rawCriteria);

            cs.Hours = spec;

            txtCriteria.Text = cs.ToString();

            evListCtl.UpdateSelected();
        }

        /// <summary>
        /// Signal handler for a change in the day cron editor control.
        /// </summary>
        /// <param name="sender">
        /// The day cron editor control
        /// </param>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected virtual void DaysEditorChanged (object sender, System.EventArgs e)
        {
            ArrayList spec = tvDays.SpecificationList;

            string rawCriteria = txtCriteria.Text;
            CronSpecification cs = new CronSpecification(rawCriteria);

            cs.Days = spec;

            txtCriteria.Text = cs.ToString();

            evListCtl.UpdateSelected();
        }

        /// <summary>
        /// Signal handler for a change in the month cron editor control.
        /// </summary>
        /// <param name="sender">
        /// The month cron editor control
        /// </param>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected virtual void MonthsEditorChanged (object sender, System.EventArgs e)
        {
            ArrayList spec = tvMonths.SpecificationList;

            string rawCriteria = txtCriteria.Text;
            CronSpecification cs = new CronSpecification(rawCriteria);

            cs.Months = spec;

            txtCriteria.Text = cs.ToString();

            evListCtl.UpdateSelected();
        }

        /// <summary>
        /// Signal handler for a change in the dow cron editor control.
        /// </summary>
        /// <param name="sender">
        /// The dow cron editor control
        /// </param>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected virtual void DaysOfWeekEditorChanged (object sender, System.EventArgs e)
        {
            ArrayList spec = tvDaysOfWeek.SpecificationList;

            string rawCriteria = txtCriteria.Text;
            CronSpecification cs = new CronSpecification(rawCriteria);

            cs.DaysOfWeek = spec;

            txtCriteria.Text = cs.ToString();

            evListCtl.UpdateSelected();
        }

        /// <summary>
        /// Signal handler for click of Add Item button.
        /// </summary>
        /// <param name="sender">
        /// The Add Item button.
        /// </param>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected virtual void AddItemClicked (object sender, System.EventArgs e)
        {
            switch (evListCtl.SelectedLevel)
            {
            case EffectiveDateLevels.TopLevel:
                // Add a new effective date
                AddNewEffectiveDate();
                break;

            case EffectiveDateLevels.EffectiveDate:
                // Add a new value criteria
                AddNewValueCriteria();
                break;
            }
        }

        /// <summary>
        /// Adds a new effective value to the tree control.
        /// </summary>
        private void AddNewEffectiveDate()
        {
            // Add the new effective value to the dynamic property
            CollectionRelationship rel = evListCtl.DynamicProperty.GetRelationship("EffectiveValues") as CollectionRelationship;

            Domain domain = rel.AddNewObject();

            // Now, add the new effective date to the end of the list
            // of effective values in the tree.
            evListCtl.AddEffectiveDate(domain);

            EffectiveValueCursorChanged(null, null);
        }

        /// <summary>
        /// Adds a new value criteria object to the tree control.
        /// </summary>
        private void AddNewValueCriteria()
        {
            // Get the owner of the new value criteria
            Domain effValue = evListCtl.GetSelectedDomain();

            CollectionRelationship rel = effValue.GetRelationship("ValueCriteria") as CollectionRelationship;

            Domain domain = rel.AddNewObject();

            domain.SetValue("RawCriteria", "* * * * *");
            domain.SetValue("Value", "PropertyValue");

            // Add the new effective date to the end of the list
            // the value criteria in the tree
            evListCtl.AddValueCriteria(domain);

            EffectiveValueCursorChanged(null, null);
        }

        /// <summary>
        /// Signal handler for a click of the Remove Item button.
        /// </summary>
        /// <param name="sender">
        /// The Remove Item button.
        /// </param>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected virtual void RemoveItemClicked (object sender, System.EventArgs e)
        {
            switch (evListCtl.SelectedLevel)
            {
            case EffectiveDateLevels.EffectiveDate:
                // Remove the selected EffectiveDate
                RemoveSelectedEffectiveValue();
                break;

            case EffectiveDateLevels.ValueCriteria:
                // Remove the selected ValueCriteria
                RemoveSelectedValueCriteria();
                break;
            }
        }

        /// <summary>
        /// Removes the selected effective value from the tree control.
        /// </summary>
        private void RemoveSelectedEffectiveValue()
        {
            Domain domain = evListCtl.GetSelectedDomain();
            
            // Remove effective value from the dynamic property
            CollectionRelationship rel = evListCtl.DynamicProperty.GetRelationship("EffectiveValues") as CollectionRelationship;

            rel.RemoveObject(domain);

            evListCtl.RemoveSelected();
            EffectiveValueCursorChanged(null, null);
        }

        /// <summary>
        /// Removes the selected value criteria from the tree control.
        /// </summary>
        private void RemoveSelectedValueCriteria()
        {
            Domain domain = evListCtl.GetSelectedDomain();
            Domain effValue = evListCtl.GetSelectedDomainParent();
            
            // Remove value criteria from effective value;
            CollectionRelationship rel = effValue.GetRelationship("ValueCriteria") as CollectionRelationship;

            rel.RemoveObject(domain);

            evListCtl.RemoveSelected();
            EffectiveValueCursorChanged(null, null);
        }
    }
}
