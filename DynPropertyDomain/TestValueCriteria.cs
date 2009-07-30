
using System;
using DomainCore;
using NUnit.Framework;

namespace DynPropertyDomain
{
    
    
    [TestFixture]
    public class TestValueCriteria : BaseDomainTest
    {
        private ValueCriteria cut;

        [SetUp]
        public void SetUp()
        {
            SetUpDomainFactory();
            
            cut = (ValueCriteria) DomainFactory.Create("ValueCriteria", false);
            cut.SetValue("Value", "TRUE");
        }

        [TearDown]
        public void TearDown()
        {
            cut = null;
        }
        
        [Test]
        public void TestNullCriteria()
        {
            Assert.IsTrue(cut.IsEffectiveAt(DateTime.Now));
        }

        [Test]
        public void TestWildcardCriteria()
        {
            cut.SetValue("RawCriteria", "* * * * *");
            Assert.IsTrue(cut.IsEffectiveAt(DateTime.Now));
        }

        [Test]
        public void TestJuneCriteria()
        {
            cut.SetValue("RawCriteria", "* * * jun *");
            
            DateTime julyDate = new DateTime(2009, 7, 1, 0, 0, 0);
            DateTime juneDate = new DateTime(2009, 6, 1, 0, 0, 0);

            Assert.IsFalse(cut.IsEffectiveAt(julyDate));
            Assert.IsTrue(cut.IsEffectiveAt(juneDate));
        }
    }
}
