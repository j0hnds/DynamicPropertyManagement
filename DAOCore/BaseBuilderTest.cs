
using System;
using DomainCore;

namespace DAOCore
{
    
    namespace TestDomains
    {
        /// <summary>
        /// A domain for testing Deletion.
        /// </summary>
        public class DeleteDomain : Domain
        {
            public DeleteDomain(DomainDAO dao) : 
                base(dao)
            {
                new LongAttribute(this, "Id", true);
                new StringAttribute(this, "StringAttr", false);
            }
        }

        /// <summary>
        /// A domain for testing Updates.
        /// </summary>
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

        /// <summary>
        /// A domain for testing Inserts.
        /// </summary>
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
        /// <summary>
        /// A DAO used for testing has primarily stubs for implementation.
        /// </summary>
        /// <remarks>
        /// Provides a base implementation to be used by the test domains.
        /// </remarks>
        public class BaseDomainDAO : DomainDAO
        {
            #region DomainDAO implementation
            public string DeleteSQL(Domain obj) { return "DeleteSQL"; }
            public string InsertSQL(Domain obj) { return "InsertSQL"; }
            public string UpdateSQL(Domain obj) { return "UpdateSQL"; }
            public void Delete(Domain obj) { }
            public void Insert(Domain obj) { }
            public void Update(Domain obj) { }

            public System.Collections.Generic.List<Domain> Get (params object[] argsRest)
            {
                return null;
            }

            public Domain GetObject (object id)
            {
                return null;
            }
            #endregion

        }

        /// <summary>
        /// DAO for the DeleteDomain.
        /// </summary>
        public class DeleteDomainDAO : BaseDomainDAO {}
        /// <summary>
        /// DAO for the InsertDomain.
        /// </summary>
        public class InsertDomainDAO : BaseDomainDAO {}
        /// <summary>
        /// DAO for the UpdateDomain.
        /// </summary>
        public class UpdateDomainDAO : BaseDomainDAO {}
    }

    /// <summary>
    /// The base class for test cases to work with the domains and
    /// DAOs defined above.
    /// </summary>
    public class BaseBuilderTest
    {

        /// <summary>
        /// Constructs a new BaseBuilderTest.
        /// </summary>
        public BaseBuilderTest()
        {
        }

        /// <summary>
        /// Set up method for the domain factory.
        /// </summary>
        protected void DomainFactorySetup()
        {
            DomainFactory.DomainNamespace = "DAOCore.TestDomains";
            DomainFactory.DomainAssembly = "DAOCore";
            DomainFactory.DAONamespace = "DAOCore.TestDAOs";
            DomainFactory.DAOAssembly = "DAOCore";
        }
    }
}
