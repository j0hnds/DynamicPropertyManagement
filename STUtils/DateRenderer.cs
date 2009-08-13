
using System;
using Antlr.StringTemplate;

namespace STUtils
{
    
    
    public class DateRenderer : IAttributeRenderer
    {

        #region IAttributeRenderer implementation
        public string ToString (object o, string formatName)
        {
            throw new System.NotImplementedException();
        }
        
        public string ToString (object o)
        {
            string format = "*";
            DateTime dt = (DateTime) o;

            if (dt != DateTime.MinValue)
            {
                format = dt.ToString("yyyy/MM/dd HH:mm");
            }

            return format;
        }
        #endregion
           
    }
}
