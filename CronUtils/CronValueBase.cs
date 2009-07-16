
using System;

namespace CronUtils
{
    
    /// <summary>
    /// Abstract base implementation of a CronValue.
    /// </summary>
    /// <description>
    /// The point of this class is to
    /// define a set of limits between which a cron value must exist. For example, if
    /// the Cron Value represents a minute, the lower limit would be 0 and the upper
    /// limit would be 59. But if the Cron Value represented an hour, the lower and
    /// upper limits would be 0 and 23 respectively.    
    /// </description>
    public abstract class CronValueBase
    {
        // The lower acceptable limit for values
        private int lowerLimit;
        // The upper acceptable limit for values
        private int upperLimit;
        // An array of names for the values. It is assumed that the names array
        // will be indexed by the raw value.
        private string[] names;

        /// <summary>
        /// Constructs a new CronValueBase object.
        /// </summary>
        /// <param name="lowerLimit">
        /// The lower acceptable limit for values handled by this class.
        /// </param>
        /// <param name="upperLimit">
        /// The upper acceptable limit for values handled by this class.
        /// </param>
        /// <param name="names">
        /// An array of names associated with the values of this class. If not
        /// null, this array must have a value entry assuming that the raw values
        /// are used as indexes into the array.
        /// </param>
        public CronValueBase(int lowerLimit, int upperLimit, string[] names)
        {
            this.lowerLimit = lowerLimit;
            this.upperLimit = upperLimit;
            this.names = names;
        }

        /// <summary>
        /// Checks to see if the specified value is valid for this class.
        /// </summary>
        /// <param name="value">
        /// The value to be verified.
        /// </param>
        /// <returns>
        /// True if the value falls within the acceptable limits.
        /// </returns>
        public bool IsValueWithinLimits(int value)
        {
            return value >= LowerLimit && value <= UpperLimit;
        }

        /// <summary>
        /// Returns the name associated with the specified value.
        /// </summary>
        /// <param name="value">
        /// The value to return the name for.
        /// </param>
        /// <returns>
        /// The name of the value. If the names property of this object is not
        /// null, the name will be obtained from the names array using the value
        /// as the index into the array. If the names property of this object is
        /// null, the name will be the string representation of the value.
        /// </returns>
        public string GetNamedValue(int value)
        {
            string namedValue = value.ToString();
            
            if (names != null)
            {
                namedValue = names[value];
            }

            return namedValue;
        }

        public int LowerLimit
        {
            get { return this.lowerLimit; }
        }

        public int UpperLimit
        {
            get { return this.upperLimit; }
        }

        public string[] Names
        {
            get { return this.names; }
        }
    }
}
