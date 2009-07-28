
using System;
using NUnit.Framework;

namespace DomainCore
{
    
    
    [TestFixture]
    public class TestLongAttribute
    {
        private Attribute cut;

        [SetUp]
        public void SetUp()
        {
            cut = new LongAttribute(null, "attr1", false);
        }
        
        [Test]
        public void TestDefaults()
        {
            Assert.AreEqual("attr1", cut.Name);
            Assert.AreEqual(0, cut.Value);
            Assert.IsFalse(cut.Id);
        }

        [Test]
        public void TestSetWithLong()
        {
            cut.Value = 23L;
            Assert.IsTrue(cut.Dirty);
            Assert.AreEqual(23, cut.Value);
        }
        [Test]
        public void TestSetWithString()
        {
            cut.Value = "42";
            Assert.IsTrue(cut.Dirty);
            Assert.AreEqual(42, cut.Value);
        }
    }
}
