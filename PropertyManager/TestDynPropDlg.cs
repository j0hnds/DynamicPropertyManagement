
using System;
using System.Globalization;
using Gtk;
using STUtils;
using DomainCore;
using DynPropertyDomain;

namespace PropertyManager
{
    
    
    public partial class TestDynPropDlg : Gtk.Dialog
    {
        private TestDynPropertyTextViewControl tvDynPropCtl;
        private DynamicProperty dynProp;
        
        public TestDynPropDlg()
        {
            this.Build();

            tvDynPropCtl = new TestDynPropertyTextViewControl(tvPropertyInformation);
        }

        protected virtual void RevertSimulatedDateClicked (object sender, System.EventArgs e)
        {
            // Get the current date/time
            DateTime dtNow = DateTime.Now;

            calSimEffectiveDate.Date = dtNow;

            txtSimTime.Text = dtNow.ToString("HH:mm");

            ApplySimulatedDateTime(dtNow);
        }           

        protected virtual void ApplySimulatedDateClicked (object sender, System.EventArgs e)
        {
            // Collect up the date/time information
            DateTime calDt = calSimEffectiveDate.Date;

            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            DateTime stTime = (txtSimTime.Text.Length > 0) ? DateTime.ParseExact(txtSimTime.Text, "HH:mm", dtfi) : new DateTime(calDt.Year, calDt.Month, calDt.Day, 0, 0, 0);

            DateTime simDt = new DateTime(calDt.Year, calDt.Month, calDt.Day, stTime.Hour, stTime.Minute, 0);

            ApplySimulatedDateTime(simDt);
        }

        private void ApplySimulatedDateTime(DateTime simDateTime)
        {
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
        
        public virtual bool DoModal(Window parentWindow, Domain domain)
        {
            bool ok = false;

            dynProp = (DynamicProperty) domain;
            
            TransientFor = parentWindow;

            // DomainToControls(domain);
            tvDynPropCtl.Render(DomainRenderer.Render(domain, "Summary"));

            DateTime dtNow = DateTime.Now;
            calSimEffectiveDate.Date = dtNow;
            txtSimTime.Text = dtNow.ToString("HH:mm");

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
