
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
        /// The DAO to be used for the Application domain object.
        /// </summary>
        class ApplicationDAO : DAOBase
        {
            /// <summary>
            /// The mapping between attribute names and db column names.
            /// </summary>
            private static readonly Dictionary<string,string> ATTR_COL_MAPPINGS = new Dictionary<string, string>();

            /// <summary>
            /// Static constructor to initialize the column mappings.
            /// </summary>
            static ApplicationDAO()
            {
                ATTR_COL_MAPPINGS["Id"] = "DYN_APPLICATION_ID";
                ATTR_COL_MAPPINGS["Name"] = "APPLICATION_NAME";
            }

            /// <summary>
            /// Constructs a new ApplicationDAO object.
            /// </summary>
            public ApplicationDAO() : 
                base("Application", "DYN_APPLICATION", ATTR_COL_MAPPINGS)
            {
            }
            
            public override Domain GetObject (object id)
            {
                IDbCommand cmd = Connection.CreateCommand();
                cmd.CommandText = String.Format("SELECT * FROM DYN_APPLICATION WHERE DYN_APPLICATION_ID = {0}", DAOUtils.ConvertValue(id));

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
                    cmd.CommandText = "SELECT * FROM DYN_APPLICATION ORDER BY APPLICATION_NAME";
                }
                else if (argsRest.Length == 1)
                {
                    // We will assume here that we were passed the ID of
                    // a property definition and we need a list of applications
                    // that implements that property definition.
                    cmd.CommandText = String.Format(@"select
          app.DYN_APPLICATION_ID,
          app.APPLICATION_NAME,
          dass.DYN_PROPERTY_ID
        from
          DYN_ASSIGN dass,
          DYN_APPLICATION app
        where
          dass.DYN_PROPERTY_ID = {0}
          and app.DYN_APPLICATION_ID = dass.APPLICATION_ID
        order by
          app.APPLICATION_NAME", DAOUtils.ConvertValue(argsRest[0]));
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
    /// This class holds application data.
    /// </summary>
    public class Application : Domain
    {
        /// <summary>
        /// The name of the Name attribute
        /// </summary>
        private const string NAME_ATTR = "Name";
        /// <summary>
        /// The name of the Id attribute.
        /// </summary>
        private const string ID_ATTR = "Id";

        /// <summary>
        /// The list of dynamic properties that are associated with
        /// this application.
        /// </summary>
        private List<Domain> dynamicProperties;

        /// <summary>
        /// Constructs a new Application domain object.
        /// </summary>
        /// <param name="dao">
        /// The DAO object for Application.
        /// </param>
        public Application(DomainDAO dao) : 
            base(dao)
        {
            new LongAttribute(this, ID_ATTR, true).AttributeValueChanged += HandleAttributeChange;
            new StringAttribute(this, NAME_ATTR, false).AttributeValueChanged += HandleAttributeChange;
        }

        /// <summary>
        /// Event handler for the AttributeChange event on application's attribute values.
        /// </summary>
        /// <param name="name">
        /// The name of the attribute whose value changed.
        /// </param>
        /// <param name="oldValue">
        /// The previous value of the attribute.
        /// </param>
        /// <param name="newValue">
        /// The new value of the attribute.
        /// </param>
        private void HandleAttributeChange(string name, object oldValue, object newValue)
        {
            log.Debug("Clearing cached collections");
            dynamicProperties = null;
        }

        /// <value>
        /// The unique identifier of the Application.
        /// </value>
        public long Id
        {
            get { return (long) GetValue(ID_ATTR); }
            set { SetValue(ID_ATTR, value); }
        }

        /// <value>
        /// The Name of the application.
        /// </value>
        public string Name
        {
            get { return (string) GetValue(NAME_ATTR); }
            set { SetValue(NAME_ATTR, value); }
        }

        /// <value>
        /// The list of dynamic properties associated with this application.
        /// </value>
        public List<Domain> DynamicProperties
        {
            get
            {
                if (dynamicProperties == null)
                {
                    DomainDAO dao = DomainFactory.GetDAO("DynamicProperty");
                    dynamicProperties = dao.Get(Name);
                }

                return dynamicProperties;
            }
        }
    }
}
