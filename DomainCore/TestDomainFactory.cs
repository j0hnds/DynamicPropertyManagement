
using System;
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
            public string DeleteSQL(object obj) { return "DeleteSQL"; }
            public string InsertSQL(object obj) { return "InsertSQL"; }
            public string UpdateSQL(object obj) { return "UpdateSQL"; }
            public void Delete(object obj) { }
            public void Insert(object obj) { }
            public void Update(object obj) { }
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
