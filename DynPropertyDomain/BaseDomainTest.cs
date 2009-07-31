
using System;
using DomainCore;
using DAOCore;

namespace DynPropertyDomain
{
    
    
    public class BaseDomainTest
    {
        
        public BaseDomainTest()
        {
        }

        protected void SetUpDomainFactory()
        {
            DomainFactory.DAOAssembly = "DynPropertyDomain";
            DomainFactory.DAONamespace = "DynPropertyDomain.DAO";
            DomainFactory.DomainAssembly = "DynPropertyDomain";
            DomainFactory.DomainNamespace = "DynPropertyDomain";
        }

        protected void SetUpConnectionString()
        {
            DataSource ds = DataSource.Instance;
            ds.Host = "localhost";
            ds.DBName = "online_logging";
            ds.UserID = "siehd";
            ds.Password = "jordan123";
        }
    }
}
