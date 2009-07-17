
using System;
using NUnit.Framework;

namespace CronUtils
{
    
    
    [TestFixture]
    public class TestStepCronValue
    {
        // Test constants
        const int STEP_VALUE = 3;
        const string STR_FORMAT = "*/3";

        // The class under test
        private StepCronValue cut;

        [SetUp]
        public void SetUp()
        {
            cut = new StepCronValue(0, 31, null, STEP_VALUE);
        }

        [TearDown]
        public void TearDown()
        {
            cut = null;
        }

        [Test]
        public void TestIsEffective()
        {
            Assert.IsTrue(cut.IsEffective(STEP_VALUE));
        }

        [Test]
        public void TestIsEffectiveBunch()
        {
            for (int i=0; i<10*STEP_VALUE; i += STEP_VALUE)
            {
                Assert.IsTrue(cut.IsEffective(i), "Value {0} should have been effective", i);
            }
        }

        [Test]
        public void TestIsNotEffectiveBunch()
        {
            for (int i=0; i<20; i++)
            {
                if (i % STEP_VALUE != 0)
                {
                    Assert.IsFalse(cut.IsEffective(i), "Value {0} should not have been effective", i);
                }
            }
        }

        [Test]
        public void TestStringFormat()
        {
            Assert.AreEqual(STR_FORMAT, cut.ToString());
        }
    }
}
