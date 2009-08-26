
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
        /// The DAO for Form objects.
        /// </summary>
        class FormDAO : DAOBase
        {
            /// <summary>
            /// The attribute name column mappings.
            /// </summary>
            private static readonly Dictionary<string,string> ATTR_COL_MAPPINGS = new Dictionary<string, string>();

            /// <summary>
            /// Static constructor to initialize the column mappings.
            /// </summary>
            static FormDAO()
            {
                ATTR_COL_MAPPINGS["Id"] = "FORM_ID";
                ATTR_COL_MAPPINGS["Description"] = "FORM_DESC";
            }

            /// <summary>
            /// Constructs a new FormDAO object.
            /// </summary>
            public FormDAO() : 
                base("Form", "FORM", ATTR_COL_MAPPINGS)
            {
                PopulateId = false;
            }
            
            public override Domain GetObject (object id)
            {
                IDbCommand cmd = Connection.CreateCommand();
                cmd.CommandText = String.Format("SELECT * FROM FORM WHERE FORM_ID = {0}", DAOUtils.ConvertValue(id));

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
                cmd.CommandText = "SELECT * FROM FORM ORDER BY FORM_DESC";

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
    /// This domain object represents forms in the system.
    /// </summary>
    public class Form : Domain
    {
        /// <summary>
        /// The name of the Id attribute.
        /// </summary>
        private const string ID_ATTR = "Id";
        /// <summary>
        /// The name of the Description attribute.
        /// </summary>
        private const string DESCRIPTION_ATTR = "Description";

        /// <summary>
        /// Constructs a new Form domain object.
        /// </summary>
        /// <param name="dao">
        /// Reference to the form DAO object.
        /// </param>
        public Form(DomainDAO dao) :
            base(dao)
        {
            new LongAttribute(this, ID_ATTR, true);
            new StringAttribute(this, DESCRIPTION_ATTR, false);
        }

        /// <value>
        /// The unique identifier of the Form object.
        /// </value>
        public long Id
        {
            get { return (long) GetValue(ID_ATTR); }
            set { SetValue(ID_ATTR, value); }
        }

        /// <value>
        /// The description of the form object.
        /// </value>
        public string Description
        {
            get { return (string) GetValue(DESCRIPTION_ATTR); }
            set { SetValue(DESCRIPTION_ATTR, value); }
        }
    }
}
