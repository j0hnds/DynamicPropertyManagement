
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

    public enum CronEditorType
    {
        Minutes,
        Hours,
        Days,
        Months,
        DaysOfWeek
    }
    
    public partial class DynPropEntryDlg : DataBoundDialog // Gtk.Dialog
    {
        private EffectiveDateListControl evListCtl;

        private static readonly string[] MONTHS = {
            "Jan",
            "Feb",
            "Mar",
            "Apr",
            "May",
            "Jun",
            "Jul",
            "Aug",
            "Sep",
            "Oct",
            "Nov",
            "Dec"
        };

        private static readonly string[] DOWS = {
            "Sun",
            "Mon",
            "Tue",
            "Wed",
            "Thu",
            "Fri",
            "Sat"
        };

        private ILog log;

        public DynPropEntryDlg() 
        {
            this.Build();

            log = LogManager.GetLogger(GetType());

//            DomainDAO appDao = DomainFactory.GetDAO("Application");
//
//            List<Domain> applications = appDao.Get();
//            
//            DomainDAO propDao = DomainFactory.GetDAO("PropertyDefinition");
//
//            List<Domain>properties = propDao.Get();

            // Set up the binding controls
//            new ComboBoxBoundControl(this, cbApplication, "ApplicationId", applications, "Name", "Id");
//            new ComboBoxBoundControl(this, cbProperty, "PropertyId", properties, "Name", "Id");
//            new EntryBoundControl(this, txtQualifier, "Qualifier");
//            new EntryBoundControl(this, txtDefaultValue, "DefaultValue");

            // Set up the form combo-box
//            SetUpForms();

            evListCtl = new EffectiveDateListControl(tvEffectiveValues);

            SetUpCronEditor();
        }

//        private void SetUpForms()
//        {
//            DomainDAO dao = DomainFactory.GetDAO("Form");
//            List<Domain> valueList = dao.Get();
//            ListStore listStore = new ListStore(GLib.GType.String, GLib.GType.Int64);
//            foreach (Domain domain in valueList)
//            {
//                object displayValue = domain.GetValue("Description");
//                object valueValue = domain.GetValue("Id");
//
//                listStore.AppendValues(displayValue, valueValue);
//            }
//
//            cbForm.Model = listStore;
//        }

//        protected virtual void FormSelectionChanged (object sender, System.EventArgs e)
//        {
//            TreeModel model = cbForm.Model;
//            TreeIter iter = TreeIter.Zero;
//
//            if (cbForm.GetActiveIter(out iter))
//            {
//                long formId = (long) model.GetValue(iter, 1);
//                txtQualifier.Text = formId.ToString();
//            }
//        }
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
    
                // Set up the values in the editors based on the value
                // of the specification
                SetCronEditorValues(tvMinutes.Model as ListStore, cs.Minutes);
                SetCronEditorValues(tvHours.Model as ListStore, cs.Hours);
                SetCronEditorValues(tvDays.Model as ListStore, cs.Days);
                SetCronEditorValues(tvMonths.Model as ListStore, cs.Months);
                SetCronEditorValues(tvDOWs.Model as ListStore, cs.DaysOfWeek);
            }
        }
        
//        public override bool DoModal(Window parentWindow, Domain domain)
//        {
//            bool ok = false;
//
//            TransientFor = parentWindow;
//
//            DomainToControls(domain);
//
//            evListCtl.Populate(domain);
//
//            evListCtl.DynamicProperty = domain;
//
//            int response = Run();
//            if (response == Gtk.ResponseType.Ok.value__)
//            {
//                ok = true;
//                ControlsToDomain(domain);
//            }
//
//            Destroy();
//
//            return ok;
//        }

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

//            DateTime effectiveStartDate = (DateTime) domain.GetValue("EffectiveStartDate");
//            DateTime effectiveEndDate = (DateTime) domain.GetValue("EffectiveEndDate");
//
//            if (effectiveStartDate == DateTime.MinValue)
//            {
//                cbStartDateNull.Active = false;
//                calStartDate.Sensitive = false;
//                txtStartTime.Sensitive = false;
//                txtStartTime.Text = "00:00";
//            }
//            else
//            {
//                calStartDate.Date = effectiveStartDate;
//                cbStartDateNull.Active = true;
//                calStartDate.Sensitive = true;
//                txtStartTime.Sensitive = true;
//                txtStartTime.Text = effectiveStartDate.ToString("HH:mm");
//            }
//            if (effectiveEndDate == DateTime.MinValue)
//            {
//                cbEndDateNull.Active = false;
//                calEndDate.Sensitive = false;
//                txtEndTime.Sensitive = false;
//                txtEndTime.Text = "00:00";
//            }
//            else
//            {
//                calEndDate.Date = effectiveEndDate;
//                cbEndDateNull.Active = true;
//                calEndDate.Sensitive = true;
//                txtEndTime.Sensitive = true;
//                txtEndTime.Text = effectiveEndDate.ToString("HH:mm");
//            }
        }

        private void LoadSelectedValueCriteriaValues()
        {
            Domain domain = evListCtl.GetSelectedDomain();

            GetContext("ValueCriteriaCtx").AddObject(domain);

//            string rawCriteria = (string) domain.GetValue("RawCriteria");
//            string domainValue = (string) domain.GetValue("Value");
//
//            if (rawCriteria == null)
//            {
//                rawCriteria = "* * * * *";
//            }
//
//            txtCriteria.Text = rawCriteria;
//            txtValue.Text = domainValue;

//            CronSpecification cs = new CronSpecification(rawCriteria);
//
//            // Set up the values in the editors based on the value
//            // of the specification
//            SetCronEditorValues(tvMinutes.Model as ListStore, cs.Minutes);
//            SetCronEditorValues(tvHours.Model as ListStore, cs.Hours);
//            SetCronEditorValues(tvDays.Model as ListStore, cs.Days);
//            SetCronEditorValues(tvMonths.Model as ListStore, cs.Months);
//            SetCronEditorValues(tvDOWs.Model as ListStore, cs.DaysOfWeek);
        }

        private void SetCronEditorValues(ListStore ls, ArrayList cronSpecs)
        {
            TreeIter iter = TreeIter.Zero;
            bool more = ls.GetIterFirst(out iter);
            while (more)
            {
                int val = (int) ls.GetValue(iter, 2);
                ls.SetValue(iter, 0, IsValueEffective(cronSpecs, val));
                more = ls.IterNext(ref iter);
            }
        }

        private bool IsValueEffective(ArrayList cronSpecs, int val)
        {
            bool effective = false;
            
            foreach (object cronSpec in cronSpecs)
            {
                CronEffectiveValue cev = (CronEffectiveValue) cronSpec;
                effective = cev.IsEffective(val);
                if (effective)
                {
                    break;
                }
            }

            return effective;
        }

        private void SetUpCronEditor()
        {
            SetUpCronMinuteEditor();
            SetUpCronHourEditor();
            SetUpCronDayEditor();
            SetUpCronMonthEditor();
            SetUpCronDOWEditor();
        }

        private void SetUpCronMinuteEditor()
        {
            ListStore listStore = new ListStore(GLib.GType.Boolean, GLib.GType.String, GLib.GType.Int);
            tvMinutes.Model = listStore;
            tvMinutes.Data["EditorType"] = CronEditorType.Minutes;

            TreeViewColumn vc = new TreeViewColumn();
            vc.Title = "Checked";
            CellRenderer cellRenderer = new CellRendererToggle();
            vc.PackStart(cellRenderer, true);
            vc.AddAttribute(cellRenderer, "active", 0);
            tvMinutes.AppendColumn(vc);

            vc = new TreeViewColumn();
            vc.Title = "Minute";
            cellRenderer = new CellRendererText();
            vc.PackStart(cellRenderer, true);
            vc.AddAttribute(cellRenderer, "text", 1);
            tvMinutes.AppendColumn(vc);

            for (int i=0; i<60; i++)
            {
                listStore.AppendValues(true, i.ToString(), i);
            }
        }

        private void SetUpCronHourEditor()
        {
            ListStore listStore = new ListStore(GLib.GType.Boolean, GLib.GType.String, GLib.GType.Int);
            tvHours.Model = listStore;
            tvHours.Data["EditorType"] = CronEditorType.Hours;

            TreeViewColumn vc = new TreeViewColumn();
            vc.Title = "Checked";
            CellRenderer cellRenderer = new CellRendererToggle();
            vc.PackStart(cellRenderer, true);
            vc.AddAttribute(cellRenderer, "active", 0);
            tvHours.AppendColumn(vc);

            vc = new TreeViewColumn();
            vc.Title = "Minute";
            cellRenderer = new CellRendererText();
            vc.PackStart(cellRenderer, true);
            vc.AddAttribute(cellRenderer, "text", 1);
            tvHours.AppendColumn(vc);

            for (int i=0; i<24; i++)
            {
                listStore.AppendValues(true, i.ToString(), i);
            }
        }

        private void SetUpCronDayEditor()
        {
            ListStore listStore = new ListStore(GLib.GType.Boolean, GLib.GType.String, GLib.GType.Int);
            tvDays.Model = listStore;
            tvDays.Data["EditorType"] = CronEditorType.Days;

            TreeViewColumn vc = new TreeViewColumn();
            vc.Title = "Checked";
            CellRenderer cellRenderer = new CellRendererToggle();
            vc.PackStart(cellRenderer, true);
            vc.AddAttribute(cellRenderer, "active", 0);
            tvDays.AppendColumn(vc);

            vc = new TreeViewColumn();
            vc.Title = "Day";
            cellRenderer = new CellRendererText();
            vc.PackStart(cellRenderer, true);
            vc.AddAttribute(cellRenderer, "text", 1);
            tvDays.AppendColumn(vc);

            for (int i=1; i<32; i++)
            {
                listStore.AppendValues(true, i.ToString(), i);
            }
        }

        private void SetUpCronMonthEditor()
        {
            ListStore listStore = new ListStore(GLib.GType.Boolean, GLib.GType.String, GLib.GType.Int);
            tvMonths.Model = listStore;
            tvMonths.Data["EditorType"] = CronEditorType.Months;

            TreeViewColumn vc = new TreeViewColumn();
            vc.Title = "Checked";
            CellRenderer cellRenderer = new CellRendererToggle();
            vc.PackStart(cellRenderer, true);
            vc.AddAttribute(cellRenderer, "active", 0);
            tvMonths.AppendColumn(vc);

            vc = new TreeViewColumn();
            vc.Title = "Month";
            cellRenderer = new CellRendererText();
            vc.PackStart(cellRenderer, true);
            vc.AddAttribute(cellRenderer, "text", 1);
            tvMonths.AppendColumn(vc);

            for (int i=0; i<MONTHS.Length; i++)
            {
                listStore.AppendValues(true, MONTHS[i], i+1);
            }
        }

        private void SetUpCronDOWEditor()
        {
            ListStore listStore = new ListStore(GLib.GType.Boolean, GLib.GType.String, GLib.GType.Int);
            tvDOWs.Model = listStore;
            tvDOWs.Data["EditorType"] = CronEditorType.DaysOfWeek;

            TreeViewColumn vc = new TreeViewColumn();
            vc.Title = "Checked";
            CellRenderer cellRenderer = new CellRendererToggle();
            vc.PackStart(cellRenderer, true);
            vc.AddAttribute(cellRenderer, "active", 0);
            tvDOWs.AppendColumn(vc);

            vc = new TreeViewColumn();
            vc.Title = "DOW";
            cellRenderer = new CellRendererText();
            vc.PackStart(cellRenderer, true);
            vc.AddAttribute(cellRenderer, "text", 1);
            tvDOWs.AppendColumn(vc);

            for (int i=0; i<DOWS.Length; i++)
            {
                listStore.AppendValues(true, DOWS[i], i);
            }
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

        protected virtual void SelectAllMinutesClicked (object sender, System.EventArgs e)
        {
            SetCheckCBList(tvMinutes.Model as ListStore, true);
            CreateMinuteSpecification();
        }

        protected virtual void ClearMinutesClicked (object sender, System.EventArgs e)
        {
            SetCheckCBList(tvMinutes.Model as ListStore, false);
            CreateMinuteSpecification();
        }

        protected virtual void SelectAllHoursClicked (object sender, System.EventArgs e)
        {
            SetCheckCBList(tvHours.Model as ListStore, true);
            CreateHourSpecification();
        }

        protected virtual void ClearHoursClicked (object sender, System.EventArgs e)
        {
            SetCheckCBList(tvHours.Model as ListStore, false);
            CreateHourSpecification();
        }

        protected virtual void SelectAllDaysClicked (object sender, System.EventArgs e)
        {
            SetCheckCBList(tvDays.Model as ListStore, true);
            CreateDaySpecification();
        }

        protected virtual void ClearDaysClicked (object sender, System.EventArgs e)
        {
            SetCheckCBList(tvDays.Model as ListStore, false);
            CreateDaySpecification();
        }

        protected virtual void SelectAllMonthsClicked (object sender, System.EventArgs e)
        {
            SetCheckCBList(tvMonths.Model as ListStore, true);
            CreateMonthSpecification();
        }

        protected virtual void ClearMonthsClicked (object sender, System.EventArgs e)
        {
            SetCheckCBList(tvMonths.Model as ListStore, false);
            CreateMonthSpecification();
        }

        protected virtual void SelectAllDOWClicked (object sender, System.EventArgs e)
        {
            SetCheckCBList(tvDOWs.Model as ListStore, true);
            CreateDOWSpecification();
        }

        protected virtual void ClearDOWClicked (object sender, System.EventArgs e)
        {
            SetCheckCBList(tvDOWs.Model as ListStore, false);
            CreateDOWSpecification();
        }

        private void ToggleValue(TreeView treeView)
        {
            TreeModel model = null;
            TreeIter iter = TreeIter.Zero;

            if (treeView.Selection.GetSelected(out model, out iter))
            {
                bool bValue = (bool) model.GetValue(iter, 0);
                model.SetValue(iter, 0, ! bValue);
            }

            switch ((CronEditorType) treeView.Data["EditorType"])
            {
            case CronEditorType.Minutes:
                CreateMinuteSpecification();
                break;

            case CronEditorType.Hours:
                CreateHourSpecification();
                break;

            case CronEditorType.Days:
                CreateDaySpecification();
                break;

            case CronEditorType.Months:
                CreateMonthSpecification();
                break;

            case CronEditorType.DaysOfWeek:
                CreateDOWSpecification();
                break;
            }

            evListCtl.UpdateSelected();
        }

        protected virtual void CronEditorCursorChanged (object sender, System.EventArgs e)
        {
            log.DebugFormat("CronEditorCursorChanged: {0}", sender.GetType().Name);
            ToggleValue(sender as TreeView);
        }

        private void CreateMinuteSpecification()
        {
            ArrayList spec = CreateSpecification(tvMinutes.Model as ListStore, 2, "0-59", CronValueFactory.GetMinuteCreator());

            string rawCriteria = txtCriteria.Text;
            CronSpecification cs = new CronSpecification(rawCriteria);

            cs.Minutes = spec;

            txtCriteria.Text = cs.ToString();
        }

        private void CreateHourSpecification()
        {
            ArrayList spec = CreateSpecification(tvHours.Model as ListStore, 2, "0-23", CronValueFactory.GetHourCreator());

            string rawCriteria = txtCriteria.Text;
            CronSpecification cs = new CronSpecification(rawCriteria);

            cs.Hours = spec;

            txtCriteria.Text = cs.ToString();
        }

        private void CreateDaySpecification()
        {
            ArrayList spec = CreateSpecification(tvDays.Model as ListStore, 2, "1-31", CronValueFactory.GetDayCreator());

            string rawCriteria = txtCriteria.Text;
            CronSpecification cs = new CronSpecification(rawCriteria);

            cs.Days = spec;

            txtCriteria.Text = cs.ToString();
        }

        private void CreateMonthSpecification()
        {
            ArrayList spec = CreateSpecification(tvMonths.Model as ListStore, 2, "1-12", CronValueFactory.GetMonthCreator());

            string rawCriteria = txtCriteria.Text;
            CronSpecification cs = new CronSpecification(rawCriteria);

            cs.Months = spec;

            txtCriteria.Text = cs.ToString();
        }

        private void CreateDOWSpecification()
        {
            ArrayList spec = CreateSpecification(tvDOWs.Model as ListStore, 2, "0-6", CronValueFactory.GetDOWCreator());

            string rawCriteria = txtCriteria.Text;
            CronSpecification cs = new CronSpecification(rawCriteria);

            cs.DaysOfWeek = spec;

            txtCriteria.Text = cs.ToString();
        }

        private ArrayList CreateSpecification(ListStore model, int valueIndex, string wildcardPattern, CronValueFactory.CronValueCreator creator)
        {
            ArrayList specArray = new ArrayList();
            TreeIter iter = TreeIter.Zero;
            bool done = false;
            bool more = model.GetIterFirst(out iter);
            bool isChecked = false;
            while (! done)
            {
                string spec = "";
                
                // Find the first checked item
                bool foundFirst = false;
                int firstValueInRange = -1;
                while (more && ! foundFirst)
                {
                    isChecked = (bool) model.GetValue(iter, 0);
                    if (isChecked)
                    {
                        foundFirst = true;
                        firstValueInRange = (int) model.GetValue(iter, valueIndex);
                    }
                    else
                    {
                        more = model.IterNext(ref iter);
                    }
                }

                if (foundFirst)
                {
                    spec += firstValueInRange.ToString();
                    bool foundEndOfContiguous = false;
                    int secondValueInRange = -1;

                    if (more)
                    {
                        more = model.IterNext(ref iter);
                        while (more && ! foundEndOfContiguous)
                        {
                            isChecked = (bool) model.GetValue(iter, 0);
                            if (isChecked)
                            {
                                secondValueInRange = (int) model.GetValue(iter, valueIndex);
                                more = model.IterNext(ref iter);
                            }
                            else
                            {
                                foundEndOfContiguous = true;
                            }
                        }

                        if (foundEndOfContiguous)
                        {
                            if (secondValueInRange >= 0)
                            {
                                spec += string.Format("-{0}", secondValueInRange);
                            }
                        }
                        else if (secondValueInRange >= 0)
                        {
                            spec += string.Format("-{0}", secondValueInRange);
                        }
                        specArray.Add(creator.CreateCronValue(spec));
                        if (more)
                        {
                            more = model.IterNext(ref iter);
                        }
                        else
                        {
                            done = true;
                        }
                    }
                    else
                    {
                        done = true;
                    }
                }
                else
                {
                    done = true;
                }
            }

            if (specArray.Count == 0)
            {
                specArray.Add(creator.CreateCronValue("*"));
            }
            else if (specArray.Count == 1)
            {
                string specStr = specArray[0].ToString();
                if (specStr.Equals(wildcardPattern))
                {
                    specArray.Clear();
                    specArray.Add(creator.CreateCronValue("*"));
                }
            }

            return specArray;
        }

//        protected virtual void ApplyButtonClicked (object sender, System.EventArgs e)
//        {
//            switch (evListCtl.SelectedLevel)
//            {
//            case EffectiveDateLevels.EffectiveDate:
//                UpdateEffectiveDate();
//                break;
//
//            case EffectiveDateLevels.ValueCriteria:
//                UpdateValueCriteria();
//                break;
//            }
//        }

//        private void UpdateEffectiveDate()
//        {
//            DateTime startDate = DateTime.MinValue;
//            if (cbStartDateNull.Active)
//            {
//                startDate = calStartDate.Date;
//                
//                // Get the time
//                string startTime = txtStartTime.Text;
//                if (startTime.Length > 0)
//                {
//                    DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
//                    DateTime stTime = DateTime.ParseExact(startTime, "HH:mm", dtfi);
//
//                    startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, stTime.Hour, stTime.Minute, 0);
//                }
//            }
//            
//            DateTime endDate = DateTime.MinValue;
//            if (cbEndDateNull.Active)
//            {
//                endDate = calEndDate.Date;
//                
//                // Get the time
//                string endTime = txtEndTime.Text;
//                if (endTime.Length > 0)
//                {
//                    DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
//                    DateTime stTime = DateTime.ParseExact(endTime, "HH:mm", dtfi);
//
//                    endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, stTime.Hour, stTime.Minute, 0);
//                }
//            }
//
//            Domain domain = evListCtl.GetSelectedDomain();
//            domain.SetValue("EffectiveStartDate", startDate);
//            domain.SetValue("EffectiveEndDate", endDate);
//
//            log.DebugFormat("Applying values, EffectiveStartDate = {0}, EffectiveEndDate = {1}", startDate, endDate);
//
//            evListCtl.UpdateSelected();
//        }

//        private void UpdateValueCriteria()
//        {
//            Domain domain = evListCtl.GetSelectedDomain();
//
//            string criteria = txtCriteria.Text;
//            string val = txtValue.Text;
//
//            log.DebugFormat("Applying values, Criteria = {0}, Value = {1}", criteria, val);
//
//            domain.SetValue("RawCriteria", criteria);
//            domain.SetValue("Value", val);
//
//            evListCtl.UpdateSelected();
//        }

//        protected virtual void StartDateNullToggled (object sender, System.EventArgs e)
//        {
//            if (cbStartDateNull.Active)
//            {
//                calStartDate.Sensitive = true;
//                txtStartTime.Sensitive = true;
//            }
//            else
//            {
//                calStartDate.Sensitive = false;
//                txtStartTime.Sensitive = false;
//            }
//        }

//        protected virtual void EndDateNullToggled (object sender, System.EventArgs e)
//        {
//            if (cbEndDateNull.Active)
//            {
//                calEndDate.Sensitive = true;
//                txtEndTime.Sensitive = true;
//            }
//            else
//            {
//                calEndDate.Sensitive = false;
//                txtEndTime.Sensitive = false;
//            }
//        }

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
    }
}
