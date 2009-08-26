
using System;
using System.Collections.Generic;
using System.Text;
using DomainCore;

namespace DAOCore
{
    
    /// <summary>
    /// This class is responsible for constructing SQL delete statements for
    /// domain objects.
    /// </summary>
    public class DeleteBuilder : BuilderBase
    {

        /// <summary>
        /// Constant string template for an SQL delete statement.
        /// </summary>
        public const string SQL_TEMPLATE = "DELETE FROM {0}\nWHERE\n  {1} = {2}";

        /// <summary>
        /// Constructs a new DeleteBuilder object.
        /// </summary>
        /// <param name="tableName">
        /// The name of the data base table.
        /// </param>
        /// <param name="columnMappings">
        /// The domain attribute name DB column mappings.
        /// </param>
        public DeleteBuilder(string tableName, 
                             Dictionary<string,string> columnMappings) :
            base(tableName, columnMappings)
        {
        }

        public override string Build(Domain domain)
        {
            DomainCore.Attribute idAttr = domain.IdAttribute;
            return String.Format(SQL_TEMPLATE, TableName, this[idAttr.Name], DAOUtils.ConvertValue(idAttr.Value));
        }
    }
}
