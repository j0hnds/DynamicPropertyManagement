
using System;
using System.Data;
using System.Collections.Generic;
using DomainCore;
using DAOCore;

namespace DynPropertyDomain
{
    
    
    namespace DAO
    {
        class ApplicationDAO : DAOBase
        {
            private static readonly Dictionary<string,string> ATTR_COL_MAPPINGS = new Dictionary<string, string>();

            static ApplicationDAO()
            {
                ATTR_COL_MAPPINGS["Id"] = "DYN_APPLICATION_ID";
                ATTR_COL_MAPPINGS["Name"] = "APPLICATION_NAME";
            }
            
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
    
    public class Application : Domain
    {
        private const string NAME_ATTR = "Name";
        private const string ID_ATTR = "Id";
        
        public Application(DomainDAO dao) : 
            base(dao)
        {
            new LongAttribute(this, ID_ATTR, true);
            new StringAttribute(this, NAME_ATTR, false);
        }

//        public object this[string name]
//        {
//            get { return GetValue(name); }
//            set { SetValue(name, value); }
//        }
//
        public long Id
        {
            get { return (long) GetValue(ID_ATTR); }
            set { SetValue(ID_ATTR, value); }
        }

        public string Name
        {
            get { return (string) GetValue(NAME_ATTR); }
            set { SetValue(NAME_ATTR, value); }
        }
    }
}
