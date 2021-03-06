
using System;
using System.Text;
using System.Collections.Generic;
using DomainCore;

namespace DAOCore
{
    
    /// <summary>
    /// This class is responsible for building SQL INSERT statements for a domain
    /// object.
    /// </summary>
    public class InsertBuilder : BuilderBase
    {

        /// <summary>
        /// Constructs a new InsertBuild object.
        /// </summary>
        /// <param name="tableName">
        /// The name of the data base table.
        /// </param>
        /// <param name="mappings">
        /// The domain attribute name DB column mappings.
        /// </param>
        public InsertBuilder(string tableName, 
                             Dictionary<string,string> mappings) :
            base(tableName, mappings)
        {
        }

        public override string Build(Domain domain)
        {
            StringBuilder sb = new StringBuilder("INSERT INTO ");
            sb.Append(TableName);
            sb.Append(" (");
            int fieldCount = 0;

            // Loop thru all the attributes on the domain for the names
            // of the fields to insert.
            foreach (DomainCore.Attribute attr in domain.Attributes.Values)
            {
                if (attr.Dirty)
                {
                    if (fieldCount > 0)
                    {
                        sb.Append(",\n  ");
                    }
                    else
                    {
                        sb.Append("\n  ");
                    }
                    sb.Append(this[attr.Name]);
                    fieldCount++;
                }
            }

            if (fieldCount == 0)
            {
                throw new Exception("Nothing to insert on the domain! Really?");
            }

            sb.Append("\n) VALUES (");

            fieldCount = 0;
            // Loop thru all the attributes on the domain for the values
            // of the fields to insert.
            foreach (DomainCore.Attribute attr in domain.Attributes.Values)
            {
                if (attr.Dirty)
                {
                    if (fieldCount > 0)
                    {
                        sb.Append(",\n  ");
                    }
                    else
                    {
                        sb.Append("\n  ");
                    }
                    sb.Append(DAOUtils.ConvertValue(attr.Value));
                    fieldCount++;
                }
            }

            sb.Append("\n)");
            
            return sb.ToString();
        }
    }
}
