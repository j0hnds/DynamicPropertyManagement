
using System;
using NUnit.Framework;

namespace CronUtils
{
    
    
    [TestFixture]
    public class TestWildcardCronValue
    {
        const string STR_FORMAT = "*";

        private WildcardCronValue cut;
        
        [SetUp]
        public void SetUp()
        {
            cut = new WildcardCronValue(0, 31, null);
        }

        [TearDown]
        public void TearDown()
        {
            cut = null;
        }
        
        [Test]
        public void TestIsEffective()
        {
            for (int i=0; i<100; i++)
            {
                Assert.IsTrue(cut.IsEffective(i), "Value {0} should have been effective", i);
            }
        }

        [Test]
        public void TestStringFormat()
        {
            Assert.AreEqual(STR_FORMAT, cut.ToString());
        }
    }
}
