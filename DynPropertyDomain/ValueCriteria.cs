
using System;
using System.Data;
using System.Collections.Generic;
using DomainCore;
using CronUtils;
using DAOCore;

namespace DynPropertyDomain
{
    namespace DAO
    {
        /// <summary>
        /// The DAO for ValueCriteria objects.
        /// </summary>
        class ValueCriteriaDAO : DAOBase
        {
            /// <summary>
            /// The attribute name column mappings.
            /// </summary>
            private static readonly Dictionary<string,string> ATTR_COL_MAPPINGS = new Dictionary<string, string>();

            /// <summary>
            /// Static constructor to initialize the column mappings.
            /// </summary>
            static ValueCriteriaDAO()
            {
                ATTR_COL_MAPPINGS["Id"] = "DYN_VALUE_ID";
                ATTR_COL_MAPPINGS["EffectiveId"] = "DYN_EFFECTIVE_ID";
                ATTR_COL_MAPPINGS["RawCriteria"] = "CRITERIA";
                ATTR_COL_MAPPINGS["Value"] = "PROP_VALUE";
            }

            /// <summary>
            /// Constructs a new ValueCriteriaDAO object.
            /// </summary>
            public ValueCriteriaDAO() : 
                base("ValueCriteria", "DYN_VALUE", ATTR_COL_MAPPINGS)
            {
            }
            
            public override Domain GetObject (object id)
            {
                IDbCommand cmd = Connection.CreateCommand();
                cmd.CommandText = String.Format("SELECT * FROM DYN_VALUE WHERE DYN_VALUE_ID = {0}", DAOUtils.ConvertValue(id));

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
                cmd.CommandText = "SELECT * FROM DYN_VALUE";

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
    /// Domain object representing ValueCriteria.
    /// </summary>
    public class ValueCriteria : Domain
    {
        /// <summary>
        /// The name of the Id attribute.
        /// </summary>
        private const string ID_ATTR = "Id";
        /// <summary>
        /// The name of the EffectiveId attribute.
        /// </summary>
        private const string EFFECTIVEID_ATTR = "EffectiveId";
        /// <summary>
        /// The name of the Value attribute.
        /// </summary>
        private const string VALUE_ATTR = "Value";
        /// <summary>
        /// The name of the RawCriteria attribute.
        /// </summary>
        private const string RAWCRITERIA_ATTR = "RawCriteria";

        /// <summary>
        /// Constructs a new ValueCriteria domain object.
        /// </summary>
        /// <param name="dao">
        /// Reference to the DAO for ValueCriteria objects.
        /// </param>
        public ValueCriteria(DomainDAO dao) : 
            base(dao)
        {
            new LongAttribute(this, ID_ATTR, true);
            new LongAttribute(this, EFFECTIVEID_ATTR, false);
            new StringAttribute(this, VALUE_ATTR, false);
            new StringAttribute(this, RAWCRITERIA_ATTR, false);
        }

        /// <value>
        /// The unique identifier for value criteria.
        /// </value>
        public long Id
        {
            get { return (long) GetValue(ID_ATTR); }
            set { SetValue(ID_ATTR, value); }
        }

        /// <value>
        /// The effective Id for value criteria.
        /// </value>
        public long EffectiveId
        {
            get { return (long) GetValue(EFFECTIVEID_ATTR); }
            set { SetValue(EFFECTIVEID_ATTR, value); }
        }

        /// <value>
        /// The valeu for the value criteria.
        /// </value>
        public string Value
        {
            get { return (string) GetValue(VALUE_ATTR); }
            set { SetValue(VALUE_ATTR, value); }
        }

        /// <value>
        /// The Raw Criteria for the value criteria.
        /// </value>
        public string RawCriteria
        {
            get { return (string) GetValue(RAWCRITERIA_ATTR); }
            set { SetValue(RAWCRITERIA_ATTR, value); }
        }

        /// <value>
        /// The compiled cron specification.
        /// </value>
        public CronSpecification CronSpec
        {
            get
            {
                CronSpecification cronSpec = null;

                if (! GetAttribute("RawCriteria").Empty)
                {
                    cronSpec = new CronSpecification((string) GetValue("RawCriteria"));
                }

                return cronSpec;
            }
        }

        /// <summary>
        /// Checks to see if the specified date time is effective.
        /// </summary>
        /// <param name="dateTime">
        /// The date time to check for effectiveness.
        /// </param>
        /// <returns>
        /// <c>true</c> if the date time is effective.
        /// </returns>
        public bool IsEffectiveAt(DateTime dateTime)
        {
            bool effective = true;

            CronSpecification cronSpec = CronSpec;
            if (cronSpec != null)
            {
                effective = cronSpec.IsDateEffective(dateTime);
            }

            return effective;
        }
    }
}
