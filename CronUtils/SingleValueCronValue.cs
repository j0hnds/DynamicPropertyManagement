
using System;

namespace CronUtils
{
    
    /// <summary>
    /// Provides the implementation of a single Cron Value.
    /// </summary>
    /// <description>
    /// This type of Cron Value allows the definition of a single value against 
    /// which tested values are deemed to be effective.
    /// </description>
    public class SingleValueCronValue : CronValueBase, CronEffectiveValue
    {
        // The single value that represents an effective value
        private int singleValue;

        /// <summary>
        /// Constructs a new SingleValueCronValue object with the limits and value.
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
        /// <param name="singleValue">
        /// The single value defining the only value that is deemed to be
        /// effective.
        /// </param>
        public SingleValueCronValue(int lowerLimit, 
                                    int upperLimit, 
                                    string[] names, 
                                    int singleValue) :
            base(lowerLimit, upperLimit, names)
        {
            if (IsValueWithinLimits(singleValue))
            {
                throw new System.ArgumentOutOfRangeException("singleValue", singleValue, "Not a legal value");
            }
            
            this.singleValue = singleValue;
        }

        public int SingleValue
        {
            get { return this.singleValue; }
        }

        #region CronEffectiveValue implementation
        public bool IsEffective (int value)
        {
            return SingleValue == value;
        }
        #endregion

    }
}
