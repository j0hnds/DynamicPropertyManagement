
using System;
using System.Collections.Generic;
using DomainCore;
using NUnit.Framework;

namespace DynPropertyDomain
{
    
    
    [TestFixture]
    public class TestDynamicPropertyDAO : BaseDomainTest
    {
        [SetUp]
        public void SetUp()
        {
            SetUpDomainFactory();
            SetUpConnectionString();
        }
        
        [Test]
        public void TestRoundTripDynamicProperty()
        {
            // Construct an application and save it
            Domain app = DomainFactory.Create("Application");
            app.SetValue("Name", "__Sample");
            app.Save();

            // Construct property definition and save it
            Domain prop = DomainFactory.Create("PropertyDefinition");
            prop.SetValue("Category", "__Category");
            prop.SetValue("Name", "__PropertyName");
            prop.SetValue("DataType", 9);
            prop.SetValue("Description", "__Description");
            prop.Save();
            
            // Construct a dyn property and save it
            Domain dp = DomainFactory.Create("DynamicProperty");
            dp.SetValue("ApplicationId", app.GetValue("Id"));
            dp.SetValue("PropertyId", prop.GetValue("Id"));
            dp.SetValue("Qualifier", "__Qual");
            dp.SetValue("DefaultValue", "__DFLT");
            /* string sql = */ dp.SaveSQL();
            dp.Save();

            // Construct an effective value and save it
            Domain ev = DomainFactory.Create("EffectiveValue");
            ev.SetValue("AssignId", dp.GetValue("Id"));
            ev.SetValue("EffectiveStartDate", DateTime.Now);
            // Leave the end date as null
            ev.Save();

            // Construct a value criteria and save it
            Domain vc = DomainFactory.Create("ValueCriteria");
            vc.SetValue("EffectiveId", ev.GetValue("Id"));
            vc.SetValue("RawCriteria", "* * * * *");
            vc.SetValue("Value", "_Value");
            vc.Save();

            // ***************************************************************
            // Everything has been saved, now test the updaters

            // Dynamic Property updater
            dp.SetValue("Qualifier", "__Nual");
            dp.Save();

            // Effective value updater
            ev.SetValue("EffectiveStartDate", null);
            ev.SetValue("EffectiveEndDate", DateTime.Now);
            ev.Save();

            // Value criteria updater
            vc.SetValue("Value", "__NewValue");
            vc.Save();

            // *************************************************************
            // Everything has been updated, now test the individual getter.

            // Get the Dynamic property and everything that goes with it.
            Domain dpg = dp.DAO.GetObject(dp.GetValue("Id"));
            Assert.AreEqual(dpg.GetValue("Id"), dp.GetValue("Id"));
            Assert.AreEqual(dpg.GetValue("ApplicationId"), dp.GetValue("ApplicationId"));
            // Assert.AreEqual(dpg.GetValue("ApplicationName"), dp.GetValue("ApplicationName"));
            Assert.AreEqual(dpg.GetValue("PropertyId"), dp.GetValue("PropertyId"));
            // Assert.AreEqual(dpg.GetValue("Category"), dp.GetValue("Category"));
            // Assert.AreEqual(dpg.GetValue("PropertyName"), dp.GetValue("PropertyName"));
            Assert.AreEqual(dpg.GetValue("DefaultValue"), dp.GetValue("DefaultValue"));
            // Assert.AreEqual(dpg.GetValue("PropertyType"), dp.GetValue("PropertyType"));
            Assert.AreEqual(dpg.GetValue("Qualifier"), dp.GetValue("Qualifier"));

            List<Domain> effValues = dpg.GetCollection("EffectiveValues");
            Assert.AreEqual(1, effValues.Count);
            Domain evg = effValues[0];
            Assert.AreEqual(evg.GetValue("Id"), ev.GetValue("Id"));
            Assert.AreEqual(evg.GetValue("AssignId"), ev.GetValue("AssignId"));
//            Assert.AreEqual(evg.GetValue("EffectiveStartDate"), ev.GetValue("EffectiveStartDate"));
//            Assert.AreEqual(evg.GetValue("EffectiveEndDate"), ev.GetValue("EffectiveEndDate"));

            List<Domain> vcs = evg.GetCollection("ValueCriteria");
            Assert.AreEqual(1, vcs.Count);
            Domain vcg = vcs[0];
            Assert.AreEqual(vcg.GetValue("Id"), vc.GetValue("Id"));
            Assert.AreEqual(vcg.GetValue("EffectiveId"), vc.GetValue("EffectiveId"));
            Assert.AreEqual(vcg.GetValue("RawCriteria"), vc.GetValue("RawCriteria"));
            Assert.AreEqual(vcg.GetValue("Value"), vc.GetValue("Value"));

            // Get the Effective value and verify it.
            evg = ev.DAO.GetObject(ev.GetValue("Id"));
            Assert.AreEqual(vcg.GetValue("Id"), vc.GetValue("Id"));
            Assert.AreEqual(vcg.GetValue("EffectiveId"), vc.GetValue("EffectiveId"));
            Assert.AreEqual(vcg.GetValue("RawCriteria"), vc.GetValue("RawCriteria"));
            Assert.AreEqual(vcg.GetValue("Value"), vc.GetValue("Value"));

            // Get the Value Criteria and verify it.
            vcg = vc.DAO.GetObject(ev.GetValue("Id"));
            Assert.AreEqual(vcg.GetValue("EffectiveId"), vc.GetValue("EffectiveId"));
            Assert.AreEqual(vcg.GetValue("RawCriteria"), vc.GetValue("RawCriteria"));
            Assert.AreEqual(vcg.GetValue("Value"), vc.GetValue("Value"));

            // Delete the dynamic property; everything else should just
            // dissappear.
            dp.ForDelete = true;
            dp.Save();
        }
    }
}
