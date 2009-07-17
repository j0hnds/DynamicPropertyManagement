
using System;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace CronUtils
{
    
    
    [TestFixture]
    public class TestRangeCronValue
    {
        // Test constants
        const int LOWER_LIMIT = 1;
        const int UPPER_LIMIT = 31;
        const int EFFECTIVE_VALUE = 10;
        const int INEFFECTIVE_LESS = 0;
        const int INEFFECTIVE_MORE = 32;
        const string STR_FORMAT = "1-31";

        // The Class under test
        private RangeCronValue cut;

        [SetUp]
        public void SetUp()
        {
            cut = new RangeCronValue(0, 31, null, LOWER_LIMIT, UPPER_LIMIT);
        }

        [TearDown]
        public void TearDown()
        {
            cut = null;
        }
        
        [Test]
        public void TestIsEffective()
        {
            Assert.IsTrue(cut.IsEffective(EFFECTIVE_VALUE));
        }

        [Test]
        public void TestIsEffectiveEqualLowerLimit()
        {
            Assert.IsTrue(cut.IsEffective(LOWER_LIMIT));
        }

        [Test]
        public void TestIsEffectiveEqualUpperLimit()
        {
            Assert.IsTrue(cut.IsEffective(UPPER_LIMIT));
        }

        [Test]
        public void TestIsNotEffectiveLess()
        {
            Assert.IsFalse(cut.IsEffective(INEFFECTIVE_LESS));
        }

        [Test]
        public void TestIsNotEffectiveMore()
        {
            Assert.IsFalse(cut.IsEffective(INEFFECTIVE_MORE));
        }

        [Test]
        public void TestIsEffectiveAll()
        {
            for (int i=LOWER_LIMIT; i <= UPPER_LIMIT; i++)
            {
                Assert.IsTrue(cut.IsEffective(i));
            }
        }

        [Test]
        public void TestStringFormatting()
        {
            Assert.AreEqual(STR_FORMAT, cut.ToString());

            string a = "99";
            Regex PATTERN = new Regex("^([0-9]+)$");
            Match m = PATTERN.Match(a);
            Assert.IsTrue(m.Success);
            Assert.AreEqual(2, m.Groups.Count);
            //Assert.AreEqual("99", m.Groups[0]);
            Assert.AreEqual("99", m.Groups[1].Value);

            // Now, determine which exception get's thrown on bad conversion
            try
            {
                int.Parse("a23b");
            } 
            catch (System.FormatException e)
            {
                Console.Out.WriteLine("Error was: " + e.Message);
            }
        }
    }
}
