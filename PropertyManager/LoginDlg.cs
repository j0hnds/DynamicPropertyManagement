
using System;
using System.Collections.Generic;
using System.Configuration;
using DAOCore;

namespace PropertyManager
{
    
    
    public partial class LoginDlg : Gtk.Dialog
    {
        private static readonly Dictionary<string,string> ENVIRONMENT_MAP = new Dictionary<string, string>();

        static LoginDlg()
        {
            ENVIRONMENT_MAP["Local"] = "local-machine";
            ENVIRONMENT_MAP["Development"] = "dev-machine";
            ENVIRONMENT_MAP["QA"] = "qa-machine";
            ENVIRONMENT_MAP["UAT"] = "uat-machine";
            ENVIRONMENT_MAP["Production"] = "prod-machine";
        }
        
        public LoginDlg()
        {
            this.Build();
        }

        public bool DoModal()
        {
            return DoModal(null);
        }
        
        public bool DoModal(Gtk.Window parent)
        {
            bool ok = false;

            TransientFor = parent;
            if (Run() == Gtk.ResponseType.Ok.value__)
            {
                // Here's a good spot to try to log in.
                DataSource ds = DataSource.Instance;
                ds.Close();

                // Get the environment name
                string environment = ENVIRONMENT_MAP[cbEnvironment.ActiveText];

                // Get the configuration for that environment
                DataSourceConfig dsc = ConfigurationManager.GetSection("local-machine") as DataSourceConfig;

                string host = dsc.HostName;
                string dbName = dsc.DBName;
                string userName = txtUserName.Text;
                string password = txtPassword.Text;

                // Set the data source variables.
                ds.Host = host;
                ds.DBName = dbName;
                ds.UserID = userName;
                ds.Password = password;

                try
                {
                    ds.TestConnection();
                    ok = true;
                }
                catch (Exception e)
                {
                    Gtk.MessageDialog md = new Gtk.MessageDialog(this,
                                                                 Gtk.DialogFlags.DestroyWithParent,
                                                                 Gtk.MessageType.Error,
                                                                 Gtk.ButtonsType.Close,
                                                                 e.Message);
                    md.Run();
                    md.Destroy();

                    ok = false;
                }
                
            }

            Destroy();
    
            return ok;
        }

        private void CheckOKSensitivity()
        {
            string userName = txtUserName.Text;
            bool userNameOk = userName.Length > 0;
            string password = txtPassword.Text;
            bool passwordOk = password.Length > 0;
            bool environmentok = cbEnvironment.Active >= 0;

            buttonOk.Sensitive = userNameOk && passwordOk && environmentok;
        }

        protected virtual void UserNameChanged (object sender, System.EventArgs e)
        {
            CheckOKSensitivity();
        }

        protected virtual void PasswordChanged (object sender, System.EventArgs e)
        {
            CheckOKSensitivity();
        }

        protected virtual void EnvironmentChanged (object sender, System.EventArgs e)
        {
            CheckOKSensitivity();
        }
    }

}
