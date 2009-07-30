
using System;
using DomainCore;

namespace DAOCore
{
    
    namespace TestDomains
    {
        public class DeleteDomain : Domain
        {
            public DeleteDomain(DomainDAO dao) : 
                base(dao)
            {
                new LongAttribute(this, "Id", true);
                new StringAttribute(this, "StringAttr", false);
            }
        }

        public class UpdateDomain : Domain
        {
            public UpdateDomain(DomainDAO dao) : 
                base(dao)
            {
                new LongAttribute(this, "Id", true);
                new StringAttribute(this, "StringAttr", false);
                new LongAttribute(this, "LongAttr", false);
            }
        }
        
        public class InsertDomain : Domain
        {
            public InsertDomain(DomainDAO dao) : 
                base(dao)
            {
                new LongAttribute(this, "Id", true);
                new StringAttribute(this, "StringAttr", false);
                new LongAttribute(this, "LongAttr", false);
            }
        }
    }
    
    namespace TestDAOs
    {
        public class BaseDomainDAO : DomainDAO
        {
            public string DeleteSQL(Domain obj) { return "DeleteSQL"; }
            public string InsertSQL(Domain obj) { return "InsertSQL"; }
            public string UpdateSQL(Domain obj) { return "UpdateSQL"; }
            public void Delete(Domain obj) { }
            public void Insert(Domain obj) { }
            public void Update(Domain obj) { }
        }
        
        public class DeleteDomainDAO : BaseDomainDAO {}
        public class InsertDomainDAO : BaseDomainDAO {}
        public class UpdateDomainDAO : BaseDomainDAO {}
    }
    
    public class BaseBuilderTest
    {
        
        public BaseBuilderTest()
        {
        }

        protected void DomainFactorySetup()
        {
            DomainFactory.DomainNamespace = "DAOCore.TestDomains";
            DomainFactory.DomainAssembly = "DAOCore";
            DomainFactory.DAONamespace = "DAOCore.TestDAOs";
            DomainFactory.DAOAssembly = "DAOCore";
        }
    }
}
