
using System;
using System.Collections.Generic;
using DomainCore;
using NUnit.Framework;

namespace DAOCore
{
    
    
    [TestFixture]
    public class TestInsertBuilder : BaseBuilderTest
    {
        private InsertBuilder cut;
        private Domain domain;

        [SetUp]
        public void SetUp()
        {
            DomainFactorySetup();

            Dictionary<string,string> mappings = new Dictionary<string, string>();
            mappings["Id"] = "DOMAIN_ID";
            mappings["StringAttr"] = "STRING_COLUMN";
            mappings["LongAttr"] = "LONG_COLUMN";

            cut = new InsertBuilder("INSERT_TABLE", mappings);

            domain = DomainFactory.Create("InsertDomain", true);
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
            domain.SetValue("Id", 23);
            domain.SetValue("StringAttr", "StringValue");
            domain.SetValue("LongAttr", 44);

            string shouldSQL = "INSERT INTO INSERT_TABLE (\n  STRING_COLUMN,\n  LONG_COLUMN,\n  DOMAIN_ID\n) VALUES (\n  'StringValue',\n  44,\n  23\n)";
            Assert.AreEqual(shouldSQL, cut.Build(domain));
        }
        [Test]
        public void TestSomeValuesSet()
        {
            domain.SetValue("StringAttr", "StringValue");
            domain.SetValue("LongAttr", 44);

            string shouldSQL = "INSERT INTO INSERT_TABLE (\n  STRING_COLUMN,\n  LONG_COLUMN\n) VALUES (\n  'StringValue',\n  44\n)";
            Assert.AreEqual(shouldSQL, cut.Build(domain));
        }
    }
}
