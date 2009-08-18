
using System;
using System.Collections;
using Gtk;
using CronUtils;

namespace ControlWrappers
{
    
    public enum CronValueType
    {
        Minutes,
        Hours,
        Days,
        Months,
        DaysOfWeek
    }
    
    [System.ComponentModel.ToolboxItem(true)]
    public partial class CronValueEditor : Gtk.Bin
    {
        
        private static readonly string[] MONTHS = 
        {
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

        private static readonly string[] DOWS = 
        {
            "Sun",
            "Mon",
            "Tue",
            "Wed",
            "Thu",
            "Fri",
            "Sat"
        };

        private ListStore listStore;
        private CronValueType valueType = CronValueType.Minutes;

        public event EventHandler Changed;
        
        public CronValueEditor()
        {
            this.Build();

            SetUpTreeListControl();
        }

        private void NotifyChanged()
        {
            EventHandler handler = Changed;

            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        public ArrayList SpecificationList
        {
            get { return CreateSpecification(); }
            set { SetCronEditorValues(value); }
        }

        private void SetCronEditorValues(ArrayList cronSpecs)
        {
            TreeIter iter = TreeIter.Zero;
            bool more = listStore.GetIterFirst(out iter);
            while (more)
            {
                int val = (int) listStore.GetValue(iter, 2);
                listStore.SetValue(iter, 0, IsValueEffective(cronSpecs, val));
                more = listStore.IterNext(ref iter);
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

        public CronValueType ValueType
        {
            get { return valueType; }
            set
            {
                valueType = value;
                SetUpCronValues();
            }
        }

        private void SetUpCronValues()
        {
            // First, get rid of everything.
            listStore.Clear();

            switch (valueType)
            {
            case CronValueType.Minutes:
                SetUpMinutes();
                break;

            case CronValueType.Hours:
                SetUpHours();
                break;

            case CronValueType.Days:
                SetUpDays();
                break;

            case CronValueType.Months:
                SetUpMonths();
                break;

            case CronValueType.DaysOfWeek:
                SetUpDaysOfWeek();
                break;
            }
        }

        private void SetUpMinutes()
        {
            for (int i=0; i<60; i++)
            {
                listStore.AppendValues(false, i.ToString(), i);
            }
        }

        private void SetUpHours()
        {
            for (int i=0; i<23; i++)
            {
                listStore.AppendValues(false, i.ToString(), i);
            }
        }

        private void SetUpDays()
        {
            for (int i=1; i<32; i++)
            {
                listStore.AppendValues(false, i.ToString(), i);
            }
        }

        private void SetUpMonths()
        {
            for (int i=1; i<=12; i++)
            {
                listStore.AppendValues(false, MONTHS[i-1], i);
            }
        }

        private void SetUpDaysOfWeek()
        {
            for (int i=0; i<DOWS.Length; i++)
            {
                listStore.AppendValues(false, DOWS[i], i);
            }
        }

        private void SetUpTreeListControl()
        {
            listStore = new ListStore(GLib.GType.Boolean, GLib.GType.String, GLib.GType.Int);

            tvList.Model = listStore;

            TreeViewColumn vc = new TreeViewColumn();
            vc.Title = "Checked";
            CellRenderer cellRenderer = new CellRendererToggle();
            vc.PackStart(cellRenderer, true);
            vc.AddAttribute(cellRenderer, "active", 0);
            tvList.AppendColumn(vc);

            vc = new TreeViewColumn();
            vc.Title = "Unit";
            cellRenderer = new CellRendererText();
            vc.PackStart(cellRenderer, true);
            vc.AddAttribute(cellRenderer, "text", 1);
            tvList.AppendColumn(vc);
        }

        private void SetCheckCBList(bool setCheck)
        {
            TreeIter iter = TreeIter.Zero;
            
            bool more = listStore.GetIterFirst(out iter);
            while (more)
            {
                listStore.SetValue(iter, 0, setCheck);
                more = listStore.IterNext(ref iter);
            }
        }

        protected virtual void SelectAllActivated (object sender, System.EventArgs e)
        {
            SetCheckCBList(true);
            NotifyChanged();
        }

        protected virtual void ClearActivated (object sender, System.EventArgs e)
        {
            SetCheckCBList(false);
            NotifyChanged();
        }

        protected virtual void TreeListCursorChanged (object sender, System.EventArgs e)
        {
            ToggleValue();
        }

        private void ToggleValue()
        {
            TreeModel model = null;
            TreeIter iter = TreeIter.Zero;

            if (tvList.Selection.GetSelected(out model, out iter))
            {
                bool bValue = (bool) model.GetValue(iter, 0);
                model.SetValue(iter, 0, ! bValue);
            }

            NotifyChanged();
        }

        private ArrayList CreateSpecification()
        {
            ArrayList al = null;
            
            switch (valueType)
            {
            case CronValueType.Minutes:
                al = CreateSpecification(2, "0-59", CronValueFactory.GetMinuteCreator());
                break;

            case CronValueType.Hours:
                al = CreateSpecification(2, "0-23", CronValueFactory.GetHourCreator());
                break;

            case CronValueType.Days:
                al = CreateSpecification(2, "1-31", CronValueFactory.GetDayCreator());
                break;

            case CronValueType.Months:
                al = CreateSpecification(2, "1-12", CronValueFactory.GetMonthCreator());
                break;

            case CronValueType.DaysOfWeek:
                al = CreateSpecification(2, "0-6", CronValueFactory.GetDOWCreator());
                break;
            }

            return al;
        }
        
        private ArrayList CreateSpecification(int valueIndex, string wildcardPattern, CronValueFactory.CronValueCreator creator)
        {
            ArrayList specArray = new ArrayList();
            TreeIter iter = TreeIter.Zero;
            bool done = false;
            bool more = listStore.GetIterFirst(out iter);
            bool isChecked = false;
            while (! done)
            {
                string spec = "";
                
                // Find the first checked item
                bool foundFirst = false;
                int firstValueInRange = -1;
                while (more && ! foundFirst)
                {
                    isChecked = (bool) listStore.GetValue(iter, 0);
                    if (isChecked)
                    {
                        foundFirst = true;
                        firstValueInRange = (int) listStore.GetValue(iter, valueIndex);
                    }
                    else
                    {
                        more = listStore.IterNext(ref iter);
                    }
                }

                if (foundFirst)
                {
                    spec += firstValueInRange.ToString();
                    bool foundEndOfContiguous = false;
                    int secondValueInRange = -1;

                    if (more)
                    {
                        more = listStore.IterNext(ref iter);
                        while (more && ! foundEndOfContiguous)
                        {
                            isChecked = (bool) listStore.GetValue(iter, 0);
                            if (isChecked)
                            {
                                secondValueInRange = (int) listStore.GetValue(iter, valueIndex);
                                more = listStore.IterNext(ref iter);
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
                            more = listStore.IterNext(ref iter);
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
    }
}
