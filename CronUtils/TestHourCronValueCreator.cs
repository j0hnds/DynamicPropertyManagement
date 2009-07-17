
using System;
using NUnit.Framework;

namespace CronUtils
{
    
    
    [TestFixture]
    public class TestHourCronValueCreator
    {

        private const string WILDCARD_VALUE = "*";
        private const string SINGLE_VALUE = "12";
        private const int LOWER_LIMIT = 18;
        private const int UPPER_LIMIT = 23;
        private static readonly string RANGE_VALUE = String.Format("{0}-{1}", LOWER_LIMIT, UPPER_LIMIT);
        private const int STEP = 4;
        private static readonly string STEP_VALUE = String.Format("*/{0}", STEP);
        private const string INVALID_WILDCARD = "%";
        private const string INVALID_SINGLE_VALUE = "62";
        private const string TWISTED_RANGE = "11-10";
        private const string INVALID_LOWER_RANGE = "24-18";
        private const string INVALID_UPPER_RANGE = "23-24";

        private CronValueFactory.CronValueCreator cut;

        [SetUp]
        public void SetUp()
        {
            cut = CronValueFactory.GetHourCreator();
        }

        [TearDown]
        public void TearDown()
        {
            cut = null;
        }
            
        
        [Test]
        public void TestGetHourWildcardCreator()
        {
            CronEffectiveValue cv = cut.CreateCronValue(WILDCARD_VALUE);
            Assert.IsNotNull(cv);

            // Any value should work
            for (int i = 0; i <= 59; i++)
            {
                Assert.IsTrue(cv.IsEffective(i), "Value {0} should have been effective", i);
            }
        }

        [Test]
        public void TestGetHourSingleValueCreator()
        {
            CronEffectiveValue cv = cut.CreateCronValue(SINGLE_VALUE);
            int sv = int.Parse(SINGLE_VALUE);
            Assert.IsNotNull(cv);
            Assert.IsTrue(cv.IsEffective(sv));
            Assert.IsFalse(cv.IsEffective(sv - 1));
            Assert.IsFalse(cv.IsEffective(sv + 1));
        }

        [Test]
        public void TestGetHourRangeCreator()
        {
            CronEffectiveValue cv = cut.CreateCronValue(RANGE_VALUE);
            Assert.IsNotNull(cv);

            // Any value in range should work
            for (int i = LOWER_LIMIT; i <= UPPER_LIMIT; i++)
            {
                Assert.IsTrue(cv.IsEffective(i), "Value {0} should have been effective", i);
            }

            Assert.IsFalse(cv.IsEffective(LOWER_LIMIT - 1));
            Assert.IsFalse(cv.IsEffective(UPPER_LIMIT + 1));
        }

        [Test]
        public void TestGetHourStepCreator()
        {
            CronEffectiveValue cv = cut.CreateCronValue(STEP_VALUE);
            Assert.IsNotNull(cv);

            // Any value of step should work
            for (int i = 0; i < (10 * STEP); i += STEP)
            {
                Assert.IsTrue(cv.IsEffective(i), "Value {0} should have been effective", i);
            }

            // Any value out of step should fail
            for (int i = 1; i < 100; i++)
            {
                if (i % STEP != 0)
                {
                    Assert.IsFalse(cv.IsEffective(i), "Value {0} should have been ineffective", i);
                }
            }
        }

        [Test]
        public void TestInvalidWildcard()
        {
            try
            {
                cut.CreateCronValue(INVALID_WILDCARD);
                Assert.Fail("Should have thrown exception");
            }
            catch (Exception)
            {
            }
        }

        [Test]
        public void TestInvalidSingleValue()
        {
            try
            {
                cut.CreateCronValue(INVALID_SINGLE_VALUE);
                Assert.Fail("Should have thrown exception");
            }
            catch (Exception)
            {
            }
        }

        [Test]
        public void TestInvalidLowerRange()
        {
            try
            {
                cut.CreateCronValue(INVALID_LOWER_RANGE);
                Assert.Fail("Should have thrown exception");
            }
            catch (Exception)
            {
            }
        }

        [Test]
        public void TestInvalidUpperRange()
        {
            try
            {
                cut.CreateCronValue(INVALID_UPPER_RANGE);
                Assert.Fail("Should have thrown exception");
            }
            catch (Exception)
            {
            }
        }

        [Test]
        public void TestTwistedRange()
        {
            try
            {
                cut.CreateCronValue(TWISTED_RANGE);
                Assert.Fail("Should have thrown exception");
            }
            catch (Exception)
            {
            }
        }
    }
}
