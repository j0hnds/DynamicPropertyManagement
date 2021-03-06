
using System;
using System.Collections.Generic;
using NUnit.Framework;
using DomainCore;

namespace DynPropertyDomain
{
    
    
    [TestFixture]
    public class TestFormDAO : BaseDomainTest
    {
        [SetUp]
        public void SetUp()
        {
            SetUpDomainFactory();
            SetUpConnectionString();
        }
        
        [Test]
        public void TestRoundTripForm()
        {
            Domain domain = DomainFactory.Create("Form");
            domain.SetValue("Id", 1001);
            domain.SetValue("Description", "__Original");
            Assert.IsTrue(domain.NewObject);
            Assert.IsTrue(domain.Dirty);

            // Now, save the domain
            /* string sql = */ domain.SaveSQL();
            domain.Save();

            Assert.IsTrue((long) domain.GetValue("Id") > 0);
            Assert.IsFalse(domain.Dirty);
            Assert.IsFalse(domain.NewObject);

            // Now, change the name of the data type
            domain.SetValue("Description", "__Changed");

            Assert.IsTrue(domain.Dirty);

            // Save it again
            /* sql = */ domain.SaveSQL();
            domain.Save();

            // Now retrieve the domain by it's id
            Domain retDomain = domain.DAO.GetObject(domain.GetValue("Id"));
            Assert.IsNotNull(retDomain);
            Assert.AreEqual(domain.GetValue("Id"), retDomain.GetValue("Id"));
            Assert.AreEqual(domain.GetValue("Description"), retDomain.GetValue("Description"));

            // Now get all the domains and make sure our's is in it...
            List<Domain> domains = domain.DAO.Get();
            Assert.IsNotNull(domains);
            bool foundIt = false;
            foreach (Domain dom in domains)
            {
                if (dom.GetValue("Id").Equals(domain.GetValue("Id")))
                {
                    Assert.AreEqual(domain.GetValue("Description"), dom.GetValue("Description"));
                    foundIt = true;
                }
            }
            Assert.IsTrue(foundIt);

            // Now, delete the domain object
            domain.ForDelete = true;
            /* sql = */ domain.SaveSQL();
            domain.Save();
        }
    }
}
