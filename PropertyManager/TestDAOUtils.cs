
using System;
using NUnit.Framework;

namespace PropertyManager
{
    
    
    [TestFixture]
    public class TestDAOUtils
    {
        [Test]
        public void TestQuoteNullString()
        {
            Assert.AreEqual("NULL", DAOUtils.QuoteString(null));
        }
        [Test]
        public void TestQuoteRegularString()
        {
            Assert.AreEqual("'abcd'", DAOUtils.QuoteString("abcd"));
        }
        [Test]
        public void TestQuoteEmbeddedQuoteString()
        {
            Assert.AreEqual("'don''t let''s go'", DAOUtils.QuoteString("don't let's go"));
        }

        [Test]
        public void TestConvertValueNull()
        {
            Assert.AreEqual("NULL", DAOUtils.ConvertValue(null));
        }

        [Test]
        public void TestConvertIntegerValue()
        {
            Assert.AreEqual("23", DAOUtils.ConvertValue(23));
        }

        [Test]
        public void TestConvertRegularString()
        {
            Assert.AreEqual("'abcd'", DAOUtils.ConvertValue("abcd"));
        }
        [Test]
        public void TestConvertEmbeddedQuoteString()
        {
            Assert.AreEqual("'don''t let''s go'", DAOUtils.ConvertValue("don't let's go"));
        }
    }
}
