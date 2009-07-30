
using System;
using DomainCore;

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
    }
}
