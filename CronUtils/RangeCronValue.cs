
using System;

namespace CronUtils
{
    
    /// <summary>
    /// Provides the implementation of a range Cron Value.
    /// </summary>
    /// <description>
    /// This type of Cron Value allows the definition of a lower and upper limit 
    /// between which tested values are deemed to be effective. The inclusion test 
    /// is inclusive to the end points of the range definition.    
    /// </description>
    public class RangeCronValue : CronValueBase, CronEffectiveValue
    {
        // The lower value of the range
        private int rangeLower;
        // The upper value of the range
        private int rangeUpper;

        /// <summary>
        /// Constructs a new RangeCronValue object.
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
        /// <param name="rangeLower">
        /// The lower value of the range. Effective values must be greater or
        /// equal to this value.
        /// </param>
        /// <param name="rangeUpper">
        /// The upper value of the range. Effective values must be less than or
        /// equal to this value.
        /// </param>
        public RangeCronValue(int lowerLimit, 
                              int upperLimit, 
                              string[] names, 
                              int rangeLower, 
                              int rangeUpper) : 
            base(lowerLimit, upperLimit, names)
        {
            if (! IsValueWithinLimits(rangeLower))
            {
                throw new System.ArgumentOutOfRangeException("rangeLower", rangeLower, "Not a legal value");
            }
            if (! IsValueWithinLimits(rangeUpper))
            {
                throw new System.ArgumentOutOfRangeException("rangeUpper", rangeUpper, "Not a legal value");
            }
            
            this.rangeLower = rangeLower;
            this.rangeUpper = rangeUpper;
        }

        public int RangeLower {
            get { return this.rangeLower; }
        }

        public int RangeUpper {
            get { return this.rangeUpper; }
        }

        #region CronEffectiveValue implementation
        public bool IsEffective (int value)
        {
            // throw new System.NotImplementedException();
            return RangeLower <= value && value <= RangeUpper;
        }
        #endregion

    }
}
