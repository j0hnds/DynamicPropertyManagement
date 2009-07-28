
using System;
using System.Data;
using MySql.Data.MySqlClient;
using NUnit.Framework;

namespace PropertyManager
{
    
    
    [TestFixture]
    public class TestDAO
    {
        private dao cut;
        
        [SetUp]
        public void SetUp()
        {
            cut = new dao();
        }
        [TearDown]
        public void TearDown()
        {
            cut = null;
        }
        [Test]
        public void TestGoodConnection()
        {
            IDbConnection conn = cut.GetConnection("localhost", "online_logging", "siehd", "jordan123");
            conn.Close();
        }
        [Test]
        public void TestBadConnection()
        {
            IDbConnection conn = null;
            try
            {
                conn = cut.GetConnection("localhost", "online_logging", "djs", "joe");
                Assert.Fail("Should have thrown an exception");
            }
            catch (MySqlException e)
            {
                Console.Out.WriteLine("Message: " + e.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
    }
}
