
using System;
using NUnit.Framework;
using DomainCore;

namespace DynPropertyDomain
{
    
    
    [TestFixture]
    public class TestDynamicProperty : BaseDomainTest
    {
        private static readonly DateTime JULY_DATE = new DateTime(2009, 7, 1, 0, 0, 0);
        private static readonly DateTime JUNE_DATE = new DateTime(2009, 6, 1, 0, 0, 0);
        private static readonly DateTime AUG_DATE = new DateTime(2009, 8, 1, 0, 0, 0);
        private static readonly DateTime SEP_DATE = new DateTime(2009, 9, 1, 0, 0, 0);
        private static readonly DateTime OCT_DATE = new DateTime(2009, 10, 1, 0, 0, 0);

        private const string JUNE_VALUE = "JuneValue";
        private const string JULY_VALUE = "JulyValue";
        private const string AUG_VALUE = "AugustValue";
        private const string SEP_VALUE = "SeptemberValue";
        private const string DEFAULT_VALUE = "DefaultValue";

        private DynamicProperty cut;

        [SetUp]
        public void SetUp()
        {
            SetUpDomainFactory();

            cut = (DynamicProperty) DomainFactory.Create("DynamicProperty", false);
            cut.SetValue("DefaultValue", DEFAULT_VALUE);

            // Set up the effective values and the value criteria for the test
            CollectionRelationship evRel = (CollectionRelationship) cut.GetRelationship("EffectiveValues");

            Domain ev = evRel.AddNewObject();
            ev.SetValue("EffectiveStartDate", JULY_DATE);
            ev.SetValue("EffectiveEndDate", AUG_DATE);

            CollectionRelationship vcRel = (CollectionRelationship) ev.GetRelationship("ValueCriteria");

            Domain vc = vcRel.AddNewObject();
            vc.SetValue("Value", JUNE_VALUE);
            vc.SetValue("RawCriteria", "* * * jun *");

            vc = vcRel.AddNewObject();
            vc.SetValue("Value", JULY_VALUE);
            vc.SetValue("RawCriteria", "* * * jul *");

            ev = evRel.AddNewObject();
            ev.SetValue("EffectiveStartDate", AUG_DATE);
            ev.SetValue("EffectiveEndDate", SEP_DATE);

            vcRel = (CollectionRelationship) ev.GetRelationship("ValueCriteria");

            vc = vcRel.AddNewObject();
            vc.SetValue("Value", AUG_VALUE);
            vc.SetValue("RawCriteria", "* * * aug *");

            vc = vcRel.AddNewObject();
            vc.SetValue("Value", SEP_VALUE);
            vc.SetValue("RawCriteria", "* * * sep *");
        }
        
        [Test]
        public void TestNoEffMatch()
        {
            Assert.AreEqual(DEFAULT_VALUE, cut.GetEffectiveValue(OCT_DATE));
        }
        [Test]
        public void TestEFJuneMatch()
        {
            Assert.AreEqual(DEFAULT_VALUE, cut.GetEffectiveValue(JUNE_DATE));
        }
        [Test]
        public void TestEFJulyMatch()
        {
            Assert.AreEqual(JULY_VALUE, cut.GetEffectiveValue(JULY_DATE));
        }
        [Test]
        public void TestEFAugMatch()
        {
            Assert.AreEqual(DEFAULT_VALUE, cut.GetEffectiveValue(AUG_DATE));
        }
        [Test]
        public void TestEFSepMatch()
        {
            Assert.AreEqual(SEP_VALUE, cut.GetEffectiveValue(SEP_DATE));
        }
    }
}
