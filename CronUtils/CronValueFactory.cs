
using System;
using System.Text.RegularExpressions;

namespace CronUtils
{
    
    /// <summary>
    /// A factory class for the construction of CronValue objects.
    /// </summary>
    public class CronValueFactory
    {
        /// <summary>
        /// The array of days of the week. Using an array here so we can map the
        /// name of the day of week to its index number
        /// </summary>
        private static readonly string[] DOWS = { 
            "sun", "mon", "tue", "wed", "thu", "fri", "sat" 
        };

        /// <summary>
        /// The array of months of the year. Using an array here so we can map
        /// the name of the month to its index number.
        /// </summary>
        private static readonly string[] MONTHS = { 
            "", "jan", "feb", "mar", "apr", "may", "jun", "jul", 
            "aug", "sep", "oct", "nov", "dec" 
        };

        /// <summary>
        /// The part of the regular expression to match the days of the week.
        /// </summary>
        private static readonly string DOW_EXPRESSION = "sun|mon|tue|wed|thu|fri|sat";

        /// <summary>
        /// The part of the regular expression to match the months.
        /// </summary>
        private static readonly string MONTH_EXPRESSION = "jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dev";

        /// <summary>
        /// Wildcard value
        /// </summary>
        private static readonly string WILDCARD_PATTERN = "*";

        /// <summary>
        /// The regular expression pattern identifying a cron value that is just
        /// a single integer value.
        /// </summary>
        private static readonly Regex SINGLE_VALUE_PATTERN = new Regex("^([0-9]+)$");

        /// <summary>
        /// The regular expression pattern that matches a numeric or named day-of-week expression
        /// </summary>
        private static readonly Regex DOW_SINGLE_VALUE_PATTERN = new Regex("^(" + DOW_EXPRESSION + "|[0-9]+)$");

        /// <summary>
        /// The regular expression pattern that matches a numeric or named month expression.
        /// </summary>
        private static readonly Regex MONTH_SINGLE_VALUE_PATTERN = new Regex("^(" + MONTH_EXPRESSION + "|[0-9]+)$");

        /// <summary>
        /// The regular expression pattern to match a numeric range expression.
        /// </summary>
        private static readonly Regex RANGE_PATTERN = new Regex("^([0-9]+)-([0-9]+)$");

        /// <summary>
        /// The regular expression pattern to match a numeric or named day-of-week range expression
        /// </summary>
        private static readonly Regex DOW_RANGE_PATTERN = new Regex("^(" + DOW_EXPRESSION + "|[0-9])-(" + DOW_EXPRESSION + "|[0-9])$");

        /// <summary>
        /// The regular expression pattern to match a numeric or named month range expression.
        /// </summary>
        private static readonly Regex MONTH_RANGE_PATTERN = new Regex("^(" + MONTH_EXPRESSION + "|[0-9]+)-(" + MONTH_EXPRESSION + "|[0-9]+)$");

        /// <summary>
        /// The regular expression pattern to match a step expression.
        /// </summary>
        private static readonly Regex STEP_PATTERN = new Regex("^\\*/([0-9]+)$");

        /// <summary>
        /// The regular expression pattern to match a named day-of-week expression.
        /// </summary>
        private static readonly Regex DOW_PATTERN = new Regex("^(" + DOW_EXPRESSION + ")$");

        /// <summary>
        /// The regular expression pattern to match a named month expression.
        /// </summary>
        private static readonly Regex MONTH_PATTERN = new Regex("^(" + MONTH_EXPRESSION + ")$");

        /// <summary>
        /// The lowest acceptable value for a minute
        /// </summary>
        private const int MINUTE_LOWER_LIMIT = 0;
        /// <summary>
        /// The highest acceptable value for a minute.
        /// </summary>
        private const int MINUTE_UPPER_LIMIT = 59;
        /// <summary>
        /// The lowest acceptable value for an hour
        /// </summary>
        private const int HOUR_LOWER_LIMIT = 0;
        /// <summary>
        /// The highest acceptable value for an hour.
        /// </summary>
        private const int HOUR_UPPER_LIMIT = 23;
        /// <summary>
        /// The lowest acceptable value for a day
        /// </summary>
        private const int DAY_LOWER_LIMIT = 1;
        /// <summary>
        /// The highest acceptable value for a day.
        /// </summary>
        private const int DAY_UPPER_LIMIT = 31;
        /// <summary>
        /// The lowest acceptable value for a month.
        /// </summary>
        private const int MONTH_LOWER_LIMIT = 1;
        /// <summary>
        /// The highest acceptable value for a month.
        /// </summary>
        private const int MONTH_UPPER_LIMIT = 12;
        /// <summary>
        /// The lowest acceptable value for a day-of-week
        /// </summary>
        private const int DOW_LOWER_LIMIT = 0;
        /// <summary>
        /// The highest acceptable value for a day-of-week.
        /// </summary>
        private const int DOW_UPPER_LIMIT = 6;

        /// <summary>
        /// Singleton member for a minute CronValue creator.
        /// </summary>
        private static CronValueCreator minuteCreator = null;
        /// <summary>
        /// Singleton member for a hour CronValue creator.
        /// </summary>
        private static CronValueCreator hourCreator = null;
        /// <summary>
        /// Singleton member for a day CronValue creator.
        /// </summary>
        private static CronValueCreator dayCreator = null;
        /// <summary>
        /// Singleton member for a month CronValue creator.
        /// </summary>
        private static CronValueCreator monthCreator = null;
        /// <summary>
        /// Singleton member for a day-of-week CronValue creator.
        /// </summary>
        private static CronValueCreator dowCreator = null;

        /// <summary>
        /// The interface definition for a cron value creator.
        /// </summary>
        public interface CronValueCreator
        {
            /// <summary>
            /// Construct a CronEffectiveValue from the specified value specification.
            /// </summary>
            /// <param name="valueSpec">
            /// The textual specification to be parsed.
            /// </param>
            /// <returns>
            /// The CronEffectiveValue that matches the value specification.
            /// </returns>
            CronEffectiveValue CreateCronValue(string valueSpec);
        }

        /// <summary>
        /// A CronValueCreator for minute values.
        /// </summary>
        private class MinuteValueCreator : CronValueCreator
        {

            #region CronValueCreator implementation
            public CronEffectiveValue CreateCronValue (string valueSpec)
            {
                return ParseCronValue(MINUTE_LOWER_LIMIT, MINUTE_UPPER_LIMIT, null, valueSpec);
            }
            #endregion
        }
        
        /// <summary>
        /// A CronValueCreator for hour values.
        /// </summary>
        private class HourValueCreator : CronValueCreator
        {

            #region CronValueCreator implementation
            public CronEffectiveValue CreateCronValue (string valueSpec)
            {
                return ParseCronValue(HOUR_LOWER_LIMIT, HOUR_UPPER_LIMIT, null, valueSpec);
            }
            #endregion
        }
        
        /// <summary>
        /// A CronValueCreator for day values.
        /// </summary>
        private class DayValueCreator : CronValueCreator
        {

            #region CronValueCreator implementation
            public CronEffectiveValue CreateCronValue (string valueSpec)
            {
                return ParseCronValue(DAY_LOWER_LIMIT, DAY_UPPER_LIMIT, null, valueSpec);
            }
            #endregion
        }
        
        /// <summary>
        /// A CronValueCreator for month values.
        /// </summary>
        private class MonthValueCreator : CronValueCreator
        {

            #region CronValueCreator implementation
            public CronEffectiveValue CreateCronValue (string valueSpec)
            {
                return ParseMonthCronValue(MONTH_LOWER_LIMIT, MONTH_UPPER_LIMIT, MONTHS, valueSpec);
            }
            #endregion
        }
        
        /// <summary>
        /// A CronValueCreator for day-of-week values.
        /// </summary>
        private class DOWValueCreator : CronValueCreator
        {

            #region CronValueCreator implementation
            public CronEffectiveValue CreateCronValue (string valueSpec)
            {
                return ParseDOWCronValue(DOW_LOWER_LIMIT, DOW_UPPER_LIMIT, DOWS, valueSpec);
            }
            #endregion
        }

        /// <summary>
        /// Constructs a new CronValueFactory.
        /// </summary>
        private CronValueFactory()
        {
        }

        /// <summary>
        /// Returns the CronValueCreator for minutes.
        /// </summary>
        /// <returns>
        /// A CronValueCreator for minutes.
        /// </returns>
        public static CronValueCreator GetMinuteCreator() {
            if (minuteCreator == null)
            {
                minuteCreator = new MinuteValueCreator();
            }

            return minuteCreator;
        }

        /// <summary>
        /// Returns the CronValueCreator for hours.
        /// </summary>
        /// <returns>
        /// A CronValueCreator for hours.
        /// </returns>
        public static CronValueCreator GetHourCreator() {
            if (hourCreator == null)
            {
                hourCreator = new HourValueCreator();
            }

            return hourCreator;
        }

        /// <summary>
        /// Returns the CronValueCreator for days.
        /// </summary>
        /// <returns>
        /// A CronValueCreator for days.
        /// </returns>
        public static CronValueCreator GetDayCreator() {
            if (dayCreator == null)
            {
                dayCreator = new DayValueCreator();
            }

            return dayCreator;
        }

        /// <summary>
        /// Returns the CronValueCreator for months.
        /// </summary>
        /// <returns>
        /// A CronValueCreator for months.
        /// </returns>
        public static CronValueCreator GetMonthCreator() {
            if (monthCreator == null)
            {
                monthCreator = new MonthValueCreator();
            }

            return monthCreator;
        }

        /// <summary>
        /// Returns the CronValueCreator for days-of-week.
        /// </summary>
        /// <returns>
        /// A CronValueCreator for days-of-week.
        /// </returns>
        public static CronValueCreator GetDOWCreator() {
            if (dowCreator == null)
            {
                dowCreator = new DOWValueCreator();
            }

            return dowCreator;
        }

        /// <summary>
        /// Find the numeric value of the specified day-of-week string.
        /// </summary>
        /// <param name="dow">
        /// The name of the day-of-week to find the numeric value for.
        /// </param>
        /// <returns>
        /// The numeric equivalent for the day-of-week. -1 if the name is not found.
        /// </returns>
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

        /// <summary>
        /// Find the numeric value of the specified month.
        /// </summary>
        /// <param name="month">
        /// The name of the month.
        /// </param>
        /// <returns>
        /// The numeric equivalent of the month. -1 if the name is not found.
        /// </returns>
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

        /// <summary>
        /// Returns the numeric equivalent of the month specified in the given expression.
        /// </summary>
        /// <param name="month">
        /// The month expression. May be a named month or numeric equivalent.
        /// </param>
        /// <returns>
        /// The numeric equivalent of the month.
        /// </returns>
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

        /// <summary>
        /// Returns the numeric equivalent of the day-of-week specified in the given expression.
        /// </summary>
        /// <param name="dow">
        /// The day-of-week expression. May be a named day-of-week or numeric equivalent.
        /// </param>
        /// <returns>
        /// The numeric equivalent of the day-of-week.
        /// </returns>
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

        /// <summary>
        /// Parses the cron value into a CronEffective value.
        /// </summary>
        /// <param name="lowerLimit">
        /// The lower acceptable limit for values.
        /// </param>
        /// <param name="upperLimit">
        /// The upper acceptable limit for values.
        /// </param>
        /// <param name="names">
        /// The array of names that are associated with this type. May be null.
        /// </param>
        /// <param name="valueSpec">
        /// The value specification to parse.
        /// </param>
        /// <returns>
        /// A cron effective value parsed from the value specification.
        /// </returns>
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

        /// <summary>
        /// Parses the specified day-of-week value specification into a cron effective value.
        /// </summary>
        /// <param name="lowerLimit">
        /// The lower acceptable limit for values of this type.
        /// </param>
        /// <param name="upperLimit">
        /// The upper acceptable limit for values of this type.
        /// </param>
        /// <param name="names">
        /// The array of names associated with the values of this type. May be null.
        /// </param>
        /// <param name="valueSpec">
        /// The value specification to be parsed into an effective value.
        /// </param>
        /// <returns>
        /// A cron effective value that is parsed from the value specification.
        /// </returns>
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

        /// <summary>
        /// Parses the specified month value specification into a cron effective value.
        /// </summary>
        /// <param name="lowerLimit">
        /// The lower acceptable limit for values of this type.
        /// </param>
        /// <param name="upperLimit">
        /// The upper acceptable limit for values of this type.
        /// </param>
        /// <param name="names">
        /// An array of names associated with the numeric values. May be null.
        /// </param>
        /// <param name="valueSpec">
        /// The value specification to be parsed.
        /// </param>
        /// <returns>
        /// A cron effective value that matches the parsed value specification.
        /// </returns>
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
