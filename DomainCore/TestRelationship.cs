
using System;
using NUnit.Framework;

namespace DomainCore
{
    
    namespace TestDomains
    {
        public class RelationshipDomain : Domain
        {
            public RelationshipDomain(DomainDAO dao) :
                base(dao)
            {
                new LongAttribute(this, "Id", true);
                new StringAttribute(this, "StringAttr", false);
            }
        }
    }

    namespace TestDAOs
    {
        public class RelationshipDomainDAO : DomainDAO
        {
            public string DeleteSQL(Domain obj) { return "DeleteSQL"; }
            public string InsertSQL(Domain obj) { return "InsertSQL"; }
            public string UpdateSQL(Domain obj) { return "UpdateSQL"; }
            public void Delete(Domain obj) { }
            public void Insert(Domain obj) { }
            public void Update(Domain obj) { }
        }
    }
    
    [TestFixture]
    public class TestRelationship
    {

        private CollectionRelationship cut;

        [SetUp]
        public void SetUp()
        {
            DomainFactory.DomainNamespace = "DomainCore.TestDomains";
            DomainFactory.DAONamespace = "DomainCore.TestDAOs";
            
            cut = new CollectionRelationship(null, "Mine", "RelationshipDomain", "Id");
        }
        
        [Test]
        public void TestEmptyCollection()
        {
            Assert.AreEqual(0, cut.CountObjects());
            Assert.IsFalse(cut.Dirty);
        }

        [Test]
        public void TestAddnew()
        {
            Domain domain = cut.AddNewObject();
            Assert.IsNotNull(domain);
            Assert.IsTrue(cut.Dirty);
            Assert.AreEqual(1, cut.CountObjects());
            domain.SetValue("StringAttr", "StringValue");
        }

        [Test]
        public void TestAdd()
        {
            Domain domain = DomainFactory.Create("RelationshipDomain", false);
            cut.AddObject(domain);
            Assert.IsFalse(cut.Dirty);
            Assert.AreEqual(1, cut.CountObjects());
        }

        [Test]
        public void TestRemove()
        {
            Domain newDomain = cut.AddNewObject();
            newDomain.SetValue("StringAttr", "NewStringValue");
            Domain existingDomain = DomainFactory.Create("RelationshipDomain", false);
            BaseAttribute.BeginPopulation();
            existingDomain.SetValue("StringAttr", "ExistingStringValue");
            BaseAttribute.EndPopulation();
            cut.AddObject(existingDomain);

            Assert.AreEqual(2, cut.CountObjects());
            Assert.IsTrue(cut.Dirty);

            // Delete the new domain object
            cut.RemoveObject(newDomain);
            Assert.AreEqual(1, cut.CountObjects());
            Assert.AreEqual(1, cut.CollectedObjects.Count);
            Assert.IsFalse(cut.Dirty);

            // Delete the existing domain object
            cut.RemoveObject(existingDomain);
            Assert.AreEqual(0, cut.CountObjects());
            Assert.AreEqual(0, cut.CollectedObjects.Count);
            Assert.IsTrue(cut.Dirty);
        }
    }
}
