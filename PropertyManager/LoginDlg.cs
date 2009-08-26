
using System;
using System.Collections.Generic;
using System.Configuration;
using DAOCore;

namespace PropertyManager
{
    
    /// <summary>
    /// The dialog used to collect authentication credentials from the user.
    /// </summary>
    public partial class LoginDlg : Gtk.Dialog
    {
        /// <summary>
        /// A mapping between the label and the configuration entry name for the
        /// different environments that we can log onto.
        /// </summary>
        private static readonly Dictionary<string,string> ENVIRONMENT_MAP = new Dictionary<string, string>();

        /// <summary>
        /// Static constructor to initialize the environment map.
        /// </summary>
        static LoginDlg()
        {
            ENVIRONMENT_MAP["Local"] = "local-machine";
            ENVIRONMENT_MAP["Development"] = "dev-machine";
            ENVIRONMENT_MAP["QA"] = "qa-machine";
            ENVIRONMENT_MAP["UAT"] = "uat-machine";
            ENVIRONMENT_MAP["Production"] = "prod-machine";
        }

        /// <summary>
        /// Constructs a new LogingDlg object.
        /// </summary>
        public LoginDlg()
        {
            this.Build();
        }

        /// <summary>
        /// Displays the login dialog as model with no parent.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the OK button was pressed.
        /// </returns>
        public bool DoModal()
        {
            return DoModal(null);
        }

        /// <summary>
        /// Displays the login dialog as modal with the specified parent.
        /// </summary>
        /// <param name="parent">
        /// The parent window of the dialog. May be null.
        /// </param>
        /// <returns>
        /// <c>true</c> if the OK button was pressed.
        /// </returns>
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
                // string environment = ENVIRONMENT_MAP[cbEnvironment.ActiveText];

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

        /// <summary>
        /// Helper method to determine if the OK button should be
        /// sensitized.
        /// </summary>
        private void CheckOKSensitivity()
        {
            string userName = txtUserName.Text;
            bool userNameOk = userName.Length > 0;
            string password = txtPassword.Text;
            bool passwordOk = password.Length > 0;
            bool environmentok = cbEnvironment.Active >= 0;

            buttonOk.Sensitive = userNameOk && passwordOk && environmentok;
        }

        /// <summary>
        /// Signal handler for change to the user name.
        /// </summary>
        /// <param name="sender">
        /// The user name entry field.
        /// </param>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected virtual void UserNameChanged (object sender, System.EventArgs e)
        {
            CheckOKSensitivity();
        }

        /// <summary>
        /// Signal handler for change to the password.
        /// </summary>
        /// <param name="sender">
        /// The password entry field.
        /// </param>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected virtual void PasswordChanged (object sender, System.EventArgs e)
        {
            CheckOKSensitivity();
        }

        /// <summary>
        /// Signal handler for change to the environment selection.
        /// </summary>
        /// <param name="sender">
        /// The environment selection combo box.
        /// </param>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected virtual void EnvironmentChanged (object sender, System.EventArgs e)
        {
            CheckOKSensitivity();
        }
    }

}
