
using System;
using System.Collections.Generic;
using DomainCore;

namespace DAOCore
{
    
    
    public abstract class BuilderBase
    {
        private Dictionary<string,string> columnMappings;
        private string tableName;
        
        public BuilderBase(string tableName, Dictionary<string,string> columnMappings)
        {
            this.tableName = tableName;
            this.columnMappings = columnMappings;
        }

        public abstract string Build(Domain domain);

        public string GetDBColumn(string domainAttrName)
        {
            return columnMappings[domainAttrName];
        }

        public string this[string attrName]
        {
            get { return columnMappings[attrName]; }
        }

        public string TableName
        {
            get { return tableName; }
        }
    }
}
