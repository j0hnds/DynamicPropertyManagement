
using System;
using System.Data;
using System.Collections.Generic;
using DomainCore;
using DAOCore;

namespace DynPropertyDomain
{
    
    
    namespace DAO
    {
        /// <summary>
        /// The DAO associated with a DynamicProperty
        /// </summary>
        class DynamicPropertyDAO : DAOBase
        {
            /// <summary>
            /// The attribute name column mappings.
            /// </summary>
            private static readonly Dictionary<string,string> ATTR_COL_MAPPINGS = new Dictionary<string, string>();

            /// <summary>
            /// Static constructor to initialize the column mappings.
            /// </summary>
            static DynamicPropertyDAO()
            {
                ATTR_COL_MAPPINGS["Id"] = "DYN_ASSIGN_ID";
                ATTR_COL_MAPPINGS["ApplicationId"] = "APPLICATION_ID";
                ATTR_COL_MAPPINGS["PropertyId"] = "DYN_PROPERTY_ID";
                ATTR_COL_MAPPINGS["Qualifier"] = "QUALIFIER";
                ATTR_COL_MAPPINGS["DefaultValue"] = "DFLT_VALUE";
            }

            /// <summary>
            /// Constructs a new DynamicPropertyDAO.
            /// </summary>
            public DynamicPropertyDAO() : 
                base("DynamicProperty", "DYN_ASSIGN", ATTR_COL_MAPPINGS)
            {
            }
            
            public override Domain GetObject (object id)
            {
                IDbCommand cmd = Connection.CreateCommand();
                cmd.CommandText = String.Format("SELECT * FROM NAMED_PROPERTY_VIEW WHERE DYN_ASSIGN_ID = {0} ORDER BY CATEGORY, NAME, EFFECTIVE_MOD_DT DESC, VALUE_MOD_DT, ASSIGN_MOD_DT", DAOUtils.ConvertValue(id));

                IDataReader reader = cmd.ExecuteReader();

                List<Domain> results = ParseResultSet(reader);
                if (results.Count == 0)
                {
                    throw new Exception("Unable to find specified Dynamic property");
                }
                if (results.Count > 1)
                {
                    throw new Exception("How did we get more than one property back?");
                }
                Domain domain = results[0];

                reader.Close();
                CloseConnection();

                return domain;
            }
            
            public override List<Domain> Get (params object[] argsRest)
            {
                List<Domain> dynProps = new List<Domain>();
                
                IDbCommand cmd = Connection.CreateCommand();
                cmd.CommandText = String.Format("SELECT * FROM NAMED_PROPERTY_VIEW WHERE APPLICATION_NAME = {0} ORDER BY CATEGORY, NAME, EFFECTIVE_MOD_DT DESC, VALUE_MOD_DT, ASSIGN_MOD_DT", DAOUtils.ConvertValue(argsRest[0]));

                IDataReader reader = cmd.ExecuteReader();

                dynProps = ParseResultSet(reader);

                reader.Close();
                CloseConnection();

                return dynProps;
            }

            /// <summary>
            /// Reads the result set and constructs the chain of objects for
            /// dynamic properties: DynamicProperty, EffectiveValue, ValueCriteria.
            /// </summary>
            /// <param name="reader">
            /// A reader on the result set.
            /// </param>
            /// <returns>
            /// The list of domain objects retrieved.
            /// </returns>
            protected List<Domain> ParseResultSet(IDataReader reader)
            {
                List<Domain> domains = new List<Domain>();
                
                string currentCategory = null;
                string currentName = null;
                Domain currentDynProp = null;
                Domain currentEffective = null;

                while (reader.Read())
                {
                    long assignId = GetLong(reader, 0); // DYN_ASSIGN_ID
                    long appId = GetLong(reader, 1); // DYN_APPLICATION_ID
                    string appName = GetString(reader, 2); // APPLICATION_NAME
                    long propId = GetLong(reader, 3); // DYN_PROPERTY_ID
                    string category = GetString(reader, 4); // CATEGORY
                    string qualifier = GetString(reader, 5); // QUALIFIER
                    string name = GetString(reader, 6); // NAME
                    string dataType = GetString(reader, 7); // DESCRIPTION
                    string defaultValue = GetString(reader, 8); // DFLT_VALUE
                    /* DateTime assignModDt = */ GetDateTime(reader, 9); // ASSIGN_MOD_DT
                    long effectiveId = GetLong(reader, 10); // DYN_EFFECTIVE_ID
                    DateTime effStartDate = GetDateTime(reader, 11); // EFF_START_DT
                    DateTime effEndDate = GetDateTime(reader, 12); // EFF_END_DT
                    /* DateTime effectiveModDt = */ GetDateTime(reader, 13); // EFFECTIVE_MOD_DT
                    long valueId = GetLong(reader, 14); // DYN_VALUE_ID
                    string criteria = GetString(reader, 15); // CRITERIA
                    string propValue = GetString(reader, 16); // PROP_VALUE
                    /* DateTime valueModDt = */ GetDateTime(reader, 17); // VALUE_MOD_DT

                    if (qualifier != null && qualifier.Length > 0)
                    {
                        // Parse out the original name rather than the combined one
                        name = name.Substring(0, name.Length - qualifier.Length);
                    }

                    BaseAttribute.BeginPopulation();
                    if ((! category.Equals(currentCategory)) || 
                        (! name.Equals(currentName)))
                    {
                        // Start a new dyn property object
                        currentDynProp = DomainFactory.Create("DynamicProperty", false);
                        currentDynProp.SetValue("Id", assignId);
                        currentDynProp.SetValue("ApplicationName", appName);
                        currentDynProp.SetValue("ApplicationId", appId);
                        currentDynProp.SetValue("PropertyId", propId);
                        currentDynProp.SetValue("Category", category);
                        currentDynProp.SetValue("Qualifier", qualifier);
                        currentDynProp.SetValue("PropertyName", name);
                        currentDynProp.SetValue("DefaultValue", defaultValue);
                        currentDynProp.SetValue("PropertyType", dataType);

                        domains.Add(currentDynProp);
                        currentEffective = null;
                        currentCategory = category;
                        currentName = name;
                    }

                    if (effectiveId >= 0)
                    {
                        if (currentEffective == null || 
                            (long) currentEffective.GetValue("Id") != Convert.ToInt64(effectiveId))
                        {
                            currentEffective = DomainFactory.Create("EffectiveValue", false);
                            currentEffective.SetValue("Id", effectiveId);
                            currentEffective.SetValue("AssignId", assignId);
                            currentEffective.SetValue("EffectiveStartDate", effStartDate);
                            currentEffective.SetValue("EffectiveEndDate", effEndDate);
                            CollectionRelationship rel = currentDynProp.GetRelationship("EffectiveValues") as CollectionRelationship;
                            rel.AddObject(currentEffective);
                        }
                    }

                    if (valueId >= 0)
                    {
                        Domain val = DomainFactory.Create("ValueCriteria", false);
                        val.SetValue("Id", valueId);
                        val.SetValue("EffectiveId", effectiveId);
                        val.SetValue("Value", propValue);
                        val.SetValue("RawCriteria", criteria);
                        CollectionRelationship rel = currentEffective.GetRelationship("ValueCriteria") as CollectionRelationship;
                        rel.AddObject(val);
                    }
                    BaseAttribute.EndPopulation();
                }

                return domains;
            }
        }
    }

    /// <summary>
    /// This domain object represents a DynamicProperty.
    /// </summary>
    public class DynamicProperty : Domain
    {
        /// <summary>
        /// The name of the Id attribute
        /// </summary>
        private const string ID_ATTR = "Id";
        /// <summary>
        /// The name of the ApplicationId attribute.
        /// </summary>
        private const string APPLICATIONID_ATTR = "ApplicationId";
        /// <summary>
        /// The name of the ApplicationName attribute.
        /// </summary>
        private const string APPLICATIONNAME_ATTR = "ApplicationName";
        /// <summary>
        /// The name of the PropertyId attribute.
        /// </summary>
        private const string PROPERTYID_ATTR = "PropertyId";
        /// <summary>
        /// The name of the Category attribute.
        /// </summary>
        private const string CATEGORY_ATTR = "Category";
        /// <summary>
        /// The name of the PropertyName attribute.
        /// </summary>
        private const string PROPERTYNAME_ATTR = "PropertyName";
        /// <summary>
        /// The name of the DefaultValue attribute.
        /// </summary>
        private const string DEFAULTVALUE_ATTR = "DefaultValue";
        /// <summary>
        /// The name of the PropertyType attribute.
        /// </summary>
        private const string PROPERTYTYPE_ATTR = "PropertyType";
        /// <summary>
        /// The name of the Qualifier attribute.
        /// </summary>
        private const string QUALIFIER_ATTR = "Qualifier";
        /// <summary>
        /// The name of the EffectiveValues relationship.
        /// </summary>
        private const string EFFECTIVEVALUES_REL = "EffectiveValues";

        /// <summary>
        /// Constructs a new DynamicProperty domain object.
        /// </summary>
        /// <param name="dao">
        /// The DynamicProperty DAO object.
        /// </param>
        public DynamicProperty(DomainDAO dao) :
            base(dao)
        {
            new LongAttribute(this, ID_ATTR, true);
            new LongAttribute(this, APPLICATIONID_ATTR, false);
            new StringAttribute(this, APPLICATIONNAME_ATTR, false);
            new LongAttribute(this, PROPERTYID_ATTR, false);
            new StringAttribute(this, CATEGORY_ATTR, false);
            new StringAttribute(this, PROPERTYNAME_ATTR, false);
            new StringAttribute(this, DEFAULTVALUE_ATTR, false);
            new StringAttribute(this, PROPERTYTYPE_ATTR, false);
            new StringAttribute(this, QUALIFIER_ATTR, false);
            new CollectionRelationship(this, EFFECTIVEVALUES_REL, "EffectiveValue", "AssignId");
        }

        /// <value>
        /// The unique identifier of the DynamicProperty.
        /// </value>
        public long Id
        {
            get { return (long) GetValue(ID_ATTR); }
            set { SetValue(ID_ATTR, value); }
        }

        /// <value>
        /// The Application Id of the DynamicProperty.
        /// </value>
        public long ApplicationId
        {
            get { return (long) GetValue(APPLICATIONID_ATTR); }
            set { SetValue(APPLICATIONID_ATTR, value); }
        }

        /// <value>
        /// The Application Name of the DynamicProperty.
        /// </value>
        public string ApplicationName
        {
            get { return (string) GetValue(APPLICATIONNAME_ATTR); }
            set { SetValue(APPLICATIONNAME_ATTR, value); }
        }

        /// <value>
        /// The Property Id of the DynamicProperty.
        /// </value>
        public long PropertyId
        {
            get { return (long) GetValue(PROPERTYID_ATTR); }
            set { SetValue(PROPERTYID_ATTR, value); }
        }

        /// <value>
        /// The Category of the DynamicProperty.
        /// </value>
        public string Category
        {
            get { return (string) GetValue(CATEGORY_ATTR); }
            set { SetValue(CATEGORY_ATTR, value); }
        }

        /// <value>
        /// The PropertyName of the DynamicProperty.
        /// </value>
        public string PropertyName
        {
            get { return (string) GetValue(PROPERTYNAME_ATTR); }
            set { SetValue(PROPERTYNAME_ATTR, value); }
        }

        /// <value>
        /// The DefaultValue of the Dynamic Property.
        /// </value>
        public string DefaultValue
        {
            get { return (string) GetValue(DEFAULTVALUE_ATTR); }
            set { SetValue(DEFAULTVALUE_ATTR, value); }
        }

        /// <value>
        /// The Property Type of the DynamicProperty.
        /// </value>
        public string PropertyType
        {
            get { return (string) GetValue(PROPERTYTYPE_ATTR); }
            set { SetValue(PROPERTYTYPE_ATTR, value); }
        }

        /// <value>
        /// The Qualifier of the DynamicProperty.
        /// </value>
        public string Qualifier
        {
            get { return (string) GetValue(QUALIFIER_ATTR); }
            set { SetValue(QUALIFIER_ATTR, value); }
        }

        /// <value>
        /// The list of effective values associated with this object.
        /// </value>
        public List<Domain> EffectiveValues
        {
            get { return GetCollection(EFFECTIVEVALUES_REL); }
        }

        /// <value>
        /// The current effective value of this object.
        /// </value>
        public object CurrentEffectiveValue
        {
            get { return GetEffectiveValue(); }
        }

        /// <summary>
        /// Returns the current effective value of this object.
        /// </summary>
        /// <returns>
        /// The effective value.
        /// </returns>
        public object GetEffectiveValue()
        {
            return GetEffectiveValue(DateTime.Now);
        }

        /// <summary>
        /// Gets the effective value of this object as of the specified
        /// date/time.
        /// </summary>
        /// <param name="dateTime">
        /// The date time to use to determine effectiveness.
        /// </param>
        /// <returns>
        /// The effective value.
        /// </returns>
        public object GetEffectiveValue(DateTime dateTime)
        {
            object defaultValue = GetValue("DefaultValue");
            object val = defaultValue;

            foreach (Domain domain in GetCollection("EffectiveValues"))
            {
                EffectiveValue ev = (EffectiveValue) domain;
                if (ev.IsEffectiveAt(dateTime))
                {
                    val = ev.GetEffectiveValue(dateTime, defaultValue);
                    break;
                }
            }

            return val;
        }

        /// <value>
        /// The qualified name of this property.
        /// </value>
        public string QualifiedName
        {
            get
            {
                string propName = (string) GetValue("PropertyName");
                string qualifier = (string) GetValue("Qualifier");

                if (qualifier != null)
                {
                    if (propName != null)
                    {
                        propName += qualifier;
                    }
                    else
                    {
                        propName = qualifier;
                    }
                }

                return propName;
            }
        }
    }
}
