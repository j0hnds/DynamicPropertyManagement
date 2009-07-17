
using System;
using System.Globalization;
using NUnit.Framework;

namespace CronUtils
{
    
    
    [TestFixture]
    public class TestCronSpecification
    {
        private const string JANUARY_1_2009 = "2009/01/01 01:01:00";
        private const string JANUARY_1_2009_MIDNIGHT = "2009/01/01 00:00:00";
        private const string MAY_24_2009_MIDNIGHT = "2009/05/24 00:00:00";
        private const string MINUTE_ONE = "1 * * * *";
        private const string HOUR_ONE = "* 1 * * *";
        private const string DAY_ONE = "* * 1 * *";
        private const string MONTH_ONE = "* * * 1 *";
        private const string DOW_FOUR = "* * * * 4";
        private const string FULL_SPEC = "1 1 1 1 4";
        private const string FULL_WRONG_SPEC = "2 2 2 2 3";
        private const string VALID_ALL_RANGES = "0-2 0-2 1-3 1-2 2-5";
        private const string MARCH_3_2009 = "2009/03/03 03:03:00";
        private const string VALID_ALL_STEPS = "*/3 */3 */3 */3 */2";
        private const string INVALID_ALL_STEPS = "*/3 */3 */3 */3 */3";
        private const string VALID_COMBO = "45-59,3 8,*/3 */2,1-4 * 6,*/2";

        private DateTime ConvertDate(string dateString)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;

            return DateTime.ParseExact(dateString, "yyyy/MM/dd HH:mm:ss", provider);
        }
        
        [Test]
        public void TestIsMinuteOneDateEffective()
        {
            CronSpecification cs = new CronSpecification(MINUTE_ONE);
            Assert.IsTrue(cs.IsDateEffective(ConvertDate(JANUARY_1_2009)));
        }

        [Test]
        public void TestIsHourOneDateEffective()
        {
            CronSpecification cs = new CronSpecification(HOUR_ONE);
            Assert.IsTrue(cs.IsDateEffective(ConvertDate(JANUARY_1_2009)));
        }

        [Test]
        public void TestIsDayOneDateEffective()
        {
            CronSpecification cs = new CronSpecification(DAY_ONE);
            Assert.IsTrue(cs.IsDateEffective(ConvertDate(JANUARY_1_2009)));
        }

        [Test]
        public void TestIsMonthOneDateEffective()
        {
            CronSpecification cs = new CronSpecification(MONTH_ONE);
            Assert.IsTrue(cs.IsDateEffective(ConvertDate(JANUARY_1_2009)));
        }

        [Test]
        public void TestIsDOWFourDateEffective()
        {
            CronSpecification cs = new CronSpecification(DOW_FOUR);
            Assert.IsTrue(cs.IsDateEffective(ConvertDate(JANUARY_1_2009)));
        }

        [Test]
        public void TestIsFullSpecEffective()
        {
            CronSpecification cs = new CronSpecification(FULL_SPEC);
            Assert.IsTrue(cs.IsDateEffective(ConvertDate(JANUARY_1_2009)));
        }

        [Test]
        public void IsFullWrongSpecNotEffective()
        {
            CronSpecification cs = new CronSpecification(FULL_WRONG_SPEC);
            Assert.IsFalse(cs.IsDateEffective(ConvertDate(JANUARY_1_2009)));
        }

        [Test]
        public void TestIsValidAllRangesEffective()
        {
            CronSpecification cs = new CronSpecification(VALID_ALL_RANGES);
            Assert.IsTrue(cs.IsDateEffective(ConvertDate(JANUARY_1_2009)));
        }

        [Test]
        public void TestIsValidAllStepsEffective()
        {
            CronSpecification cs = new CronSpecification(VALID_ALL_STEPS);
            Assert.IsTrue(cs.IsDateEffective(ConvertDate(MARCH_3_2009)));
        }

        [Test]
        public void TestIsInvalidAllStepsEffective()
        {
            CronSpecification cs = new CronSpecification(INVALID_ALL_STEPS);
            Assert.IsFalse(cs.IsDateEffective(ConvertDate(MARCH_3_2009)));
        }

        [Test]
        public void TestIsValidComboEffective()
        {
            CronSpecification cs = new CronSpecification(VALID_COMBO);
            Assert.IsTrue(cs.IsDateEffective(ConvertDate(MARCH_3_2009)));
        }

        [Test]
        public void TestGetRawSpecification()
        {
            CronSpecification cs = new CronSpecification(MINUTE_ONE);
            Assert.AreEqual(MINUTE_ONE, cs.RawSpecification);
        }

        [Test]
        public void TestYearlyShortcut()
        {
            CronSpecification cs = new CronSpecification("@yearly");
            Assert.IsTrue(cs.IsDateEffective(ConvertDate(JANUARY_1_2009_MIDNIGHT)));
        }

        [Test]
        public void TestAnnuallyShortcut()
        {
            CronSpecification cs = new CronSpecification("@annually");
            Assert.IsTrue(cs.IsDateEffective(ConvertDate(JANUARY_1_2009_MIDNIGHT)));
        }

        [Test]
        public void TestMonthlyShortcut()
        {
            CronSpecification cs = new CronSpecification("@monthly");
            Assert.IsTrue(cs.IsDateEffective(ConvertDate(JANUARY_1_2009_MIDNIGHT)));
        }

        [Test]
        public void TestWeeklyShortcut()
        {
            CronSpecification cs = new CronSpecification("@weekly");
            Assert.IsTrue(cs.IsDateEffective(ConvertDate(MAY_24_2009_MIDNIGHT)));
        }

        [Test]
        public void TestDailyShortcut()
        {
            CronSpecification cs = new CronSpecification("@daily");
            Assert.IsTrue(cs.IsDateEffective(ConvertDate(MAY_24_2009_MIDNIGHT)));
        }

        [Test]
        public void TestHourlyShortcut()
        {
            CronSpecification cs = new CronSpecification("@hourly");
            Assert.IsTrue(cs.IsDateEffective(ConvertDate(MAY_24_2009_MIDNIGHT)));
        }
        
    }
}
