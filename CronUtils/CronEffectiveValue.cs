
using System;

namespace CronUtils
{
    
    /// <summary>
    /// A portion of a cron specification allowing a determination of effectiveness.
    /// </summary>
    public interface CronEffectiveValue
    {
        /// <summary>
        /// Test to determine if the specified value is effective.
        /// </summary>
        /// <param name="value">
        /// The value to test for effectiveness.
        /// </param>
        /// <returns>
        /// True if the value is deemed effective.
        /// </returns>
        bool IsEffective(int value);
    }
}
