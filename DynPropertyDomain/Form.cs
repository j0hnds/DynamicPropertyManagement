
using System;
using System.Data;
using System.Collections.Generic;
using DomainCore;
using DAOCore;

namespace DynPropertyDomain
{
    
    
    namespace DAO
    {
        class FormDAO : DAOBase
        {
            private static readonly Dictionary<string,string> ATTR_COL_MAPPINGS = new Dictionary<string, string>();

            static FormDAO()
            {
                ATTR_COL_MAPPINGS["Id"] = "FORM_ID";
                ATTR_COL_MAPPINGS["Description"] = "FORM_DESC";
            }
            
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
    public class Form : Domain
    {
        private const string ID_ATTR = "Id";
        private const string DESCRIPTION_ATTR = "Description";
        
        public Form(DomainDAO dao) :
            base(dao)
        {
            new LongAttribute(this, ID_ATTR, true);
            new StringAttribute(this, DESCRIPTION_ATTR, false);
        }

        public long Id
        {
            get { return (long) GetValue(ID_ATTR); }
            set { SetValue(ID_ATTR, value); }
        }

        public string Description
        {
            get { return (string) GetValue(DESCRIPTION_ATTR); }
            set { SetValue(DESCRIPTION_ATTR, value); }
        }
    }
}
