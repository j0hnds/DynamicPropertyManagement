
using System;

namespace CronUtils
{
    
    /// <summary>
    /// Provides the implementation of a step Cron Value.
    /// </summary>
    /// <remarks>
    /// This type of Cron Value allows the definition of a step value against 
    /// which other values are tested for effectiveness. For example, if the 
    /// step value specified was 3, then any value that is evenly divisible 
    /// by 3 is deemed effective.
    /// </remarks>
    public class StepCronValue : CronValueBase, CronEffectiveValue
    {
        // The step value used to determine effectiveness.
        private int stepValue;

        /// <summary>
        /// Constructs a new StepCronValue object.
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
        /// <param name="stepValue">
        /// The step value.
        /// </param>
        public StepCronValue(int lowerLimit, 
                             int upperLimit, 
                             string[] names, 
                             int stepValue) : 
            base(lowerLimit, upperLimit, names)
        {
            this.stepValue = stepValue;
        }

        /// <value>
        /// The step value.
        /// </value>
        public int StepValue
        {
            get { return this.stepValue; }
        }

        #region CronEffectiveValue implementation
        public bool IsEffective (int value)
        {
            return value % StepValue == 0;
        }

        #endregion

        public override string ToString ()
        {
            return String.Format("*/{0}", StepValue);
        }
    }
}
