
using System;
using System.Collections.Generic;
using DomainCore;
using NUnit.Framework;

namespace DynPropertyDomain
{
    
    
    [TestFixture]
    public class TestPropertyDefinitionDAO : BaseDomainTest
    {
        [SetUp]
        public void SetUp()
        {
            SetUpDomainFactory();
            SetUpConnectionString();
        }
        
        [Test]
        public void TestRoundTripPropertyDefinition()
        {
            Domain domain = DomainFactory.Create("PropertyDefinition");
            domain.SetValue("Category", "__Category");
            domain.SetValue("Name", "__Original");
            domain.SetValue("DataType", 9);
            domain.SetValue("Description", "__Description");
            Assert.IsTrue(domain.NewObject);
            Assert.IsTrue(domain.Dirty);

            // Now, save the domain
            /* string sql = */ domain.SaveSQL();
            domain.Save();

            Assert.IsTrue((long) domain.GetValue("Id") > 0);
            Assert.IsFalse(domain.Dirty);
            Assert.IsFalse(domain.NewObject);

            // Now, change the name of the data type
            domain.SetValue("Name", "__Changed");

            Assert.IsTrue(domain.Dirty);

            // Save it again
            /* sql = */ domain.SaveSQL();
            domain.Save();

            // Now retrieve the domain by it's id
            Domain retDomain = domain.DAO.GetObject(domain.GetValue("Id"));
            Assert.IsNotNull(retDomain);
            Assert.AreEqual(domain.GetValue("Id"), retDomain.GetValue("Id"));
            Assert.AreEqual(domain.GetValue("Category"), retDomain.GetValue("Category"));
            Assert.AreEqual(domain.GetValue("Name"), retDomain.GetValue("Name"));
            Assert.AreEqual(domain.GetValue("DataType"), retDomain.GetValue("DataType"));
            Assert.AreEqual(domain.GetValue("Description"), retDomain.GetValue("Description"));

            // Now get all the domains and make sure our's is in it...
            List<Domain> domains = domain.DAO.Get();
            Assert.IsNotNull(domains);
            bool foundIt = false;
            foreach (Domain dom in domains)
            {
                if (dom.GetValue("Id").Equals(domain.GetValue("Id")))
                {
                    Assert.AreEqual(domain.GetValue("Category"), dom.GetValue("Category"));
                    Assert.AreEqual(domain.GetValue("Name"), dom.GetValue("Name"));
                    Assert.AreEqual(domain.GetValue("DataType"), dom.GetValue("DataType"));
                    Assert.AreEqual(domain.GetValue("Description"), dom.GetValue("Description"));
                    foundIt = true;
                }
            }
            Assert.IsTrue(foundIt);

            // Now, just run a query with a single argument
            domains = domain.DAO.Get(1);

            // Now, delete the domain object
            domain.ForDelete = true;
            /* sql = */ domain.SaveSQL();
            domain.Save();
        }
    }
}
