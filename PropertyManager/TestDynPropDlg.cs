
using System;
using System.Globalization;
using Gtk;
using STUtils;
using DomainCore;
using DynPropertyDomain;
using log4net;

namespace PropertyManager
{
    
    /// <summary>
    /// Dialog to test different effective values for a Dynamic Property.
    /// </summary>
    public partial class TestDynPropDlg : Gtk.Dialog
    {
        /// <summary>
        /// Wrapper for text view.
        /// </summary>
        private TestDynPropertyTextViewControl tvDynPropCtl;
        /// <summary>
        /// The Dynamic Property being tested.
        /// </summary>
        private DynamicProperty dynProp;
        /// <summary>
        /// The logger for the dialog.
        /// </summary>
        private ILog log;

        /// <summary>
        /// Constructs a new TestDynPropDlg dialog.
        /// </summary>
        public TestDynPropDlg()
        {
            this.Build();
            log = LogManager.GetLogger(GetType().Name);

            tvDynPropCtl = new TestDynPropertyTextViewControl(tvPropertyInformation);
        }

        /// <summary>
        /// Signal handler for click of Revert button.
        /// </summary>
        /// <param name="sender">
        /// The Revert button.
        /// </param>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected virtual void RevertSimulatedDateClicked (object sender, System.EventArgs e)
        {
            // Get the current date/time
            DateTime dtNow = DateTime.Now;

            calSimEffectiveDate.Date = dtNow;
//            calSimEffectiveDate.Date = dtNow;
//
//            txtSimTime.Text = dtNow.ToString("HH:mm");

            ApplySimulatedDateTime(dtNow);
        }           

        /// <summary>
        /// The signal handler for click of Apply button.
        /// </summary>
        /// <param name="sender">
        /// The Apply button.
        /// </param>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected virtual void ApplySimulatedDateClicked (object sender, System.EventArgs e)
        {
            // Collect up the date/time information
//            DateTime calDt = calSimEffectiveDate.Date;
//
//            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
//            DateTime stTime = (txtSimTime.Text.Length > 0) ? DateTime.ParseExact(txtSimTime.Text, "HH:mm", dtfi) : new DateTime(calDt.Year, calDt.Month, calDt.Day, 0, 0, 0);
//
//            DateTime simDt = new DateTime(calDt.Year, calDt.Month, calDt.Day, stTime.Hour, stTime.Minute, 0);
            DateTime simDt = calSimEffectiveDate.Date;

            ApplySimulatedDateTime(simDt);
        }

        /// <summary>
        /// Use the simulated date/time to determine the effective value.
        /// </summary>
        /// <param name="simDateTime">
        /// The simulated date time to apply.
        /// </param>
        private void ApplySimulatedDateTime(DateTime simDateTime)
        {
            log.DebugFormat("Simulating date: {0}", simDateTime);

            string effValue = dynProp.GetEffectiveValue(simDateTime) as string;
            if (effValue != null)
            {
                txtEffectiveValue.Text = effValue;
            }
            else
            {
                txtEffectiveValue.Text = "";
            }
        }

        /// <summary>
        /// Display the dialog modally.
        /// </summary>
        /// <param name="parentWindow">
        /// The parent window to the dialog
        /// </param>
        /// <param name="domain">
        /// The DynamicProperty domain object to evaluate.
        /// </param>
        /// <returns>
        /// <c>true</c> if the user clicked OK.
        /// </returns>
        public virtual bool DoModal(Window parentWindow, Domain domain)
        {
            bool ok = false;

            dynProp = (DynamicProperty) domain;
            
            TransientFor = parentWindow;

            // DomainToControls(domain);
            tvDynPropCtl.Render(DomainRenderer.Render(domain, "Summary"));

            calSimEffectiveDate.Date = DateTime.Now;
//            DateTime dtNow = DateTime.Now;
//            calSimEffectiveDate.Date = dtNow;
//            txtSimTime.Text = dtNow.ToString("HH:mm");

            int response = Run();
            if (response == Gtk.ResponseType.Ok.value__)
            {
                ok = true;
                // ControlsToDomain(domain);
            }

            Destroy();

            return ok;
        }
    }
}
