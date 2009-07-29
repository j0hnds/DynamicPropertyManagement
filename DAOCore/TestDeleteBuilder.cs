
using System;
using System.Collections.Generic;
using DomainCore;
using NUnit.Framework;

namespace DAOCore
{
    
    
    [TestFixture]
    public class TestDeleteBuilder : BaseBuilderTest
    {
        private DeleteBuilder cut;
        private Domain domain;
        
        [SetUp]
        public void SetUp()
        {
            DomainFactorySetup();

            Dictionary<string,string> mappings = new Dictionary<string, string>();
            mappings["Id"] = "DOMAIN_ID";
            mappings["StringAttr"] = "STRING_COLUMN";

            cut = new DeleteBuilder("DOMAIN_TABLE", mappings);

            domain = DomainFactory.Create("DeleteDomain", false);
        }
        
        [Test]
        public void TestCreateDeleteStatement()
        {
            domain.SetValue("Id", 23);
            domain.SetValue("StringAttr", "StringValue");

            string sql = cut.Build(domain);

            string shouldSQL = String.Format(DeleteBuilder.SQL_TEMPLATE, "DOMAIN_TABLE", "DOMAIN_ID", 23);

            Assert.AreEqual(shouldSQL, sql);
        }
    }
}
