
using System;
using System.Data;
using System.Collections.Generic;
using DomainCore;
using DAOCore;

namespace DynPropertyDomain
{
    
    
    namespace DAO
    {
        class DynamicPropertyDAO : DAOBase
        {
            private static readonly Dictionary<string,string> ATTR_COL_MAPPINGS = new Dictionary<string, string>();

            static DynamicPropertyDAO()
            {
                ATTR_COL_MAPPINGS["Id"] = "DYN_ASSIGN_ID";
                ATTR_COL_MAPPINGS["ApplicationId"] = "APPLICATION_ID";
                ATTR_COL_MAPPINGS["PropertyId"] = "DYN_PROPERTY_ID";
                ATTR_COL_MAPPINGS["Qualifier"] = "QUALIFIER";
                ATTR_COL_MAPPINGS["DefaultValue"] = "DFLT_VALUE";
            }
            
            public DynamicPropertyDAO() : 
                base("DynamicProperty", "DYN_ASSIGN", ATTR_COL_MAPPINGS)
            {
            }
            
            public override Domain GetObject (object id)
            {
                IDbCommand cmd = Connection.CreateCommand();
                cmd.CommandText = String.Format("SELECT * FROM NAMED_PROPERTY_VIEW WHERE DYN_ASSIGN_ID = {0} ORDER BY CATEGORY, NAME, EFFECTIVE_MOD_DT DESC, VALUE_MOD_DT, ASSIGN_MOD_DT", DAOUtils.ConvertValue(id));

                IDataReader reader = cmd.ExecuteReader();
//                if (! reader.Read())
//                {
//                    throw new Exception("Unable to find specified data type");
//                }

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

            private DateTime HandleNullDateTime(object obj)
            {
                DateTime dt = DateTime.MinValue;
                if (obj != null)
                {
                    dt = (obj is DBNull) ? DateTime.MinValue : (DateTime) obj;
                }

                return dt;
            }

            protected List<Domain> ParseResultSet(IDataReader reader)
            {
                List<Domain> domains = new List<Domain>();
                
                string currentCategory = null;
                string currentName = null;
                Domain currentDynProp = null;
                Domain currentEffective = null;

                while (reader.Read())
                {
                    long assignId = (int) reader["DYN_ASSIGN_ID"];
                    long appId = (int) reader["DYN_APPLICATION_ID"];
                    string appName = (string) reader["APPLICATION_NAME"];
                    long propId = (int) reader["DYN_PROPERTY_ID"];
                    string category = (string) reader["CATEGORY"];
                    string qualifier = (string) reader["QUALIFIER"];
                    string name = (string) reader["NAME"];
                    string dataType = (string) reader["DESCRIPTION"];
                    string defaultValue = (string) reader["DFLT_VALUE"];
                    DateTime assignModDt = HandleNullDateTime(reader["ASSIGN_MOD_DT"]);
                    object effectiveId = reader["DYN_EFFECTIVE_ID"];
                    DateTime effStartDate = HandleNullDateTime(reader["EFF_START_DT"]);
                    DateTime effEndDate = HandleNullDateTime(reader["EFF_END_DT"]);
                    DateTime effectiveModDt = HandleNullDateTime(reader["EFFECTIVE_MOD_DT"]);
                    object valueId = reader["DYN_VALUE_ID"];
                    string criteria = (string) reader["CRITERIA"];
                    string propValue = (string) reader["PROP_VALUE"];
                    DateTime valueModDt = HandleNullDateTime(reader["VALUE_MOD_DT"]);

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

                    if (effectiveId != null)
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

                    if (valueId != null)
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
    
    public class DynamicProperty : Domain
    {
        
        public DynamicProperty(DomainDAO dao) :
            base(dao)
        {
            new LongAttribute(this, "Id", true);
            new LongAttribute(this, "ApplicationId", false);
            new StringAttribute(this, "ApplicationName", false);
            new LongAttribute(this, "PropertyId", false);
            new StringAttribute(this, "Category", false);
            new StringAttribute(this, "PropertyName", false);
            new StringAttribute(this, "DefaultValue", false);
            new StringAttribute(this, "PropertyType", false);
            new StringAttribute(this, "Qualifier", false);
            new CollectionRelationship(this, "EffectiveValues", "EffectiveValue", "AssignId");
        }

        public object GetEffectiveValue()
        {
            return GetEffectiveValue(DateTime.Now);
        }
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
