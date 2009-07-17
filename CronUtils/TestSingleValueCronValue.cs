
using System;
using NUnit.Framework;

namespace CronUtils
{
    
    
    [TestFixture]
    public class TestSingleValueCronValue
    {
        // Test constants
        const int GOOD_TEST_VALUE = 11;
        const int BAD_TEST_VALUE = 8;
        const string STR_FORMAT = "11";

        // The class under test
        private SingleValueCronValue cut;

        [SetUp]
        public void SetUp()
        {
            cut = new SingleValueCronValue(0, 31, null, GOOD_TEST_VALUE);
        }

        [TearDown]
        public void TearDown()
        {
            cut = null;
        }
        
        [Test]
        public void TestIsEffective()
        {
            Assert.IsTrue(cut.IsEffective(GOOD_TEST_VALUE));
        }

        [Test]
        public void TestIsNotEffective()
        {
            Assert.IsFalse(cut.IsEffective(BAD_TEST_VALUE));
        }

        [Test]
        public void TestStringFormat()
        {
            Assert.AreEqual(STR_FORMAT, cut.ToString());
        }
    }
}
