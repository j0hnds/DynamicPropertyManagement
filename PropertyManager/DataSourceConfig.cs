
using System;
using System.Configuration;

namespace PropertyManager
{
    
    /// <summary>
    /// Custom configuration class to extract data base connection information
    /// from the application configuration file.
    /// </summary>
    public class DataSourceConfig : ConfigurationSection
    {

        /// <summary>
        /// Static constructor to set up the structure of the configuration
        /// entry.
        /// </summary>
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

        /// <value>
        /// The attribute in the configuration element for the host name.
        /// </value>
        private static ConfigurationProperty hostName;
        /// <value>
        /// The attribute in the configuration element for the data base name.
        /// </value>
        private static ConfigurationProperty dbName;

        /// <value>
        /// The root element that contains the above attributes.
        /// </value>
        private static ConfigurationPropertyCollection properties;

        /// <value>
        /// The host name of the data base connection.
        /// </value>
        [ConfigurationProperty("hostName", IsRequired=true)]
        public string HostName
        {
            get { return (string) base[hostName]; }
        }

        /// <value>
        /// The name of the data base.
        /// </value>
        [ConfigurationProperty("dbName", IsRequired=true)]
        public string DBName
        {
            get { return (string) base[dbName]; }
        }

        /// <value>
        /// The collection of attributes for the configuration element.
        /// </value>
        protected override ConfigurationPropertyCollection Properties
        {
            get { return properties; }
        }
    }
}
