using System;
using Gtk;
using System.Configuration;

namespace PropertyManager
{
    class MainClass
    {
        public static void Main (string[] args)
        {
            Console.WriteLine("From Main");
            Console.WriteLine(ConfigurationManager.AppSettings["Message"]);
            DataSourceConfig dsc = ConfigurationManager.GetSection("local-machine") as DataSourceConfig;
            Console.WriteLine(String.Format("Host = {0}, DB Name = {1}",
                                            dsc.HostName,
                                            dsc.DBName));
            Application.Init ();
            MainWindow win = new MainWindow ();
            win.Show ();
            Application.Run ();
        }
    }
}