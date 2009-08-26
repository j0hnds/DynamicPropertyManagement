using System;
using Gtk;
using System.Configuration;
using log4net;
using DomainCore;

namespace PropertyManager
{
    /// <summary>
    /// The Main application class for the Property Manager Application.
    /// </summary>
    class MainClass
    {
        /// <summary>
        /// Configures the domain factory for this application.
        /// </summary>
        /// <remarks>
        /// This tells the factory where to look for domain objects and their
        /// associated DAO objects.
        /// </remarks>
        private static void ConfigureDomainFactory()
        {
            ILog log = LogManager.GetLogger(typeof(MainClass));

            log.Debug("Configuring Domain Factory...");
            
            // Set up the Domain Factory settings.
            string domainAssembly = ConfigurationManager.AppSettings["DomainAssembly"];
            string domainNamespace = ConfigurationManager.AppSettings["DomainNamespace"];
            string daoAssembly = ConfigurationManager.AppSettings["DAOAssembly"];
            string daoNamespace = ConfigurationManager.AppSettings["DAONamespace"];

            log.DebugFormat("DAO Assembly = {0}", daoAssembly);
            DomainFactory.DAOAssembly = daoAssembly;
            log.DebugFormat("DAO Namespace = {0}", daoNamespace);
            DomainFactory.DAONamespace = daoNamespace;
            log.DebugFormat("Domain Assembly = {0}", domainAssembly);
            DomainFactory.DomainAssembly = domainAssembly;
            log.DebugFormat("Domain Namespace = {0}", domainNamespace);
            DomainFactory.DomainNamespace = domainNamespace;

            log.Debug("Done configuring Domain Factory");
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">
        /// The command line arguments.
        /// </param>
        public static void Main (string[] args)
        {
            ILog log = LogManager.GetLogger(typeof(MainClass));

            ConfigureDomainFactory();

            DataSourceConfig dsc = ConfigurationManager.GetSection("local-machine") as DataSourceConfig;
            log.DebugFormat("Host = {0}, DB Name = {1}",
                                            dsc.HostName,
                                            dsc.DBName);
            Application.Init ();
            MainWindow win = new MainWindow ();
            LoginDlg dlg = new LoginDlg();
            if (dlg.DoModal())
            {
                win.SetUpApplication();
                win.Show ();
                Application.Run ();
            }
                
        }
    }
}