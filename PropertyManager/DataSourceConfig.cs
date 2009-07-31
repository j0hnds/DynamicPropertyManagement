
using System;
using System.Configuration;

namespace PropertyManager
{
    
    
    public class DataSourceConfig : ConfigurationSection
    {

        static DataSourceConfig()
        {
            hostName = new ConfigurationProperty("hostName",
                                                 typeof(string),
                                                 null,
                                                 ConfigurationPropertyOptions.IsRequired);
            dbName = new ConfigurationProperty("dbName",
                                               typeof(String),
                                               null,
                                               ConfigurationPropertyOptions.IsRequired);

            properties = new ConfigurationPropertyCollection();
            properties.Add(hostName);
            properties.Add(dbName);
        }

        private static ConfigurationProperty hostName;
        private static ConfigurationProperty dbName;

        private static ConfigurationPropertyCollection properties;

        [ConfigurationProperty("hostName", IsRequired=true)]
        public string HostName
        {
            get { return (string) base[hostName]; }
        }
        [ConfigurationProperty("dbName", IsRequired=true)]
        public string DBName
        {
            get { return (string) base[dbName]; }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return properties; }
        }
    }
}
