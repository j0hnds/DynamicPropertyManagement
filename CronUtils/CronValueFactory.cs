
using System;
using System.Text.RegularExpressions;

namespace CronUtils
{
    
    /// <summary>
    /// A factory class for the construction of CronValue objects.
    /// </summary>
    public class CronValueFactory
    {
        // The array of days of the week. Using an array here so we can map the
        // name of the day of week to its index number
        private static readonly string[] DOWS = { 
            "sun", "mon", "tue", "wed", "thu", "fri", "sat" 
        };

        // The array of months of the year. Using an array here so we can map
        // the name of the month to its index number.
        private static readonly string[] MONTHS = { 
            "", "jan", "feb", "mar", "apr", "may", "jun", "jul", 
            "aug", "sep", "oct", "nov", "dec" 
        };

        // The part of the regular expression to match the days of the week.
        private static readonly string DOW_EXPRESSION = "sun|mon|tue|wed|thu|fri|sat";

        // The partof the regular expression to match the months.
        private static readonly string MONTH_EXPRESSION = "jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dev";

        // Wildcard value
        private static readonly string WILDCARD_PATTERN = "*";

        // The regular expression pattern identifying a cron value that is just
        // a single integer value.
        private static readonly Regex SINGLE_VALUE_PATTERN = new Regex("^([0-9]+)$");

        private static readonly Regex DOW_SINGLE_VALUE_PATTERN = new Regex("^(" + DOW_EXPRESSION + "|[0-9]+)$");

        private static readonly Regex MONTH_SINGLE_VALUE_PATTERN = new Regex("^(" + MONTH_EXPRESSION + "|[0-9]+)$");

        private static readonly Regex RANGE_PATTERN = new Regex("^([0-9]+)-([0-9]+)$");

        private static readonly Regex DOW_RANGE_PATTERN = new Regex("^(" + DOW_EXPRESSION + "|[0-9])-(" + DOW_EXPRESSION + "|[0-9])$");

        private static readonly Regex MONTH_RANGE_PATTERN = new Regex("^(" + MONTH_EXPRESSION + "|[0-9]+)-(" + MONTH_EXPRESSION + "|[0-9]+)$");

        private static readonly Regex STEP_PATTERN = new Regex("^\\*/([0-9]+)$");

        private static readonly Regex DOW_PATTERN = new Regex("^(" + DOW_EXPRESSION + ")$");

        private static readonly Regex MONTH_PATTERN = new Regex("^(" + MONTH_EXPRESSION + ")$");

        private const int MINUTE_LOWER_LIMIT = 0;
        private const int MINUTE_UPPER_LIMIT = 59;
        private const int HOUR_LOWER_LIMIT = 0;
        private const int HOUR_UPPER_LIMIT = 23;
        private const int DAY_LOWER_LIMIT = 1;
        private const int DAY_UPPER_LIMIT = 31;
        private const int MONTH_LOWER_LIMIT = 1;
        private const int MONTH_UPPER_LIMIT = 12;
        private const int DOW_LOWER_LIMIT = 0;
        private const int DOW_UPPER_LIMIT = 6;

        // Static singleton members to hold the various CronValueCreator objects.
        private static CronValueCreator minuteCreator = null;
        private static CronValueCreator hourCreator = null;
        private static CronValueCreator dayCreator = null;
        private static CronValueCreator monthCreator = null;
        private static CronValueCreator dowCreator = null;

        public interface CronValueCreator
        {
            CronEffectiveValue CreateCronValue(string valueSpec);
        }

        private class MinuteValueCreator : CronValueCreator
        {

            #region CronValueCreator implementation
            public CronEffectiveValue CreateCronValue (string valueSpec)
            {
                return ParseCronValue(MINUTE_LOWER_LIMIT, MINUTE_UPPER_LIMIT, null, valueSpec);
            }
            #endregion
        }
        
        private class HourValueCreator : CronValueCreator
        {

            #region CronValueCreator implementation
            public CronEffectiveValue CreateCronValue (string valueSpec)
            {
                return ParseCronValue(HOUR_LOWER_LIMIT, HOUR_UPPER_LIMIT, null, valueSpec);
            }
            #endregion
        }
        
        private class DayValueCreator : CronValueCreator
        {

            #region CronValueCreator implementation
            public CronEffectiveValue CreateCronValue (string valueSpec)
            {
                return ParseCronValue(DAY_LOWER_LIMIT, DAY_UPPER_LIMIT, null, valueSpec);
            }
            #endregion
        }
        
        private class MonthValueCreator : CronValueCreator
        {

            #region CronValueCreator implementation
            public CronEffectiveValue CreateCronValue (string valueSpec)
            {
                return ParseMonthCronValue(MONTH_LOWER_LIMIT, MONTH_UPPER_LIMIT, MONTHS, valueSpec);
            }
            #endregion
        }
        
        private class DOWValueCreator : CronValueCreator
        {

            #region CronValueCreator implementation
            public CronEffectiveValue CreateCronValue (string valueSpec)
            {
                return ParseDOWCronValue(DOW_LOWER_LIMIT, DOW_UPPER_LIMIT, DOWS, valueSpec);
            }
            #endregion
        }
        
        private CronValueFactory()
        {
        }

        public static CronValueCreator GetMinuteCreator() {
            if (minuteCreator == null)
            {
                minuteCreator = new MinuteValueCreator();
            }

            return minuteCreator;
        }

        public static CronValueCreator GetHourCreator() {
            if (hourCreator == null)
            {
                hourCreator = new HourValueCreator();
            }

            return hourCreator;
        }

        public static CronValueCreator GetDayCreator() {
            if (dayCreator == null)
            {
                dayCreator = new DayValueCreator();
            }

            return dayCreator;
        }

        public static CronValueCreator GetMonthCreator() {
            if (monthCreator == null)
            {
                monthCreator = new MonthValueCreator();
            }

            return monthCreator;
        }

        public static CronValueCreator GetDOWCreator() {
            if (dowCreator == null)
            {
                dowCreator = new DOWValueCreator();
            }

            return dowCreator;
        }

        private static int FindDOW(string dow)
        {
            int index = -1;

            for (int i=0; i<DOWS.Length && index < 0; i++)
            {
                if (DOWS[i].Equals(dow))
                {
                    index = i;
                }
            }

            return index;
        }

        private static int FindMonth(string month)
        {
            int index = -1;

            for (int i=0; i<MONTHS.Length && index < 0; i++)
            {
                if (MONTHS[i].Equals(month)) 
                {
                    index = i;
                }
            }

            return index;
        }

        private static int GetMonth(string month)
        {
            int value = -1;

            if (MONTH_PATTERN.IsMatch(month))
            {
                value = FindMonth(month);
                if (value < 0)
                {
                    throw new System.ArgumentException("Illegal value", "month");
                }
            }
            else
            {
                try
                {
                    value = int.Parse(month);
                }
                catch (System.FormatException e)
                {
                    throw new System.ArgumentException(e.Message, "month");
                }
            }

            return value;
        }

        private static int GetDOW(string dow)
        {
            int value = -1;

            if (DOW_PATTERN.IsMatch(dow))
            {
                value = FindDOW(dow);
                if (value < 0)
                {
                    throw new System.ArgumentException("Illegal value", "dow");
                }
            }
            else
            {
                try
                {
                    value = int.Parse(dow);
                }
                catch (System.FormatException e)
                {
                    throw new System.ArgumentException(e.Message, "dow");
                }

                if (value == 7)
                {
                    // Convert Sunday from 7 to 0
                    value = 0;
                }
            }

            return value;
        }

        private static CronEffectiveValue ParseCronValue(int lowerLimit,
                                                         int upperLimit,
                                                         string[] names,
                                                         string valueSpec)
        {
            CronEffectiveValue cronValue = null;
            // The matcher to use
            Match m = null;

            if (valueSpec == null) 
            {
                throw new System.ArgumentNullException("valueSpec");
            }

            if (WILDCARD_PATTERN.Equals(valueSpec))
            {
                // This is a wildcard specification (*)
                cronValue = new WildcardCronValue(lowerLimit, upperLimit, names);
            }
            else if ((m = SINGLE_VALUE_PATTERN.Match(valueSpec)).Success)
            {
                // This is a single value specification (just an integer)
                try
                {
                    int singlevalue = int.Parse(m.Groups[1].Value);
                    cronValue = new SingleValueCronValue(lowerLimit, upperLimit, names, singlevalue);
                }
                catch (System.FormatException e)
                {
                    throw new ArgumentException(e.Message, "valueSpec");
                }
            }
            else if ((m = RANGE_PATTERN.Match(valueSpec)).Success)
            {
                // This is a range value specification (e.g. 5-10)
                try
                {
                    int rangeLower = int.Parse(m.Groups[1].Value);
                    int rangeUpper = int.Parse(m.Groups[2].Value);
                    cronValue = new RangeCronValue(lowerLimit, upperLimit, names, rangeLower, rangeUpper);
                }
                catch (System.FormatException e)
                {
                    throw new System.ArgumentException(e.Message, "valueSpec");
                }
            }
            else if ((m = STEP_PATTERN.Match(valueSpec)).Success)
            {
                // This is a step pattern (e.g. */3 - every third time period
                try
                {
                    int stepValue = int.Parse(m.Groups[1].Value);
                    cronValue = new StepCronValue(lowerLimit, upperLimit, names, stepValue);
                }
                catch (System.FormatException e)
                {
                    throw new System.ArgumentException(e.Message, "valueSpec");
                }
            }
            else
            {
                // Unrecognized value pattern
                throw new System.ArgumentException("Invalid value", "valueSpec");
            }

            return cronValue;
        }

        private static CronEffectiveValue ParseDOWCronValue(int lowerLimit,
                                                            int upperLimit,
                                                            string[] names,
                                                            string valueSpec)
        {
            CronEffectiveValue cronValue = null;
            // The matcher to use
            Match m = null;

            if (valueSpec == null) 
            {
                throw new System.ArgumentNullException("valueSpec");
            }

            if (WILDCARD_PATTERN.Equals(valueSpec))
            {
                // This is a wildcard specification (*)
                cronValue = new WildcardCronValue(lowerLimit, upperLimit, names);
            }
            else if ((m = DOW_SINGLE_VALUE_PATTERN.Match(valueSpec)).Success)
            {
                // This is a single value specification (just an integer)
                int dowNumber = GetDOW(m.Groups[1].Value);
                cronValue = new SingleValueCronValue(lowerLimit, upperLimit, names, dowNumber);
            }
            else if ((m = DOW_RANGE_PATTERN.Match(valueSpec)).Success)
            {
                // This is a range value specification (e.g. 5-10)
                int rangeLower = GetDOW(m.Groups[1].Value);
                int rangeUpper = GetDOW(m.Groups[2].Value);
                cronValue = new RangeCronValue(lowerLimit, upperLimit, names, rangeLower, rangeUpper);
            }
            else if ((m = STEP_PATTERN.Match(valueSpec)).Success)
            {
                // This is a step pattern (e.g. */3 - every third time period
                try
                {
                    int stepValue = int.Parse(m.Groups[1].Value);
                    cronValue = new StepCronValue(lowerLimit, upperLimit, names, stepValue);
                }
                catch (System.FormatException e)
                {
                    throw new System.ArgumentException(e.Message, "valueSpec");
                }
            }
            else
            {
                // Unrecognized value pattern
                throw new System.ArgumentException("Invalid value", "valueSpec");
            }

            return cronValue;
        }
        
        private static CronEffectiveValue ParseMonthCronValue(int lowerLimit,
                                                              int upperLimit,
                                                              string[] names,
                                                              string valueSpec)
        {
            CronEffectiveValue cronValue = null;
            // The matcher to use
            Match m = null;

            if (valueSpec == null) 
            {
                throw new System.ArgumentNullException("valueSpec");
            }

            if (WILDCARD_PATTERN.Equals(valueSpec))
            {
                // This is a wildcard specification (*)
                cronValue = new WildcardCronValue(lowerLimit, upperLimit, names);
            }
            else if ((m = MONTH_SINGLE_VALUE_PATTERN.Match(valueSpec)).Success)
            {
                // This is a single value specification (just an integer)
                int monthNumber = GetMonth(m.Groups[1].Value);
                cronValue = new SingleValueCronValue(lowerLimit, upperLimit, names, monthNumber);
            }
            else if ((m = MONTH_RANGE_PATTERN.Match(valueSpec)).Success)
            {
                // This is a range value specification (e.g. 5-10)
                int rangeLower = GetMonth(m.Groups[1].Value);
                int rangeUpper = GetMonth(m.Groups[2].Value);
                cronValue = new RangeCronValue(lowerLimit, upperLimit, names, rangeLower, rangeUpper);
            }
            else if ((m = STEP_PATTERN.Match(valueSpec)).Success)
            {
                // This is a step pattern (e.g. */3 - every third time period
                try
                {
                    int stepValue = int.Parse(m.Groups[1].Value);
                    cronValue = new StepCronValue(lowerLimit, upperLimit, names, stepValue);
                }
                catch (System.FormatException e)
                {
                    throw new System.ArgumentException(e.Message, "valueSpec");
                }
            }
            else
            {
                // Unrecognized value pattern
                throw new System.ArgumentException("Invalid value", "valueSpec");
            }

            return cronValue;
        }
        
    }
}
