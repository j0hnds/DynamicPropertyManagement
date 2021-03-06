
using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace DomainCore
{
    namespace TestDomains
    {
        public class TDomain : Domain
        {
            public TDomain(DomainDAO dao) :
                base(dao)
            {
                new LongAttribute(this, "Id", true);
                new StringAttribute(this, "StringAttr", false);
            }
        }
        public class CDomain : Domain
        {
            public CDomain(DomainDAO dao) :
                base(dao)
            {
                new LongAttribute(this, "Id", true);
                new StringAttribute(this, "StringAttr", false);
            }
        }
    }

    namespace TestDAOs
    {
        public class TDomainDAO : DomainDAO
        {
            #region DomainDAO implementation
            public string DeleteSQL(Domain obj) { return "DeleteSQL"; }
            public string InsertSQL(Domain obj) { return "InsertSQL"; }
            public string UpdateSQL(Domain obj) { return "UpdateSQL"; }
            public void Delete(Domain obj) { }
            public void Insert(Domain obj) { }
            public void Update(Domain obj) { }

            public List<Domain> Get (params object[] argsRest) { return null; }

            public Domain GetObject (object id)
            {
                return null;
            }
            #endregion

        }
    }
    
    
    [TestFixture]
    public class TestDomainFactory
    {

        [SetUp]
        public void SetUp()
        {
            DomainFactory.DomainNamespace = "DomainCore.TestDomains";
            DomainFactory.DAONamespace = "DomainCore.TestDAOs";
        }

        [TearDown]
        public void TearDown()
        {
            DomainFactory.DomainNamespace = null;
            DomainFactory.DAONamespace = null;
        }
        
        [Test]
        public void TestNewObject()
        {
            Domain d = DomainFactory.Create("TDomain");
            Assert.IsNotNull(d);
            Assert.IsTrue(d.NewObject);
        }
        [Test]
        public void TestNotNewObject()
        {
            Domain d = DomainFactory.Create("TDomain", false);
            Assert.IsNotNull(d);
            Assert.IsFalse(d.NewObject);
        }
        [Test]
        public void TestNonExistentDomain()
        {
            try
            {
                DomainFactory.Create("BDomain");
                Assert.Fail("Should have thrown an exception");
            } catch (TypeLoadException)
            {
            }
        }
        [Test]
        public void TestNonExistentDAO()
        {
            try
            {
                DomainFactory.Create("CDomain");
                Assert.Fail("Should have thrown an exception");
            } catch (TypeLoadException)
            {
            }
        }
        [Test]
        public void TestNewObjectNoNamespaceSet()
        {
            DomainFactory.DomainNamespace = null;
            
            Domain d = DomainFactory.Create("DomainCore.TestDomains.TDomain");
            Assert.IsNotNull(d);
            Assert.IsTrue(d.NewObject);
        }
    }
}
