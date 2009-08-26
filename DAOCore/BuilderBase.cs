
using System;
using System.Collections.Generic;
using DomainCore;

namespace DAOCore
{
    
    /// <summary>
    /// An abstract base class for classes that will build SQL statements.
    /// </summary>
    public abstract class BuilderBase
    {
        /// <summary>
        /// A dictionary of the domain attribute to DB columns.
        /// </summary>
        private Dictionary<string,string> columnMappings;
        /// <summary>
        /// The name of the table for the built SQL.
        /// </summary>
        private string tableName;

        /// <summary>
        /// Constructs a new BuilderBase object.
        /// </summary>
        /// <param name="tableName">
        /// The name of the table to be created.
        /// </param>
        /// <param name="columnMappings">
        /// The domain attribute to DB column mappings to use.
        /// </param>
        public BuilderBase(string tableName, 
                           Dictionary<string,string> columnMappings)
        {
            this.tableName = tableName;
            this.columnMappings = columnMappings;
        }

        /// <summary>
        /// Construct the SQL string for the specified domain.
        /// </summary>
        /// <param name="domain">
        /// Reference to the domain to create the SQL for.
        /// </param>
        /// <returns>
        /// An SQL statement appropriate for the state of the domain.
        /// </returns>
        public abstract string Build(Domain domain);

        /// <summary>
        /// Returns the data base column that is mapped to the specified domain attribute name.
        /// </summary>
        /// <param name="domainAttrName">
        /// The name of the domain attribute.
        /// </param>
        /// <returns>
        /// The data base column name.
        /// </returns>
        public string GetDBColumn(string domainAttrName)
        {
            return columnMappings[domainAttrName];
        }

        /// <value>
        /// The data base column associated with the specified attrName.
        /// </value>
        public string this[string attrName]
        {
            get { return columnMappings[attrName]; }
        }

        /// <value>
        /// The name of the data base table.
        /// </value>
        public string TableName
        {
            get { return tableName; }
        }
    }
}
