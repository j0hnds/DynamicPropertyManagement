
using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace CronUtils
{
    
    
    public class CronSpecification
    {
        private const int MINUTE = 0;
        private const int HOUR = 1;
        private const int DAY = 2;
        private const int MONTH = 3;
        private const int DOW = 4;

        private static readonly Regex SHORTCUT_PATTERN = new Regex("^@(yearly|annually|monthly|weekly|daily|midnight|hourly)$");

        private static readonly Hashtable SHORTCUT_MAP = new Hashtable();

        static CronSpecification()
        {
            SHORTCUT_MAP.Add("yearly", "0 0 1 1 *");
            SHORTCUT_MAP.Add("annually", "0 0 1 1 *");
            SHORTCUT_MAP.Add("monthly", "0 0 1 * *");
            SHORTCUT_MAP.Add("weekly", "0 0 * * 0");
            SHORTCUT_MAP.Add("daily", "0 0 * * *");
            SHORTCUT_MAP.Add("midnight", "0 0 * * *");
            SHORTCUT_MAP.Add("hourly", "0 * * * *");
        }

        private string rawSpecification;
        private string[] specification;
        private ArrayList minutes;
        private ArrayList hours;
        private ArrayList days;
        private ArrayList months;
        private ArrayList daysOfWeek;

        public CronSpecification(string rawSpecification)
        {
            if (rawSpecification == null)
            {
                throw new ArgumentNullException("rawSpecification");
            }

            this.rawSpecification = rawSpecification;

            // Deal with the shortcut possibility
            Match m = SHORTCUT_PATTERN.Match(rawSpecification);
            if (m.Success)
            {
                string shortcut = m.Groups[1].Value;
                if (! SHORTCUT_MAP.ContainsKey(shortcut))
                {
                    throw new Exception("Mismatch between shortcut regular expression and shortcut mapping for value: " + shortcut);
                }

                this.rawSpecification = (string) SHORTCUT_MAP[shortcut]; 
            }

            // Split the specification into its components
            this.specification = this.rawSpecification.Split(' ');
            if (specification.Length != 5)
            {
                throw new ArgumentException("Invalid cron specification: " + rawSpecification, "rawSpecification");
            }

            minutes = new ArrayList();
            hours = new ArrayList();
            days = new ArrayList();
            months = new ArrayList();
            daysOfWeek = new ArrayList();

            ParseSpecification(specification[MINUTE], minutes, CronValueFactory.GetMinuteCreator());
            ParseSpecification(specification[HOUR], hours, CronValueFactory.GetHourCreator());
            ParseSpecification(specification[DAY], days, CronValueFactory.GetDayCreator());
            ParseSpecification(specification[MONTH], months, CronValueFactory.GetMonthCreator());
            ParseSpecification(specification[DOW], daysOfWeek, CronValueFactory.GetDOWCreator());
            
        }

        private void ParseSpecification(string specification, ArrayList cronValues, CronValueFactory.CronValueCreator cronValueCreator)
        {
            if (specification == null)
            {
                throw new ArgumentNullException("specification");
            }

            string[] specComponents = specification.Split(',');

            foreach (string specComponent in specComponents)
            {
                cronValues.Add(cronValueCreator.CreateCronValue(specComponent));
            }
        }

        public bool IsDateEffective(DateTime dateToCheck)
        {
            bool effective = false;

            bool minutesOK = CheckMinutes(dateToCheck.Minute);
            bool hoursOK = CheckHours(dateToCheck.Hour);
            bool daysOK = CheckDays(dateToCheck.Day);
            bool monthsOK = CheckMonths(dateToCheck.Month);
            bool dowOK = CheckDaysOfWeek(dateToCheck.DayOfWeek.value__);

            effective = minutesOK && hoursOK && daysOK && monthsOK && dowOK;

            return effective;
        }

        public bool IsDateEffective()
        {
            return IsDateEffective(new DateTime());
        }

        private bool CheckMinutes(int minute)
        {
            return CheckValues(minutes, minute);
        }

        private bool CheckHours(int hour)
        {
            return CheckValues(hours, hour);
        }

        private bool CheckDays(int day)
        {
            return CheckValues(days, day);
        }

        private bool CheckMonths(int month)
        {
            return CheckValues(months, month);
        }

        private bool CheckDaysOfWeek(int dow)
        {
            return CheckValues(daysOfWeek, dow);
        }

        private bool CheckValues(ArrayList valuePatterns, int value)
        {
            bool effective = false;

            foreach (CronEffectiveValue cronValue in valuePatterns)
            {
                effective = cronValue.IsEffective(value);
                if (effective)
                {
                    break;
                }
            }

            return effective;
        }

        public string RawSpecification
        {
            get { return this.rawSpecification; }
        }
    }
}
