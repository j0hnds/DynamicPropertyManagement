
using System;

namespace CronUtils
{
    
    /// <summary>
    /// Provides the implementation of a wildcard Cron Value. 
    /// </summary>
    /// <description>
    /// This type of Cron Value provides a scenario where all tested values are 
    /// deemed effective. This is used when the value of a particular cron 
    /// component is not material to the determination of effectiveness.
    /// </description>
    public class WildcardCronValue : CronValueBase, CronEffectiveValue
    {

        /// <summary>
        /// Constructs a new WildcardCronValue object.
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
        public WildcardCronValue(int lowerLimit, 
                                 int upperLimit, 
                                 string[] names) :
            base(lowerLimit, upperLimit, names)
        {
        }
        
        #region CronEffectiveValue implementation
        public bool IsEffective (int value)
        {
            return true;
        }
        #endregion
           
    }
}
