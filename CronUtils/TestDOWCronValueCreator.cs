
using System;
using NUnit.Framework;

namespace CronUtils
{
    
    
    [TestFixture]
    public class TestDOWCronValueCreator
    {
        private const string WILDCARD_VALUE = "*";
        private const string SINGLE_VALUE = "5";
        private const int LOWER_LIMIT = 1;
        private const int UPPER_LIMIT = 3;
        private static readonly string RANGE_VALUE = String.Format("{0}-{1}", LOWER_LIMIT, UPPER_LIMIT);
        private const int STEP = 2;
        private static readonly string STEP_VALUE = String.Format("*/{0}", STEP);
        private const string INVALID_WILDCARD = "%";
        private const string INVALID_SINGLE_VALUE = "62";
        private const string TWISTED_RANGE = "5-4";
        private const string INVALID_LOWER_RANGE = "8-6";
        private const string INVALID_UPPER_RANGE = "6-8";
        private const string VALID_NAME_SINGLE_VALUE = "tue";
        private const string INVALID_NAME_SINGLE_VALUE = "Tue";
        private const string VALID_NAME_RANGE = "thu-sat";
        private const string INVALID_NAME_RANGE = "Wed-Fri";
        private const string TWISTED_NAME_RANGE = "fri-thu";
        private const string VALID_MIXED_RANGE = "tue-4";
        private const string VALID_SUNDAY_SEVEN = "7";

        private CronValueFactory.CronValueCreator cut;

        [SetUp]
        public void SetUp()
        {
            cut = CronValueFactory.GetDOWCreator();
        }

        [TearDown]
        public void TearDown()
        {
            cut = null;
        }
        
        [Test]
        public void TestValidNameSingleValue()
        {
            CronEffectiveValue cv = cut.CreateCronValue(VALID_NAME_SINGLE_VALUE);
            Assert.IsTrue(cv.IsEffective(2));
        }

        [Test]
        public void TestValidSundaySeven()
        {
            CronEffectiveValue cv = cut.CreateCronValue(VALID_SUNDAY_SEVEN);
            Assert.IsTrue(cv.IsEffective(0));
        }

        [Test]
        public void TestInvalidNameSingleValue()
        {
            try
            {
                cut.CreateCronValue(INVALID_NAME_SINGLE_VALUE);
                Assert.Fail("Should have thrown exception");
            } 
            catch (Exception)
            {
            }
        }

        [Test]
        public void TestValidNameRange()
        {
            CronEffectiveValue cv = cut.CreateCronValue(VALID_NAME_RANGE);

            for (int i = 4; i <= 6; i++)
            {
                Assert.IsTrue(cv.IsEffective(i), "Value {0} should have been effective", i);
            }

            Assert.IsFalse(cv.IsEffective(3));
            Assert.IsFalse(cv.IsEffective(7));
        }

        [Test]
        public void TestValidMixedRange()
        {
            CronEffectiveValue cv = cut.CreateCronValue(VALID_MIXED_RANGE);

            for (int i = 2; i <= 4; i++)
            {
                Assert.IsTrue(cv.IsEffective(i), "Value {0} should have been effective", i);
            }

            Assert.IsFalse(cv.IsEffective(1));
            Assert.IsFalse(cv.IsEffective(5));
        }

        [Test]
        public void TestInvalidNameRange()
        {
            try
            {
                cut.CreateCronValue(INVALID_NAME_RANGE);
                Assert.Fail("Should have thrown exception");
            }
            catch (Exception)
            {
            }
        }

        [Test]
        public void TestTwistedNameRange()
        {
            try
            {
                cut.CreateCronValue(TWISTED_NAME_RANGE);
                Assert.Fail("Should have thrown exception");
            }
            catch (Exception)
            {
            }
        }

        [Test]
        public void TestGetDOWWildcardCreator()
        {
            CronEffectiveValue cv = cut.CreateCronValue(WILDCARD_VALUE);
            Assert.IsNotNull(cv);

            // Any value should work
            for (int i = 0; i <=59; i++)
            {
                Assert.IsTrue(cv.IsEffective(i), "Value {0} should have been effective", i);
            }
        }

        [Test]
        public void TestGetDOWNSingleValueCreator()
        {
            CronEffectiveValue cv = cut.CreateCronValue(SINGLE_VALUE);

            int sv = int.Parse(SINGLE_VALUE);

            Assert.IsNotNull(cv);
            Assert.IsTrue(cv.IsEffective(sv));
            Assert.IsFalse(cv.IsEffective(sv - 1));
            Assert.IsFalse(cv.IsEffective(sv + 1));
        }

        [Test]
        public void TestDOWRangeCreator()
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
        public void TestGetDOWStepCreator()
        {
            CronEffectiveValue cv = cut.CreateCronValue(STEP_VALUE);

            Assert.IsNotNull(cv);

            // Any value of step should work
            for (int i = 0; i < (10 * STEP); i += STEP)
            {
                Assert.IsTrue(cv.IsEffective(i), "Value {0} should be effective", i);
            }

            // Any value out of step should fail
            for (int i = 0; i < 100; i++)
            {
                if (i % STEP != 0)
                {
                    Assert.IsFalse(cv.IsEffective(i), "Value {0} should be ineffective", i);
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
