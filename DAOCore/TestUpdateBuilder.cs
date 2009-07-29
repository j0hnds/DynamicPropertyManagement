
using System;
using System.Collections.Generic;
using DomainCore;
using NUnit.Framework;

namespace DAOCore
{
    
    
    [TestFixture]
    public class TestUpdateBuilder : BaseBuilderTest
    {
        private UpdateBuilder cut;
        private Domain domain;
        
        [SetUp]
        public void SetUp()
        {
            DomainFactorySetup();

            Dictionary<string,string> mappings = new Dictionary<string, string>();
            mappings["Id"] = "DOMAIN_ID";
            mappings["StringAttr"] = "STRING_COLUMN";
            mappings["LongAttr"] = "LONG_COLUMN";

            cut = new UpdateBuilder("UPDATE_TABLE", mappings);

            domain = DomainFactory.Create("UpdateDomain", true);
        }
        
        [Test]
        public void TestNoValuesSet()
        {
            try
            {
                cut.Build(domain);
                Assert.Fail("Should have thrown an exception");
            }
            catch (Exception) {}
        }

        [Test]
        public void TestAllValuesSet()
        {
            BaseAttribute.BeginPopulation();
            domain.SetValue("Id", 23);
            BaseAttribute.EndPopulation();
            domain.SetValue("StringAttr", "StringValue");
            domain.SetValue("LongAttr", 44);

            string shouldSQL = "UPDATE UPDATE_TABLE SET\n  STRING_COLUMN = 'StringValue',\n  LONG_COLUMN = 44\nWHERE\n  DOMAIN_ID = 23";
            Assert.AreEqual(shouldSQL, cut.Build(domain));
        }
        [Test]
        public void TestAllValuesSetNoPopulate()
        {
            domain.SetValue("Id", 23);
            domain.SetValue("StringAttr", "StringValue");
            domain.SetValue("LongAttr", 44);

            string shouldSQL = "UPDATE UPDATE_TABLE SET\n  STRING_COLUMN = 'StringValue',\n  LONG_COLUMN = 44\nWHERE\n  DOMAIN_ID = 23";
            Assert.AreEqual(shouldSQL, cut.Build(domain));
        }
        [Test]
        public void TestSomeValuesSet()
        {
            BaseAttribute.BeginPopulation();
            domain.SetValue("Id", 23);
            BaseAttribute.EndPopulation();
            domain.SetValue("StringAttr", "StringValue");

            string shouldSQL = "UPDATE UPDATE_TABLE SET\n  STRING_COLUMN = 'StringValue'\nWHERE\n  DOMAIN_ID = 23";
            Assert.AreEqual(shouldSQL, cut.Build(domain));
        }
    }
}
