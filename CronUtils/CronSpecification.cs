
using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace CronUtils
{
    
    /// <summary>
    /// This class is the encapsulation of a complete CRON specification.
    /// </summary>
    /// <remarks>
    /// The class understands how to parse out a raw (textual) cron specification
    /// into an internal form that can quickly determine if a date/time is effective.
    /// <para>The following is a description of the raw specification extracted from
    /// Wikipedia:</para>
    /// <para>Each line of a crontab file represents a job and follows a particular format as a series of fields, 
    /// separated by spaces and/or tabs. Each field can have a single value or a series of values.</para>
    /// <para><STRONG>Operators</STRONG></para>
    /// <para>There are several ways of specifying multiple date/time values in a field:</para>
    /// <list type="bullet">
    /// <item>The comma (',') operator specifies a list of values, for example: "1,3,4,7,8" (space inside the list 
    /// must not be used)</item>
    /// <item>The dash ('-') operator specifies a range of values, for example: "1-6", which is equivalent to 
    /// "1,2,3,4,5,6"</item>
    /// <item>The asterisk ('*') operator specifies all possible values for a field. For example, an asterisk in the 
    /// hour time field would be equivalent to 'every hour' (subject to matching other specified fields).</item>
    /// </list>
    /// <para>There is also an operator which some extended versions of cron support, the slash ('/') operator (called 
    /// "step"), which can be used to skip a given number of values. For example, "*/3" in the hour time field is 
    /// equivalent to "0,3,6,9,12,15,18,21".</para>
    /// <para>So "*" specifies 'every hour' but the "*/3" means only those hours divisible by 3. The meaning of '/' 
    /// specifier, however, means "when the modulo is zero" rather than "every". For example, "*/61" in the minute will 
    /// in fact be executed hourly, not every 61 minutes.</para>
    /// <para>Example: the following will be effective at one minute past midnight each day.</para>
    /// <code>1 0 * * *</code>
    /// <para>Slash example: the following will be effective every 5 minutes.</para>
    /// <code>*/5 * * * *</code>
    /// <code>
    /// *  *  *  *  * 
    /// |  |  |  |  |
    /// |  |  |  |  .---- day of week (0 - 6) (Sunday=0 or 7)  OR sun,mon,tue,wed,thu,fri,sat 
    /// |  |  |  .------- month (1 - 12) OR jan,feb,mar,apr ... 
    /// |  |  .---------- day of month (1 - 31)
    /// |  .------------- hour (0 - 23)
    /// .---------------- minute (0 - 59)
    /// </code>
    /// <para>There are several special entries, most of which are just shortcuts, that can be used instead of 
    /// specifying the full cron entry:</para>
    /// <code>
    /// Entry       Description         Equivalent To
    /// @yearly     Run once a year     0 0 1 1 *
    /// @annually   (same as @yearly)   0 0 1 1 *
    /// @monthly    Run once a month    0 0 1 * *
    /// @weekly     Run once a week     0 0 * * 0
    /// @daily      Run once a day      0 0 * * *
    /// @midnight   (same as @daily)    0 0 * * *
    /// @hourly     Run once an hour    0 * * * *
    /// </code>
    /// <para>Each of the patterns from the first five fields may be either * (an asterisk), which matches all legal 
    /// values, or a list of elements separated by commas.</para>
    /// <para>For "day of the week" (field 5), both 0 and 7 are considered Sunday.</para>
    /// </remarks>
    public class CronSpecification
    {
        /// <summary>
        /// The default specification if none is specified.
        /// </summary>
        private const string DEFAULT_SPECIFICATION = "* * * * *";
        /// <summary>
        /// Index of the cron specification that relates to MINUTES
        /// </summary>
        private const int MINUTE = 0;
        /// <summary>
        /// Index of the cron specification that relates to HOURS
        /// </summary>
        private const int HOUR = 1;
        /// <summary>
        /// Index of the cron specification that relates to DAYS
        /// </summary>
        private const int DAY = 2;
        /// <summary>
        /// Index of the cron specification that relates to MONTHS
        /// </summary>
        private const int MONTH = 3;
        /// <summary>
        /// Index of the cron specification that relates to the DAYS OF WEEK
        /// </summary>
        private const int DOW = 4;

        /// <summary>
        /// Regular expression that matches the standard shortcuts that are available
        /// for cron specifications.
        /// </summary>
        private static readonly Regex SHORTCUT_PATTERN = new Regex("^@(yearly|annually|monthly|weekly|daily|midnight|hourly)$");

        /// <summary>
        /// A map that provides a cross-reference between the name of a shortcut pattern
        /// and the equivalent raw cron specification.
        /// </summary>
        private static readonly Hashtable SHORTCUT_MAP = new Hashtable();

        /// <summary>
        /// Static constructor to set up the short cut map.
        /// </summary>
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

        /// <summary>
        /// The raw cron specification provided in the constructor. We keep this to allow
        /// error messages to be related to the original input by the caller.
        /// </summary>
        private string rawSpecification;
        /// <summary>
        /// An array that holds the temporal components of the raw specification for ease of
        /// processing. See the INDEX constants above.
        /// </summary>
        private string[] specification;
        /// <summary>
        /// The array of fully parsed minute specifications. Each element in the ArrayList is
        /// a CronValue.
        /// </summary>
        private ArrayList minutes;
        /// <summary>
        /// The array of fully parsed hour specifications. Each element in the ArrayList is
        /// a CronValue.
        /// </summary>
        private ArrayList hours;
        /// <summary>
        /// The array of fully parsed day specifications. Each element in the ArrayList is
        /// a CronValue.
        /// </summary>
        private ArrayList days;
        /// <summary>
        /// The array of fully parsed month specifications. Each element in the ArrayList is
        /// a CronValue.
        /// </summary>
        private ArrayList months;
        /// <summary>
        /// The array of fully parsed day-of-week specifications. Each element in the ArrayList is
        /// a CronValue.
        /// </summary>
        private ArrayList daysOfWeek;

        /// <summary>
        /// Constructs a new CronSpecification using the default
        /// specification ("* * * * *").
        /// </summary>
        public CronSpecification() :
            this(DEFAULT_SPECIFICATION)
        {
        }

        /// <summary>
        /// Constructs a new CronSpecification object from a raw specification.
        /// </summary>
        /// <param name="rawSpecification">
        /// The raw specification to be parsed into a CronSpecification.
        /// </param>
        public CronSpecification(string rawSpecification)
        {
            if (rawSpecification == null || rawSpecification.Length == 0)
            {
                rawSpecification = DEFAULT_SPECIFICATION;
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

        /// <value>
        /// The list of Minute specifications.
        /// </value>
        public ArrayList Minutes
        {
            get { return minutes; }
            set { minutes = value; }
        }

        /// <value>
        /// The list of Hour specifications.
        /// </value>
        public ArrayList Hours
        {
            get { return hours; }
            set { hours = value; }
        }

        /// <value>
        /// The list of Day specifications.
        /// </value>
        public ArrayList Days
        {
            get { return days; }
            set { days = value; }
        }

        /// <value>
        /// The list of Month specifications.
        /// </value>
        public ArrayList Months
        {
            get { return months; }
            set { months = value; }
        }

        /// <value>
        /// The list of Day-of-week specifications.
        /// </value>
        public ArrayList DaysOfWeek
        {
            get { return daysOfWeek; }
            set { daysOfWeek = value; }
        }

        /// <summary>
        /// Parses a single temporal component of the raw specification into the appropriate list of CronValues.
        /// </summary>
        /// <remarks>
        /// For example, you might pass the minute portion of the specification through to be parsed.
        /// </remarks>
        /// <param name="specification">
        /// A portion of the raw specification to be parsed (e.g. the minutes portion)
        /// </param>
        /// <param name="cronValues">
        /// The list of parsed cron values.
        /// </param>
        /// <param name="cronValueCreator">
        /// An implementation of the appropriate creator for the type of cron specification being parsed.
        /// </param>
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

        /// <summary>
        /// Tests the specified DateTime to determine if it passes the compiled cron specification.
        /// </summary>
        /// <param name="dateToCheck">
        /// The datetime to check against the cron specification.
        /// </param>
        /// <returns>
        /// <c>true</c> if the DateTime is effective relative to the cron specification.
        /// </returns>
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

        /// <summary>
        /// Tests the current DateTime to determine if it passes the compiled cron specification.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the DateTime is effective relative to the cron specification.
        /// </returns>
        public bool IsDateEffective()
        {
            return IsDateEffective(new DateTime());
        }

        /// <summary>
        /// Verifies that the specified minute value is effective relative to the cron specification.
        /// </summary>
        /// <param name="minute">
        /// The minute value to test.
        /// </param>
        /// <returns>
        /// <c>true</c> if the minute value is effective.
        /// </returns>
        private bool CheckMinutes(int minute)
        {
            return CheckValues(minutes, minute);
        }

        /// <summary>
        /// Verifies that the specified hour value is effective relative to the cron specification.
        /// </summary>
        /// <param name="hour">
        /// The hour value to test.
        /// </param>
        /// <returns>
        /// <c>true</c> if the hour value is effective.
        /// </returns>
        private bool CheckHours(int hour)
        {
            return CheckValues(hours, hour);
        }

        /// <summary>
        /// Verifies that the specified day value is effective relative to the cron specification.
        /// </summary>
        /// <param name="day">
        /// The day value to test.
        /// </param>
        /// <returns>
        /// <c>true</c> if the day value is effective.
        /// </returns>
        private bool CheckDays(int day)
        {
            return CheckValues(days, day);
        }

        /// <summary>
        /// Verifies that the specified month value is effective relative to the cron specification.
        /// </summary>
        /// <param name="month">
        /// The month value to test.
        /// </param>
        /// <returns>
        /// <c>true</c> if the month value is effective.
        /// </returns>
        private bool CheckMonths(int month)
        {
            return CheckValues(months, month);
        }

        /// <summary>
        /// Verifies that the specified dow value is effective relative to the cron specification.
        /// </summary>
        /// <param name="dow">
        /// The dow value to test.
        /// </param>
        /// <returns>
        /// <c>true</c> if the dow value is effective.
        /// </returns>
        private bool CheckDaysOfWeek(int dow)
        {
            return CheckValues(daysOfWeek, dow);
        }

        /// <summary>
        /// Verify a specific value against a list of patterns (CronValue's)
        /// </summary>
        /// <param name="valuePatterns">
        /// The list of CronValues against which to check the value.
        /// </param>
        /// <param name="value">
        /// The value to check
        /// </param>
        /// <returns>
        /// <c>true</c> if the value matches at least one of the CronValues in the list.
        /// </returns>
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

        /// <summary>
        /// Formats a string based on the CronValue contents of the specified array list.
        /// </summary>
        /// <remarks>
        /// This method is used to construct the formatted textual cron specification from the compiled specification.
        /// </remarks>
        /// <param name="al">
        /// The array of CronValues.
        /// </param>
        /// <returns>
        /// A properly formatted string.
        /// </returns>
        private string FormatArray(ArrayList al)
        {
            string result = "";
            foreach (object obj in al)
            {
                if (result.Length > 0)
                {
                    result += ",";
                }
                result += obj.ToString();
            }

            return result;
        }

        public override string ToString ()
        {
            string result = FormatArray(minutes);
            result += " ";
            result += FormatArray(hours);
            result += " ";
            result += FormatArray(days);
            result += " ";
            result += FormatArray(months);
            result += " ";
            result += FormatArray(daysOfWeek);

            return result;
        }

        /// <value>
        /// The raw specification that was passed to the constructor for compilation.
        /// </value>
        public string RawSpecification
        {
            get { return this.rawSpecification; }
        }
    }
}
