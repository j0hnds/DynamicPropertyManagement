
using System;
using System.Collections.Generic;
using System.Text;
using DomainCore;

namespace DAOCore
{
    
    
    public class DeleteBuilder : BuilderBase
    {

        public const string SQL_TEMPLATE = "DELETE FROM {0}\nWHERE\n  {1} = {2}";
        
        public DeleteBuilder(string tableName, Dictionary<string,string> columnMappings) :
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
