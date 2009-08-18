
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

    public partial class DynPropEntryDlg : DataBoundDialog // Gtk.Dialog
    {
        private EffectiveDateListControl evListCtl;

        private ILog log;

        public DynPropEntryDlg() 
        {
            this.Build();

            log = LogManager.GetLogger(GetType());

            evListCtl = new EffectiveDateListControl(tvEffectiveValues);
        }

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
        
        protected virtual void EffectiveValueCursorChanged (object sender, System.EventArgs e)
        {
            switch (evListCtl.SelectedLevel)
            {
            case EffectiveDateLevels.EffectiveDate:
                nbValuePages.CurrentPage = 1;
                LoadSelectedEffectiveDateValues();
                break;

            case EffectiveDateLevels.ValueCriteria:
                nbValuePages.CurrentPage = 2;
                LoadSelectedValueCriteriaValues();
                break;

            case EffectiveDateLevels.None:
                nbValuePages.CurrentPage = 0;
                break;
            }
        }

        private void LoadSelectedEffectiveDateValues()
        {
            Domain domain = evListCtl.GetSelectedDomain();

            // Set the EffectiveValue domain object on the appropriate
            // context
            GetContext("EffectiveDateCtx").AddObject(domain);
        }

        private void LoadSelectedValueCriteriaValues()
        {
            Domain domain = evListCtl.GetSelectedDomain();

            GetContext("ValueCriteriaCtx").AddObject(domain);
        }

        private void SetCheckCBList(ListStore ls, bool setCheck)
        {
            TreeIter iter = TreeIter.Zero;
            
            bool more = ls.GetIterFirst(out iter);
            while (more)
            {
                ls.SetValue(iter, 0, setCheck);
                more = ls.IterNext(ref iter);
            }
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

        protected virtual void OnCbFormChanged (object sender, System.EventArgs e)
        {
            long id = cbForm.ActiveId;

            txtQualifier.Text = id.ToString();
        }

        protected virtual void StartDateChanged (object sender, System.EventArgs e)
        {
            // Update the currently selected label on the tree
            evListCtl.UpdateSelected();
        }

        protected virtual void EndDateChanged (object sender, System.EventArgs e)
        {
            evListCtl.UpdateSelected();
        }

        protected virtual void VCValueChanged (object sender, System.EventArgs e)
        {
            evListCtl.UpdateSelected();
        }

        protected virtual void MinutesEditorChanged (object sender, System.EventArgs e)
        {
            ArrayList spec = tvMinutes.SpecificationList;

            string rawCriteria = txtCriteria.Text;
            CronSpecification cs = new CronSpecification(rawCriteria);

            cs.Minutes = spec;

            txtCriteria.Text = cs.ToString();

            evListCtl.UpdateSelected();
        }

        protected virtual void HoursEditorChanged (object sender, System.EventArgs e)
        {
            ArrayList spec = tvHours.SpecificationList;

            string rawCriteria = txtCriteria.Text;
            CronSpecification cs = new CronSpecification(rawCriteria);

            cs.Hours = spec;

            txtCriteria.Text = cs.ToString();

            evListCtl.UpdateSelected();
        }

        protected virtual void DaysEditorChanged (object sender, System.EventArgs e)
        {
            ArrayList spec = tvDays.SpecificationList;

            string rawCriteria = txtCriteria.Text;
            CronSpecification cs = new CronSpecification(rawCriteria);

            cs.Days = spec;

            txtCriteria.Text = cs.ToString();

            evListCtl.UpdateSelected();
        }

        protected virtual void MonthsEditorChanged (object sender, System.EventArgs e)
        {
            ArrayList spec = tvMonths.SpecificationList;

            string rawCriteria = txtCriteria.Text;
            CronSpecification cs = new CronSpecification(rawCriteria);

            cs.Months = spec;

            txtCriteria.Text = cs.ToString();

            evListCtl.UpdateSelected();
        }

        protected virtual void DaysOfWeekEditorChanged (object sender, System.EventArgs e)
        {
            ArrayList spec = tvDaysOfWeek.SpecificationList;

            string rawCriteria = txtCriteria.Text;
            CronSpecification cs = new CronSpecification(rawCriteria);

            cs.DaysOfWeek = spec;

            txtCriteria.Text = cs.ToString();

            evListCtl.UpdateSelected();
        }
    }
}
