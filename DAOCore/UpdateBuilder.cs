
using System;
using System.Collections.Generic;
using System.Text;
using DomainCore;

namespace DAOCore
{
    
    
    public class UpdateBuilder : BuilderBase
    {
        
        public UpdateBuilder(string tableName, Dictionary<string,string> mappings) :
            base(tableName, mappings)
        {
        }
        
        public override string Build(Domain domain)
        {
            StringBuilder sb = new StringBuilder("UPDATE ");
            sb.Append(TableName);
            sb.Append(" SET");
            int fieldCount = 0;

            // Loop thru all the attributes on the domain for the names
            // of the fields to insert.
            foreach (DomainCore.Attribute attr in domain.Attributes.Values)
            {
                if (! attr.Id && attr.Dirty)
                {
                    if (fieldCount > 0)
                    {
                        sb.Append(",\n  ");
                    }
                    else
                    {
                        sb.Append("\n  ");
                    }
                    sb.Append(String.Format("{0} = {1}", 
                                            this[attr.Name], 
                                            DAOUtils.ConvertValue(attr.Value)));
                    fieldCount++;
                }
            }

            if (fieldCount == 0)
            {
                throw new Exception("Nothing to update on the domain! Really?");
            }

            sb.Append("\nWHERE\n");

            DomainCore.Attribute idAttribute = domain.IdAttribute;
            sb.Append(String.Format("  {0} = {1}", 
                                    this[idAttribute.Name], 
                                    DAOUtils.ConvertValue(idAttribute.Value)));

            return sb.ToString();
        }
    }
}
