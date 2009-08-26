
using System;

namespace DAOCore
{
    
    
    /// <summary>
    /// Utility class providing methods to support SQL generation.
    /// </summary>
    public class DAOUtils
    {
        /// <summary>
        /// Constant for SQL NULL value.
        /// </summary>
        private const string NULL_VALUE = "NULL";
        /// <summary>
        /// Constant for a SQL single-quote value.
        /// </summary>
        private const string SINGLE_QUOTE = "'";
        /// <summary>
        /// Constant for a two SQL single-quotes.
        /// </summary>
        private const string DOUBLE_QUOTE = "''";
        
        /// <summary>
        /// Given a string, this method handles quoting it. This means that
        /// the resulting string will be surrounded by "'" marks and any internal
        /// quotes in the string will be escaped with "''".
        /// </summary>
        /// <param name="string_to_quote">
        /// The string to quote.
        /// </param>
        /// <returns>
        /// The string wrapped in quotes and internal quotes escaped. If the incoming
        /// string is null, then the string "NULL" is returned.
        /// </returns>
        public static string QuoteString(string string_to_quote)
        {
            string value = NULL_VALUE;

            if (string_to_quote != null)
            {
                value = SINGLE_QUOTE + string_to_quote.Replace(SINGLE_QUOTE, DOUBLE_QUOTE) + SINGLE_QUOTE;
            }

            return value;
        }

        /// <summary>
        /// Converts a value as appropriate for use in a SQL string.
        /// </summary>
        /// <param name="value_to_convert">
        /// The value to convert.
        /// </param>
        /// <returns>
        /// The string value of the value.
        /// </returns>
        public static string ConvertValue(object value_to_convert)
        {
            string value = NULL_VALUE;

            if (value_to_convert != null)
            {
                if (value_to_convert is string)
                {
                    value = QuoteString((string) value_to_convert);
                }
                else if (value_to_convert is DateTime)
                {
                    DateTime dt = (DateTime) value_to_convert;
                    if (dt == DateTime.MinValue || dt == DateTime.MaxValue)
                    {
                        value = NULL_VALUE;
                    }
                    else
                    {
                        value = QuoteString(dt.ToString("yyyy/MM/dd HH:mm:ss"));
                    }
                }
                else
                {
                    value = value_to_convert.ToString();
                }    
            }

            return value;
        }
    }
}
