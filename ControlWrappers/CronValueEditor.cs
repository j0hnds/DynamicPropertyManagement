
using System;
using System.ComponentModel;
using System.Collections;
using Gtk;
using CronUtils;

namespace ControlWrappers
{
    /// <summary>
    /// The various categories of information in the cron specification.
    /// </summary>
    public enum CronValueType
    {
        Minutes = 0,
        Hours = 1,
        Days = 2,
        Months = 3,
        DaysOfWeek = 4
    }

    /// <summary>
    /// A custom Gtk control implementing a cron editor.
    /// </summary>
    [System.ComponentModel.ToolboxItem(true)]
    public partial class CronValueEditor : Gtk.Bin
    {
        /// <summary>
        /// The list of short names of the months. Used when the editor
        /// type is <c>Months</c>.
        /// </summary>
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

        /// <summary>
        /// The list of short day of week names used if the editor type
        /// is <c>DaysOfWeek</c>.
        /// </summary>
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

        /// <summary>
        /// The data model for the list control.
        /// </summary>
        private ListStore listStore;
        /// <summary>
        /// The type of editor being constructed.
        /// </summary>
        private CronValueType valueType = CronValueType.Minutes;

        /// <summary>
        /// Event fired when the value of the cron editor has changed.
        /// </summary>
        public event EventHandler Changed;

        /// <summary>
        /// Constructs a new CronValueEditor control.
        /// </summary>
        public CronValueEditor()
        {
            this.Build();

            SetUpTreeListControl();
        }

        /// <summary>
        /// Helper method to notify subscribers when the value of the cron editor
        /// has changed.
        /// </summary>
        private void NotifyChanged()
        {
            EventHandler handler = Changed;

            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        /// <value>
        /// The list of cron specifications that matches the settings in the
        /// control.
        /// </value>
        public ArrayList SpecificationList
        {
            get { return CreateSpecification(); }
            set { SetCronEditorValues(value); }
        }

        /// <summary>
        /// Sets up the control to reflect the specified cron array list.
        /// </summary>
        /// <param name="cronSpecs">
        /// An array of CronValues.
        /// </param>
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

        /// <summary>
        /// Check to see if specified value is 'effective' with respect to the
        /// cron specification array.
        /// </summary>
        /// <param name="cronSpecs">
        /// The array of CronValue specifications to check against.
        /// </param>
        /// <param name="val">
        /// The value to check the effectiveness of.
        /// </param>
        /// <returns>
        /// <c>true</c> if the value is effective with respect to the array of
        /// cron values.
        /// </returns>
        private static bool IsValueEffective(ArrayList cronSpecs, int val)
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

        /// <value>
        /// The type of editor to construct.
        /// </value>
        /// <remarks>
        /// This should be a CronValueType, but the stupid property editor in the
        /// IDE won't recognize it as a valid property, so we are using the integer value
        /// of the enumeration instead. Hack.
        /// </remarks>
        [DefaultValue(0)]
        public int ValueType
        {
            get { return (int) valueType; }
            set
            {
                // valueType = value;
                switch (value)
                {
                case 0:
                    valueType = CronValueType.Minutes;
                    break;

                case 1:
                    valueType = CronValueType.Hours;
                    break;

                case 2:
                    valueType = CronValueType.Days;
                    break;

                case 3:
                    valueType = CronValueType.Months;
                    break;

                case 4:
                    valueType = CronValueType.DaysOfWeek;
                    break;
                }
                SetUpCronValues();
            }
        }

        /// <summary>
        /// Sets up the list items to reflect the type of editor we are going
        /// to be presenting.
        /// </summary>
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

        /// <summary>
        /// Sets up the list to display minutes.
        /// </summary>
        private void SetUpMinutes()
        {
            for (int i=0; i<60; i++)
            {
                listStore.AppendValues(false, i.ToString(), i);
            }
        }

        /// <summary>
        /// Sets up the list to display hours.
        /// </summary>
        private void SetUpHours()
        {
            for (int i=0; i<24; i++)
            {
                listStore.AppendValues(false, i.ToString(), i);
            }
        }

        /// <summary>
        /// Sets up the list to display days.
        /// </summary>
        private void SetUpDays()
        {
            for (int i=1; i<32; i++)
            {
                listStore.AppendValues(false, i.ToString(), i);
            }
        }

        /// <summary>
        /// Sets up the list to display months.
        /// </summary>
        private void SetUpMonths()
        {
            for (int i=1; i<=12; i++)
            {
                listStore.AppendValues(false, MONTHS[i-1], i);
            }
        }

        /// <summary>
        /// Sets up the list to display days of week.
        /// </summary>
        private void SetUpDaysOfWeek()
        {
            for (int i=0; i<DOWS.Length; i++)
            {
                listStore.AppendValues(false, DOWS[i], i);
            }
        }

        /// <summary>
        /// Sets up the basic look/feel of the list control.
        /// </summary>
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

            SetUpCronValues();
        }

        /// <summary>
        /// Sets all the toggles in the check list box to the specified boolean
        /// value.
        /// </summary>
        /// <param name="setCheck">
        /// <c>true</c> if the items are to be all checked.
        /// </param>
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

        /// <summary>
        /// Signal handler for a click of the Select All button.
        /// </summary>
        /// <param name="sender">
        /// Reference to the Select All button.
        /// </param>
        /// <param name="e">
        /// Reference to the event arguments.
        /// </param>
        protected virtual void SelectAllActivated (object sender, System.EventArgs e)
        {
            SetCheckCBList(true);
            NotifyChanged();
        }

        /// <summary>
        /// Signal handler for click of the Clear button.
        /// </summary>
        /// <param name="sender">
        /// Reference to the Clear button
        /// </param>
        /// <param name="e">
        /// Reference to the event arguments.
        /// </param>
        protected virtual void ClearActivated (object sender, System.EventArgs e)
        {
            SetCheckCBList(false);
            NotifyChanged();
        }

        /// <summary>
        /// Signal handler for cursor change in tree list.
        /// </summary>
        /// <param name="sender">
        /// Reference to the tree list.
        /// </param>
        /// <param name="e">
        /// Reference to the event arguments.
        /// </param>
        protected virtual void TreeListCursorChanged (object sender, System.EventArgs e)
        {
            ToggleValue();
        }

        /// <summary>
        /// Toggles the check box on the currently selected tree list item.
        /// </summary>
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

        /// <summary>
        /// Constructs an array of cron values from the current settings of the control.
        /// </summary>
        /// <returns>
        /// An array cron values representing the current settings of the control.
        /// </returns>
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

        /// <summary>
        /// Constructs an array of cron values from the current settings of the control.
        /// </summary>
        /// <param name="valueIndex">
        /// The index of the value column of the tree list store.
        /// </param>
        /// <param name="wildcardPattern">
        /// The cron value pattern which indicates that the value represents all possible values.
        /// </param>
        /// <param name="creator">
        /// The factory method that creates CronValues of the appropriate type.
        /// </param>
        /// <returns>
        /// An array cron values representing the current settings of the control.
        /// </returns>
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
