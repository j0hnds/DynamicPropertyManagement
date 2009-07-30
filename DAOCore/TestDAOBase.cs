
using System;
using System.Data;
using MySql.Data.MySqlClient;
using NUnit.Framework;

namespace DAOCore
{
    
    
    [TestFixture]
    public class TestDataSource
    {
        private DataSource cut;
        
        [SetUp]
        public void SetUp()
        {
            cut = DataSource.Instance;
        }
        [TearDown]
        public void TearDown()
        {
            cut = null;
        }
        [Test]
        public void TestGoodConnection()
        {
            cut.Host = "localhost";
            cut.DBName = "online_logging";
            cut.UserID = "siehd";
            cut.Password = "jordan123";
            IDbConnection conn = cut.Connection;
            cut.Close();
        }
        [Test]
        public void TestBadConnection()
        {
            cut.Host = "localhost";
            cut.DBName = "online_logging";
            cut.UserID = "djs";
            cut.Password = "joe";
            IDbConnection conn = null;
            try
            {
                conn = cut.Connection;
                Assert.Fail("Should have thrown an exception");
            }
            catch (MySqlException e)
            {
                Console.Out.WriteLine("Message: " + e.Message);
            }
            finally
            {
                cut.Close();
            }
        }
    }
}
