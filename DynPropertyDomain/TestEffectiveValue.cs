
using System;
using NUnit.Framework;
using DomainCore;

namespace DynPropertyDomain
{
    
    
    [TestFixture]
    public class TestEffectiveValue : BaseDomainTest
    {
        private static readonly DateTime JULY_DATE = new DateTime(2009, 7, 1, 0, 0, 0);
        private static readonly DateTime JUNE_DATE = new DateTime(2009, 6, 1, 0, 0, 0);
        private static readonly DateTime AUG_DATE = new DateTime(2009, 8, 1, 0, 0, 0);
        private static readonly DateTime SEP_DATE = new DateTime(2009, 9, 1, 0, 0, 0);

        private const string JUNE_VALUE = "JuneValue";
        private const string JULY_VALUE = "JulyValue";
        private const string DEFAULT_VALUE = "DefaultValue";

        private EffectiveValue cut;
        
        [SetUp]
        public void SetUp()
        {
            SetUpDomainFactory();

            cut = (EffectiveValue) DomainFactory.Create("EffectiveValue", false);

            CollectionRelationship rel = (CollectionRelationship) cut.GetRelationship("ValueCriteria");

            Domain vc = rel.AddNewObject();
            vc.SetValue("Value", JUNE_VALUE);
            vc.SetValue("RawCriteria", "* * * jun *");

            vc = rel.AddNewObject();
            vc.SetValue("Value", JULY_VALUE);
            vc.SetValue("RawCriteria", "* * * jul *");
            
        }

        [TearDown]
        public void TearDown()
        {
            cut = null;
        }
        
        [Test]
        public void TestNullDates()
        {
            Assert.IsTrue(cut.IsEffectiveAt(JUNE_DATE));
            Assert.IsTrue(cut.IsEffectiveAt(JULY_DATE));
            Assert.IsTrue(cut.IsEffectiveAt(AUG_DATE));

            Assert.AreEqual(JUNE_VALUE, cut.GetEffectiveValue(JUNE_DATE, DEFAULT_VALUE));
            Assert.AreEqual(JULY_VALUE, cut.GetEffectiveValue(JULY_DATE, DEFAULT_VALUE));
            Assert.AreEqual(DEFAULT_VALUE, cut.GetEffectiveValue(AUG_DATE, DEFAULT_VALUE));
        }

        [Test]
        public void TestNullStartDate()
        {
            cut.SetValue("EffectiveEndDate", JULY_DATE);
            Assert.IsTrue(cut.IsEffectiveAt(JUNE_DATE));
            Assert.IsTrue(cut.IsEffectiveAt(JULY_DATE));
            Assert.IsFalse(cut.IsEffectiveAt(AUG_DATE));
        }

        [Test]
        public void TestNullEndDate()
        {
            cut.SetValue("EffectiveStartDate", JULY_DATE);
            Assert.IsFalse(cut.IsEffectiveAt(JUNE_DATE));
            Assert.IsTrue(cut.IsEffectiveAt(JULY_DATE));
            Assert.IsTrue(cut.IsEffectiveAt(AUG_DATE));
        }

        [Test]
        public void TestBothDatesNonNull()
        {
            cut.SetValue("EffectiveStartDate", JULY_DATE);
            cut.SetValue("EffectiveEndDate", AUG_DATE);
            Assert.IsFalse(cut.IsEffectiveAt(JUNE_DATE));
            Assert.IsTrue(cut.IsEffectiveAt(JULY_DATE));
            Assert.IsTrue(cut.IsEffectiveAt(AUG_DATE));
            Assert.IsFalse(cut.IsEffectiveAt(SEP_DATE));
        }
    }
}
