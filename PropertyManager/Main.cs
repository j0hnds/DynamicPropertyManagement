using System;
using Gtk;
using System.Configuration;
using log4net;

namespace PropertyManager
{
    class MainClass
    {
        public static void Main (string[] args)
        {
            ILog log = LogManager.GetLogger(typeof(MainClass));
            DataSourceConfig dsc = ConfigurationManager.GetSection("local-machine") as DataSourceConfig;
            log.DebugFormat("Host = {0}, DB Name = {1}",
                                            dsc.HostName,
                                            dsc.DBName);
            Application.Init ();
            MainWindow win = new MainWindow ();
            LoginDlg dlg = new LoginDlg();
            if (dlg.DoModal())
            {
                win.Show ();
                Application.Run ();
            }
                
        }
    }
}