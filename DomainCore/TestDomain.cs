
using System;
using NUnit.Framework;

namespace DomainCore
{
    
    
    [TestFixture]
    public class TestDomain
    {

        private Domain cut;

        [SetUp]
        public void SetUp()
        {
            cut = new Domain(null);

            // Set up one of each type of attribute.
            new LongAttribute(cut, "Id", true);
            new StringAttribute(cut, "Str", false);
            
        }

        [TearDown]
        public void TearDown()
        {
            cut = null;
        }
        
        [Test]
        public void TestDefaults()
        {
            Assert.IsFalse(cut.Dirty);
            Assert.IsFalse(cut.ForDelete);
            Attribute idAttribute = cut.IdAttribute;
            Assert.IsNotNull(idAttribute);
            Assert.AreEqual("Id", idAttribute.Name);
            Assert.IsFalse(cut.NewObject);
        }

        [Test]
        public void TestPopulation()
        {
            BaseAttribute.BeginPopulation();
            cut.SetValue("Id", 32);
            cut.SetValue("Str", "OtherValue");
            BaseAttribute.EndPopulation();
            Assert.IsFalse(cut.Dirty);
            Assert.AreEqual(32, cut.GetValue("Id"));
            Assert.AreEqual("OtherValue", cut.GetValue("Str"));
            cut.SetValue("Id", 82);
            cut.SetValue("Str", "SomeValue");
            Assert.IsTrue(cut.Dirty);
            Assert.AreEqual(82, cut.GetValue("Id"));
            Assert.AreEqual("SomeValue", cut.GetValue("Str"));
        }

        [Test]
        public void TestRevertConstructor()
        {
            cut.Revert();
            Assert.AreEqual(0, cut.GetValue("Id"));
            Assert.IsNull(cut.GetValue("Str"));
        }

        [Test]
        public void TestRevertPopulated()
        {
            BaseAttribute.BeginPopulation();
            cut.SetValue("Id", 44);
            cut.SetValue("Str", "NewStr");
            BaseAttribute.EndPopulation();

            cut.Revert();
            Assert.AreEqual(44, cut.GetValue("Id"));
            Assert.AreEqual("NewStr", cut.GetValue("Str"));
        }

        [Test]
        public void TestRevertDirty()
        {
            BaseAttribute.BeginPopulation();
            cut.SetValue("Id", 44);
            cut.SetValue("Str", "NewStr");
            BaseAttribute.EndPopulation();

            cut.SetValue("Id", 45);
            cut.SetValue("Str", "DifferentString");

            Assert.AreEqual(45, cut.GetValue("Id"));
            Assert.AreEqual("DifferentString", cut.GetValue("Str"));
            Assert.IsTrue(cut.Dirty);

            cut.SetValue("Id", 46);
            cut.SetValue("Str", "DifferentString1");

            Assert.AreEqual(46, cut.GetValue("Id"));
            Assert.AreEqual("DifferentString1", cut.GetValue("Str"));
            Assert.IsTrue(cut.Dirty);

            cut.Revert();
            
            Assert.AreEqual(44, cut.GetValue("Id"));
            Assert.AreEqual("NewStr", cut.GetValue("Str"));
            Assert.IsFalse(cut.Dirty);
            
            cut.SetValue("Id", 45);
            cut.SetValue("Str", "DifferentString");

            Assert.AreEqual(45, cut.GetValue("Id"));
            Assert.AreEqual("DifferentString", cut.GetValue("Str"));
            Assert.IsTrue(cut.Dirty);

            cut.Clean();

            cut.SetValue("Id", 46);
            cut.SetValue("Str", "DifferentString1");

            Assert.AreEqual(46, cut.GetValue("Id"));
            Assert.AreEqual("DifferentString1", cut.GetValue("Str"));
            Assert.IsTrue(cut.Dirty);

            cut.Revert();
            
            Assert.AreEqual(45, cut.GetValue("Id"));
            Assert.AreEqual("DifferentString", cut.GetValue("Str"));
            Assert.IsFalse(cut.Dirty);
        }
    }
}
