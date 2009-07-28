
using System;
using NUnit.Framework;

namespace DomainCore
{
    
    
    [TestFixture]
    public class TestStringAttribute
    {
        private Attribute cut;
        private Attribute cutId;

        [SetUp]
        public void SetUp()
        {
            cut = new StringAttribute(null, "attr1", false);
            cutId = new StringAttribute(null, "attr2", true);
        }

        [TearDown]
        public void TearDown()
        {
            cut = null;
            cutId = null;
        }
        
        [Test]
        public void TestConstructorAttributes()
        {
            Assert.AreEqual("attr1", cut.Name);
            Assert.AreEqual("attr2", cutId.Name);
            Assert.IsFalse(cut.Id);
            Assert.IsTrue(cutId.Id);
            Assert.IsNull(cut.Value);
            Assert.IsNull(cutId.Value);
            Assert.IsFalse(cut.Dirty);
            Assert.IsFalse(cut.Dirty);
            Assert.IsFalse(cut.Populating);
            Assert.IsFalse(cutId.Populating);
        }

        [Test]
        public void TestSetEmptyAttributeNull()
        {
            cut.Value = null;
            Assert.IsNull(cut.Value);
            Assert.IsFalse(cut.Dirty);
        }
        [Test]
        public void TestSetEmptyAttributeNonNull()
        {
            cut.Value = "Bob";
            Assert.AreEqual("Bob", cut.Value);
            Assert.IsTrue(cut.Dirty);
        }
        [Test]
        public void TestClean()
        {
            cut.Value = "Bob";
            Assert.AreEqual("Bob", cut.Value);
            Assert.IsTrue(cut.Dirty);
            cut.Dirty = false;
            Assert.IsFalse(cut.Dirty);
        }

        [Test]
        public void TestPopulating()
        {
            BaseAttribute.BeginPopulation();
            cut.Value = "newValue";
            BaseAttribute.EndPopulation();
            Assert.IsFalse(cut.Dirty);
            Assert.AreEqual("newValue", cut.Value);
            cut.Value = "differentValue";
            Assert.IsTrue(cut.Dirty);
            Assert.AreEqual("differentValue", cut.Value);
        }

        [Test]
        public void TestSetNonNullToEqualNonNull()
        {
            BaseAttribute.BeginPopulation();
            cut.Value = "OriginalValue";
            BaseAttribute.EndPopulation();
            cut.Value = "OriginalValue";
            Assert.IsFalse(cut.Dirty);
            Assert.AreEqual("OriginalValue", cut.Value);
        }
        [Test]
        public void TestSetNonNullToNull()
        {
            BaseAttribute.BeginPopulation();
            cut.Value = "OriginalValue";
            BaseAttribute.EndPopulation();
            cut.Value = null;
            Assert.IsTrue(cut.Dirty);
            Assert.IsNull(cut.Value);
        }
    }
}
