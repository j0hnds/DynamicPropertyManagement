
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
        /// The DAO for EffectiveValue objects.
        /// </summary>
        class EffectiveValueDAO : DAOBase
        {
            /// <summary>
            /// The attribute name column name mapping.
            /// </summary>
            private static readonly Dictionary<string,string> ATTR_COL_MAPPINGS = new Dictionary<string, string>();

            /// <summary>
            /// Static constructor to initialize the column mappings.
            /// </summary>
            static EffectiveValueDAO()
            {
                ATTR_COL_MAPPINGS["Id"] = "DYN_EFFECTIVE_ID";
                ATTR_COL_MAPPINGS["AssignId"] = "DYN_ASSIGN_ID";
                ATTR_COL_MAPPINGS["EffectiveStartDate"] = "EFF_START_DT";
                ATTR_COL_MAPPINGS["EffectiveEndDate"] = "EFF_END_DT";
            }

            /// <summary>
            /// Constructs a new EffectiveValueDAO object.
            /// </summary>
            public EffectiveValueDAO() : 
                base("EffectiveValue", "DYN_EFFECTIVE", ATTR_COL_MAPPINGS)
            {
            }
            public override Domain GetObject (object id)
            {
                IDbCommand cmd = Connection.CreateCommand();
                cmd.CommandText = String.Format("SELECT * FROM DYN_EFFECTIVE WHERE DYN_EFFECTIVE_ID = {0}", DAOUtils.ConvertValue(id));

                IDataReader reader = cmd.ExecuteReader();
                if (! reader.Read())
                {
                    throw new Exception("Unable to find specified effective value");
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
                cmd.CommandText = "SELECT * FROM DYN_EFFECTIVE";

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
    /// This domain object represents an effective value associated with a
    /// DynamicProperty.
    /// </summary>
    public class EffectiveValue : Domain
    {
        /// <summary>
        /// The name of the Id attribute.
        /// </summary>
        private const string ID_ATTR = "Id";
        /// <summary>
        /// The name of the Assign Id attribute
        /// </summary>
        private const string ASSIGNID_ATTR = "AssignId";
        /// <summary>
        /// The name of the Effective Start Date attribute.
        /// </summary>
        private const string EFFECTIVESTARTDATE_ATTR = "EffectiveStartDate";
        /// <summary>
        /// The name of the Effective End Date attribute.
        /// </summary>
        private const string EFFECTIVEENDDATE_ATTR = "EffectiveEndDate";
        /// <summary>
        /// The name of the ValueCriteria relationship.
        /// </summary>
        private const string VALUECRITERIA_REL = "ValueCriteria";

        /// <summary>
        /// Constructs a new EffectiveValue domain object.
        /// </summary>
        /// <param name="dao">
        /// Reference to the DAO object for this domain.
        /// </param>
        public EffectiveValue(DomainDAO dao) :
            base(dao)
        {
            new LongAttribute(this, ID_ATTR, true);
            new LongAttribute(this, ASSIGNID_ATTR, false);
            new DateTimeAttribute(this, EFFECTIVESTARTDATE_ATTR);
            new DateTimeAttribute(this, EFFECTIVEENDDATE_ATTR);
            new CollectionRelationship(this, VALUECRITERIA_REL, "ValueCriteria", "EffectiveId");
        }

        /// <value>
        /// The unique identifier of the effective value.
        /// </value>
        public long Id
        {
            get { return (long) GetValue(ID_ATTR); }
            set { SetValue(ID_ATTR, value); }
        }

        /// <value>
        /// The ID of the assignment
        /// </value>
        public long AssignId
        {
            get { return (long) GetValue(ASSIGNID_ATTR); }
            set { SetValue(ASSIGNID_ATTR, value); }
        }

        /// <value>
        /// The effective start date.
        /// </value>
        public DateTime EffectiveStartDate
        {
            get { return (DateTime) GetValue(EFFECTIVESTARTDATE_ATTR); }
            set { SetValue(EFFECTIVESTARTDATE_ATTR, value); }
        }

        /// <value>
        /// The effective end date.
        /// </value>
        public DateTime EffectiveEndDate
        {
            get { return (DateTime) GetValue(EFFECTIVEENDDATE_ATTR); }
            set { SetValue(EFFECTIVEENDDATE_ATTR, value); }
        }

        /// <value>
        /// The list of ValueCriteria associated with this effective value.
        /// </value>
        public List<Domain> ValueCriteria
        {
            get { return GetCollection(VALUECRITERIA_REL); }
        }

        /// <summary>
        /// Checks to see if this effective value is effective at the specified
        /// date time.
        /// </summary>
        /// <param naRetume="dateTime">
        /// The date time to check.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified date time falls within the effective start/end dates.
        /// </returns>
        public bool IsEffectiveAt(DateTime dateTime)
        {
            DateTime startDate = (DateTime) GetValue("EffectiveStartDate");
            DateTime endDate = (DateTime) GetValue("EffectiveEndDate");

            bool lowEndValid = startDate <= dateTime;

            bool highEndValid = false;
            if (endDate == DateTime.MinValue)
            {
                highEndValid = true;
            }
            else
            {
                highEndValid = endDate >= dateTime;
            }

            return lowEndValid && highEndValid;
        }

        /// <summary>
        /// Returns the effective value of this object.
        /// </summary>
        /// <param name="dateTime">
        /// The date time 
        /// </param>
        /// <returns>
        /// The effective value. <c>null</c> if no match found.
        /// </returns>
        public object GetEffectiveValue(DateTime dateTime)
        {
            return GetEffectiveValue(dateTime, null);
        }

        /// <summary>
        /// Returns the effective value of this object.
        /// </summary>
        /// <param name="dateTime">
        /// The date time
        /// </param>
        /// <param name="defaultValue">
        /// The default value to use.
        /// </param>
        /// <returns>
        /// The effective value.
        /// </returns>
        public object GetEffectiveValue(DateTime dateTime, object defaultValue)
        {
            object effectiveValue = defaultValue;

            foreach (Domain domain in GetCollection("ValueCriteria"))
            {
                if (((ValueCriteria) domain).IsEffectiveAt(dateTime))
                {
                    effectiveValue = domain.GetValue("Value");
                    break;
                }
            }
            
            return effectiveValue;
        }
    }
}
