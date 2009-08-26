
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
        /// <summary>
        /// The DAO object for PropertyDefinitions
        /// </summary>
        class PropertyDefinitionDAO : DAOBase
        {
            /// <summary>
            /// The attribute name column mappings.
            /// </summary>
            private static readonly Dictionary<string,string> ATTR_COL_MAPPINGS = new Dictionary<string, string>();

            /// <summary>
            /// Static constructor to initialize the column mappings.
            /// </summary>
            static PropertyDefinitionDAO()
            {
                ATTR_COL_MAPPINGS["Id"] = "DYN_PROPERTY_ID";
                ATTR_COL_MAPPINGS["Category"] = "CATEGORY";
                ATTR_COL_MAPPINGS["Name"] = "NAME";
                ATTR_COL_MAPPINGS["DataType"] = "DATA_TYPE";
                ATTR_COL_MAPPINGS["Description"] = "DESCRIPTION";
            }

            /// <summary>
            /// Constructs a new PropertyDefinitionDAO object.
            /// </summary>
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

    /// <summary>
    /// Domain object for a Property Definition.
    /// </summary>
    public class PropertyDefinition : Domain
    {
        /// <summary>
        /// The name of the Id attribute.
        /// </summary>
        private const string ID_ATTR = "Id";
        /// <summary>
        /// The name of the Category attribute.
        /// </summary>
        private const string CATEGORY_ATTR = "Category";
        /// <summary>
        /// The name of the Name attribute.
        /// </summary>
        private const string NAME_ATTR = "Name";
        /// <summary>
        /// The name of the DataType attribute.
        /// </summary>
        private const string DATATYPE_ATTR = "DataType";
        /// <summary>
        /// The name of the Description attribute.
        /// </summary>
        private const string DESCRIPTION_ATTR = "Description";

        /// <summary>
        /// The list of applications that use this property definition.
        /// </summary>
        private List<Domain> usedByApplications;
        /// <summary>
        /// The list of properties in a particular category.
        /// </summary>
        private List<Domain> propertiesInCategory;

        /// <summary>
        /// The logger for this domain object.
        /// </summary>
        private ILog log;

        /// <summary>
        /// Constructs a new PropertyDefinition domain object.
        /// </summary>
        /// <param name="dao">
        /// Reference to the DAO object for PropertyDefinitions.
        /// </param>
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

        /// <summary>
        /// Signal handler to deal with attribute value changes on this object.
        /// </summary>
        /// <param name="name">
        /// The name of the attribute that changed.
        /// </param>
        /// <param name="oldValue">
        /// The previous value of the attribute.
        /// </param>
        /// <param name="newValue">
        /// The current value of the attribute.
        /// </param>
        private void HandleAttributeChange(string name, object oldValue, object newValue)
        {
            log.Debug("Clearing cached collections");
            usedByApplications = null;
            propertiesInCategory = null;
        }

        /// <value>
        /// The unique identifier of the property definition.
        /// </value>
        public long Id
        {
            get { return (long) GetValue(ID_ATTR); }
            set { SetValue(ID_ATTR, value); }
        }

        /// <value>
        /// The category of the property definition.
        /// </value>
        public string Category
        {
            get { return (string) GetValue(CATEGORY_ATTR); }
            set { SetValue(CATEGORY_ATTR, value); }
        }

        /// <value>
        /// The name of the property definition.
        /// </value>
        public string Name
        {
            get { return (string) GetValue(NAME_ATTR); }
            set { SetValue(NAME_ATTR, value); }
        }

        /// <value>
        /// The data type of the property definition.
        /// </value>
        public long DataType
        {
            get { return (long) GetValue(DATATYPE_ATTR); }
            set { SetValue(DATATYPE_ATTR, value); }
        }

        /// <value>
        /// The description of the property definition.
        /// </value>
        public string Description
        {
            get { return (string) GetValue(DESCRIPTION_ATTR); }
            set { SetValue(DESCRIPTION_ATTR, value); }
        }

        /// <value>
        /// The list of applications that use this property definition.
        /// </value>
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

        /// <value>
        /// The list of properties in this category.
        /// </value>
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
