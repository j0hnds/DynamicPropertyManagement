
using System;
using System.Data;
using System.Collections.Generic;
using DomainCore;
using DAOCore;
using log4net;

namespace DynPropertyDomain
{
    
    
    namespace DAO
    {
        class PropertyDefinitionDAO : DAOBase
        {
            private static readonly Dictionary<string,string> ATTR_COL_MAPPINGS = new Dictionary<string, string>();

            static PropertyDefinitionDAO()
            {
                ATTR_COL_MAPPINGS["Id"] = "DYN_PROPERTY_ID";
                ATTR_COL_MAPPINGS["Category"] = "CATEGORY";
                ATTR_COL_MAPPINGS["Name"] = "NAME";
                ATTR_COL_MAPPINGS["DataType"] = "DATA_TYPE";
                ATTR_COL_MAPPINGS["Description"] = "DESCRIPTION";
            }
            
            public PropertyDefinitionDAO() : 
                base("PropertyDefinition", "DYN_PROPERTY", ATTR_COL_MAPPINGS)
            {
            }
            
            public override Domain GetObject (object id)
            {
                IDbCommand cmd = Connection.CreateCommand();
                cmd.CommandText = String.Format("SELECT * FROM DYN_PROPERTY WHERE DYN_PROPERTY_ID = {0}", DAOUtils.ConvertValue(id));

                IDataReader reader = cmd.ExecuteReader();
                if (! reader.Read())
                {
                    throw new Exception("Unable to find specified data type");
                }

                Domain domain = PopulateDomain(reader);

                reader.Close();
                CloseConnection();

                return domain;
            }
            
            public override List<Domain> Get (params object[] argsRest)
            {
                List<Domain> dataTypes = new List<Domain>();
                
                IDbCommand cmd = Connection.CreateCommand();
                if (argsRest.Length == 0)
                {
                    cmd.CommandText = "SELECT * FROM DYN_PROPERTY ORDER BY CATEGORY, NAME";
                }
                else
                {
                    cmd.CommandText = string.Format("SELECT * FROM DYN_PROPERTY WHERE CATEGORY = '{0}' ORDER BY NAME", argsRest[0]);
                }

                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Domain domain = PopulateDomain(reader);

                    dataTypes.Add(domain);
                }

                reader.Close();
                CloseConnection();

                return dataTypes;
            }
        }
    }
    public class PropertyDefinition : Domain
    {
        private const string ID_ATTR = "Id";
        private const string CATEGORY_ATTR = "Category";
        private const string NAME_ATTR = "Name";
        private const string DATATYPE_ATTR = "DataType";
        private const string DESCRIPTION_ATTR = "Description";

        private List<Domain> usedByApplications;
        private List<Domain> propertiesInCategory;

        private ILog log;
        
        public PropertyDefinition(DomainDAO dao) :
            base(dao)
        {
            log = LogManager.GetLogger(typeof(PropertyDefinition));
            
            new LongAttribute(this, ID_ATTR, true).AttributeValueChanged += HandleAttributeChange;
            new StringAttribute(this, CATEGORY_ATTR, false).AttributeValueChanged += HandleAttributeChange;
            new StringAttribute(this, NAME_ATTR, false).AttributeValueChanged += HandleAttributeChange;
            new LongAttribute(this, DATATYPE_ATTR, false).AttributeValueChanged += HandleAttributeChange;
            new StringAttribute(this, DESCRIPTION_ATTR, false).AttributeValueChanged += HandleAttributeChange;
        }

        private void HandleAttributeChange(string name, object oldValue, object newValue)
        {
            log.Debug("Clearing cached collections");
            usedByApplications = null;
            propertiesInCategory = null;
        }

        public long Id
        {
            get { return (long) GetValue(ID_ATTR); }
            set { SetValue(ID_ATTR, value); }
        }

        public string Category
        {
            get { return (string) GetValue(CATEGORY_ATTR); }
            set { SetValue(CATEGORY_ATTR, value); }
        }

        public string Name
        {
            get { return (string) GetValue(NAME_ATTR); }
            set { SetValue(NAME_ATTR, value); }
        }

        public long DataType
        {
            get { return (long) GetValue(DATATYPE_ATTR); }
            set { SetValue(DATATYPE_ATTR, value); }
        }

        public string Description
        {
            get { return (string) GetValue(DESCRIPTION_ATTR); }
            set { SetValue(DESCRIPTION_ATTR, value); }
        }

        public List<Domain> UsingApplications
        {
            get
            {
                if (usedByApplications == null)
                {
                    DomainDAO dao = DomainFactory.GetDAO("Application");

                    usedByApplications = dao.Get(Id);
                }

                return usedByApplications;
            }
        }

        public List<Domain> PropertiesInCategory
        {
            get
            {
                if (propertiesInCategory == null)
                {
                    propertiesInCategory = DAO.Get(Category);
                }

                return propertiesInCategory;
            }
        }
    }
}
