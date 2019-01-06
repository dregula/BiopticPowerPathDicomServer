using System;
using System.Windows.Forms;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Data.Linq;
using System.Diagnostics;
using System.Data.SqlClient;
using System.ComponentModel;
using Common.Logging;
using ReactiveUI;
using BiopticPowerPathDicomServer.ViewModels;


namespace BiopticPowerPathDicomServer
{
    public partial class PPLoginForm : Form, IViewFor<PPLoginViewModel>
    {
        private static ILog Log = LogManager.GetLogger("PPLoginForm");

        private ServerConfiguration serverconfig;

        #region "Component variables (leave alone!)"
        private System.ComponentModel.Container components = null;
        internal TextBox tbPreamble;
        private GroupBox gbLogon;
        internal TextBox tbPassword;
        internal TextBox tbDatabase;
        internal TextBox tbUsername;
        internal TextBox tbServer;
        private ComboBox cbDatabase;
        private ComboBox cbServer;
        private TextBox tbPasswordInput;
        private Button bntOK;
        private Button btnQuit;
        private TextBox tbUserNameInput;
        #endregion

        #region "ReactiveUI.WinForms"
        public PPLoginViewModel VM { get; set; }

        object IViewFor.ViewModel
        {
            get { return VM; }
            set { VM = (PPLoginViewModel)value; }
        }

        PPLoginViewModel IViewFor<PPLoginViewModel>.ViewModel
        {
            get { return VM; }
            set { VM = value; }
        }
        # endregion

        public PPLoginForm()
        {
            serverconfig = new ServerConfiguration();
            VM = new PPLoginViewModel();
            VM.ServerConfig = serverconfig;
            InitializeAndBind();
        }

        public PPLoginForm(ServerConfiguration serverConfig)
        {
            serverconfig = serverConfig;
            VM = new PPLoginViewModel();
            VM.ServerConfig = serverConfig;
            InitializeAndBind();
        }

        private void InitializeAndBind()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            #region "Bind this Form to the ReactiveUI viewmodel"
            this.Bind(VM, x => x.UserID, x => x.tbUserNameInput.Text);
            this.Bind(VM, x => x.Password, x => x.tbPasswordInput.Text);
//not-working this.OneWayBind(VM, x => x.RecentServers, x => x.cbServer.DataSource);
            this.Bind(VM, x => x.DataSource, x => x.cbServer.Text);
            this.Bind(VM, x => x.InitialCatalog, x => x.cbDatabase.Text);
//not-working this.OneWayBind(VM, x => x.AvailableDatabases , x => x.cbDatabase.DataSource);

            //static-type error            this.BindCommand(VM, x => x.OKCmd, x => x.bntOK);
            #endregion
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbPreamble = new System.Windows.Forms.TextBox();
            this.gbLogon = new System.Windows.Forms.GroupBox();
            this.bntOK = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            this.tbPasswordInput = new System.Windows.Forms.TextBox();
            this.tbUserNameInput = new System.Windows.Forms.TextBox();
            this.cbDatabase = new System.Windows.Forms.ComboBox();
            this.cbServer = new System.Windows.Forms.ComboBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbDatabase = new System.Windows.Forms.TextBox();
            this.tbUsername = new System.Windows.Forms.TextBox();
            this.tbServer = new System.Windows.Forms.TextBox();
            this.gbLogon.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbPreamble
            // 
            this.tbPreamble.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbPreamble.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPreamble.Location = new System.Drawing.Point(30, 37);
            this.tbPreamble.Multiline = true;
            this.tbPreamble.Name = "tbPreamble";
            this.tbPreamble.ReadOnly = true;
            this.tbPreamble.Size = new System.Drawing.Size(1060, 83);
            this.tbPreamble.TabIndex = 9;
            this.tbPreamble.TabStop = false;
            this.tbPreamble.Text = "Enter your Powerpath username and password.";
            // 
            // gbLogon
            // 
            this.gbLogon.Controls.Add(this.bntOK);
            this.gbLogon.Controls.Add(this.btnQuit);
            this.gbLogon.Controls.Add(this.tbPasswordInput);
            this.gbLogon.Controls.Add(this.tbUserNameInput);
            this.gbLogon.Controls.Add(this.cbDatabase);
            this.gbLogon.Controls.Add(this.cbServer);
            this.gbLogon.Controls.Add(this.tbPassword);
            this.gbLogon.Controls.Add(this.tbDatabase);
            this.gbLogon.Controls.Add(this.tbUsername);
            this.gbLogon.Controls.Add(this.tbServer);
            this.gbLogon.Location = new System.Drawing.Point(30, 95);
            this.gbLogon.Name = "gbLogon";
            this.gbLogon.Size = new System.Drawing.Size(1086, 618);
            this.gbLogon.TabIndex = 17;
            this.gbLogon.TabStop = false;
            this.gbLogon.Text = "Logon";
            // 
            // bntOK
            // 
            this.bntOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bntOK.Location = new System.Drawing.Point(256, 496);
            this.bntOK.Name = "bntOK";
            this.bntOK.Size = new System.Drawing.Size(359, 91);
            this.bntOK.TabIndex = 26;
            this.bntOK.Text = "OK";
            this.bntOK.UseVisualStyleBackColor = true;
            this.bntOK.Click += new System.EventHandler(this.bntOK_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuit.Location = new System.Drawing.Point(650, 496);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(410, 91);
            this.btnQuit.TabIndex = 25;
            this.btnQuit.Text = "Quit MWL Server";
            this.btnQuit.UseVisualStyleBackColor = true;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // tbPasswordInput
            // 
            this.tbPasswordInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPasswordInput.Location = new System.Drawing.Point(256, 271);
            this.tbPasswordInput.Name = "tbPasswordInput";
            this.tbPasswordInput.PasswordChar = '*';
            this.tbPasswordInput.Size = new System.Drawing.Size(804, 53);
            this.tbPasswordInput.TabIndex = 21;
            // 
            // tbUserNameInput
            // 
            this.tbUserNameInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbUserNameInput.Location = new System.Drawing.Point(256, 163);
            this.tbUserNameInput.Name = "tbUserNameInput";
            this.tbUserNameInput.Size = new System.Drawing.Size(804, 53);
            this.tbUserNameInput.TabIndex = 22;
            // 
            // cbDatabase
            // 
            this.cbDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbDatabase.FormattingEnabled = true;
            this.cbDatabase.Location = new System.Drawing.Point(256, 378);
            this.cbDatabase.Name = "cbDatabase";
            this.cbDatabase.Size = new System.Drawing.Size(804, 54);
            this.cbDatabase.TabIndex = 24;
            // 
            // cbServer
            // 
            this.cbServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbServer.FormattingEnabled = true;
            this.cbServer.Location = new System.Drawing.Point(256, 56);
            this.cbServer.Name = "cbServer";
            this.cbServer.Size = new System.Drawing.Size(804, 54);
            this.cbServer.TabIndex = 23;
            // 
            // tbPassword
            // 
            this.tbPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPassword.Location = new System.Drawing.Point(0, 277);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.ReadOnly = true;
            this.tbPassword.Size = new System.Drawing.Size(284, 46);
            this.tbPassword.TabIndex = 13;
            this.tbPassword.TabStop = false;
            this.tbPassword.Text = "Password:";
            // 
            // tbDatabase
            // 
            this.tbDatabase.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbDatabase.Location = new System.Drawing.Point(4, 384);
            this.tbDatabase.Name = "tbDatabase";
            this.tbDatabase.ReadOnly = true;
            this.tbDatabase.Size = new System.Drawing.Size(280, 46);
            this.tbDatabase.TabIndex = 12;
            this.tbDatabase.TabStop = false;
            this.tbDatabase.Text = "Database:";
            // 
            // tbUsername
            // 
            this.tbUsername.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbUsername.Location = new System.Drawing.Point(4, 169);
            this.tbUsername.Name = "tbUsername";
            this.tbUsername.ReadOnly = true;
            this.tbUsername.Size = new System.Drawing.Size(280, 46);
            this.tbUsername.TabIndex = 11;
            this.tbUsername.TabStop = false;
            this.tbUsername.Text = "User Name:";
            // 
            // tbServer
            // 
            this.tbServer.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbServer.Location = new System.Drawing.Point(4, 62);
            this.tbServer.Name = "tbServer";
            this.tbServer.ReadOnly = true;
            this.tbServer.Size = new System.Drawing.Size(280, 46);
            this.tbServer.TabIndex = 10;
            this.tbServer.TabStop = false;
            this.tbServer.Text = "Server:";
            // 
            // PPLoginForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(13, 31);
            this.ClientSize = new System.Drawing.Size(1153, 752);
            this.Controls.Add(this.gbLogon);
            this.Controls.Add(this.tbPreamble);
            this.Name = "PPLoginForm";
            this.Text = "PowerPath Login";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.PPLoginForm_Closing);
            this.Load += new System.EventHandler(this.PPLoginForm_Load);
            this.gbLogon.ResumeLayout(false);
            this.gbLogon.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion


        #region  "Startup"
        private void PPLoginForm_Load(object sender, EventArgs e)
        {
        }
        #endregion

        #region "Shutdown"
        private void PPLoginForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //this.bwSCP.CancelAsync();
        }
        #endregion

        private void btnQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //REFACTOR: into reactive binding
        private void bntOK_Click(object sender, EventArgs e)
        {
//TODO: check whether we should test the serverconfig or the control text?
            // check if username is present
            if (this.tbUserNameInput.Text.Length < 1)
            {
//TODO: don't use messagebox!
                MessageBox.Show("Username must be entered!");
                return;
            }

            // check if password is present
            if (this.tbPasswordInput.Text.Length < 1)
            {
                MessageBox.Show("Password must be entered!");
                return;
            }

            if (this.serverconfig.ConnectionString.Length < 1)
            {
                Log.Error("PowerPath connection string is empty!");
                MessageBox.Show("PowerPath connection string is empty!");
                return;
            }
            Log.Info("Attempting connection to database.");
//TODO: Not Implemented!
            if ( isPowerPathConnected() )
            {

            }
            else
            {

            }
        }

        private bool isPowerPathConnected()
        {
            try
            {
                using (SqlConnection db = new SqlConnection(this.serverconfig.ConnectionString))
                {
                    db.Open();
//TODO: REFACTOR db can be opened, but without an initial catalog 
                    var databases = db.GetSchema("Databases");
                    if (databases?.Rows != null)
                    {
                        foreach (System.Data.DataRow row in databases.Rows)
                        {
                            this.serverconfig.AvailableDatabases.Add(row.Field<string>(@"database_name"));
                        }
                    }
                    /* If we reached here, that means the connection to the database was successful. */
                    return true;
                }
            }
            catch (SqlException se)
            {
//TODO: if server connected, but n=wrong database, then do something...
                 Log.Warn("Sql Error connecting to PowerPath with connectionstring: '"
                    + this.serverconfig.ConnectionString + " with error: " + se.Message);
                return false;
            }
            catch (Exception ex)
            {
                // We are here that means the connection failed!
                // You can handle the exception differently if you want to provide richer error handling.
                // At this moment we just return "false" which means the connection failed.
                Log.Error("Error connecting to PowerPath with connectionstring: '"
                    + this.serverconfig.ConnectionString + " with error: " + ex.Message);
                return false;
            }
        }
    }
}

