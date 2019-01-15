using System;
using System.Windows.Forms;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Data.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.ComponentModel;
using Common.Logging;
using Microsoft.Win32;

namespace BiopticPowerPathDicomServer
{
    // View
    public partial class PPLoginFormView : Form, IBindableComponent
    {
        private static readonly ILog Log
       = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public PPLoginFormView()
        {
            PowerPathConfigurationPresenter presenter = new PowerPathConfigurationPresenter(this);
            InitializeComponent();
            //TODO: disconnect existing OnLoad event
            this.Load += (s, e) => OnViewLoad(); // event delegate shorthand notation
            this.bntOK.Click += (s, e) => OnValidateDbConnection();
            this.btnQuit.Click += (s, e) => OnQuit();
        }

        public PPLoginFormView(PowerPathConfigurationViewModel serverLogin)
        {
            PowerPathConfigurationPresenter presenter = new PowerPathConfigurationPresenter(this);
            this.serverlogin = serverLogin.CopyPowerPathConfiguration();
            InitializeComponent();
//this.Load += (s, e) => OnViewLoadConfig(serverLogin);
            this.bntOK.Click += (s, e) => OnValidateDbConnection();
            this.btnQuit.Click += (s, e) => OnQuit();
        }

        public event Action OnViewLoad;
//public event Action OnViewLoadConfig(PowerPathConfigurationMVP);
        public event Action OnValidateDbConnection;
        public event Action OnQuit;

        public PowerPathConfigurationViewModel serverlogin
        {
            set
            {
                // note: a List can serve as a datasource, but the individual builder object cannot
                this.cbServer.DataBindings.Add(new Binding("Items", value, "ListServers", true, DataSourceUpdateMode.OnPropertyChanged, string.Empty));
                //this.cbServer.DataSource = serverlogin.ListServers;
                this.cbServer.DataBindings.Add(new Binding("Text", value, "DataSource", true, DataSourceUpdateMode.OnPropertyChanged, string.Empty));
                this.tbUserNameInput.DataBindings.Add(new Binding("Text", value, "UserID", true, DataSourceUpdateMode.OnPropertyChanged, string.Empty));
                this.tbPasswordInput.DataBindings.Add(new Binding("Text", value, "Password", true, DataSourceUpdateMode.OnPropertyChanged, string.Empty));
                this.cbServer.DataBindings.Add(new Binding("Items", value, "ListDatabases", true, DataSourceUpdateMode.OnPropertyChanged, string.Empty));
                //this.cbServer.DataSource = serverlogin.ListDatabases;
                this.cbDatabase.DataBindings.Add(new Binding("Text", value, "InitialCatalog", true, DataSourceUpdateMode.OnPropertyChanged, string.Empty));
            }
        }
        private IContainer components;
        private bool hasValidationError = false;

        #region "Component variables (leave alone!)"
        internal TextBox tbPreamble;
        private GroupBox gbLogon;
        internal TextBox tbPassword;
        internal TextBox tbDatabase;
        internal TextBox tbUsername;
        internal TextBox tbServer;
        private ComboBox cbDatabase;
        private ComboBox cbServer;
        private TextBox tbPasswordInput;
        public Button bntOK;
        private Button btnQuit;
        private ErrorProvider errorProviderPPForm;
        private TextBox tbUserNameInput;

        #endregion

        #region "Disposal"
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
        #endregion

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
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
            this.errorProviderPPForm = new System.Windows.Forms.ErrorProvider(this.components);
            this.gbLogon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderPPForm)).BeginInit();
            this.SuspendLayout();
            // 
            // tbPreamble
            // 
            this.tbPreamble.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbPreamble.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbPreamble.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPreamble.Location = new System.Drawing.Point(4, 0);
            this.tbPreamble.Multiline = true;
            this.tbPreamble.Name = "tbPreamble";
            this.tbPreamble.ReadOnly = true;
            this.tbPreamble.Size = new System.Drawing.Size(368, 32);
            this.tbPreamble.TabIndex = 9;
            this.tbPreamble.TabStop = false;
            this.tbPreamble.Text = "Enter your Powerpath username and password.";
            // 
            // gbLogon
            // 
            this.gbLogon.Controls.Add(this.bntOK);
            this.gbLogon.Controls.Add(this.tbPreamble);
            this.gbLogon.Controls.Add(this.btnQuit);
            this.gbLogon.Controls.Add(this.tbPasswordInput);
            this.gbLogon.Controls.Add(this.tbUserNameInput);
            this.gbLogon.Controls.Add(this.cbDatabase);
            this.gbLogon.Controls.Add(this.cbServer);
            this.gbLogon.Controls.Add(this.tbPassword);
            this.gbLogon.Controls.Add(this.tbDatabase);
            this.gbLogon.Controls.Add(this.tbUsername);
            this.gbLogon.Controls.Add(this.tbServer);
            this.gbLogon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbLogon.Location = new System.Drawing.Point(0, 0);
            this.gbLogon.Name = "gbLogon";
            this.gbLogon.Size = new System.Drawing.Size(541, 296);
            this.gbLogon.TabIndex = 17;
            this.gbLogon.TabStop = false;
            // 
            // bntOK
            // 
            this.bntOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bntOK.Location = new System.Drawing.Point(119, 244);
            this.bntOK.Name = "bntOK";
            this.bntOK.Size = new System.Drawing.Size(166, 44);
            this.bntOK.TabIndex = 26;
            this.bntOK.Text = "OK";
            this.bntOK.UseVisualStyleBackColor = true;
            // 
            // btnQuit
            // 
            this.btnQuit.CausesValidation = false;
            this.btnQuit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuit.Location = new System.Drawing.Point(301, 244);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(189, 44);
            this.btnQuit.TabIndex = 25;
            this.btnQuit.Text = "Quit MWL Server";
            this.btnQuit.UseVisualStyleBackColor = true;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // tbPasswordInput
            // 
            this.tbPasswordInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPasswordInput.Location = new System.Drawing.Point(119, 138);
            this.tbPasswordInput.Name = "tbPasswordInput";
            this.tbPasswordInput.PasswordChar = '*';
            this.tbPasswordInput.Size = new System.Drawing.Size(371, 30);
            this.tbPasswordInput.TabIndex = 21;
            this.tbPasswordInput.Validating += new System.ComponentModel.CancelEventHandler(this.tbPasswordInput_Validating);
            // 
            // tbUserNameInput
            // 
            this.tbUserNameInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbUserNameInput.Location = new System.Drawing.Point(119, 86);
            this.tbUserNameInput.Name = "tbUserNameInput";
            this.tbUserNameInput.Size = new System.Drawing.Size(371, 30);
            this.tbUserNameInput.TabIndex = 22;
            this.tbUserNameInput.Validating += new System.ComponentModel.CancelEventHandler(this.tbUserNameInput_Validating);
            // 
            // cbDatabase
            // 
            this.cbDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbDatabase.FormattingEnabled = true;
            this.cbDatabase.Location = new System.Drawing.Point(119, 190);
            this.cbDatabase.Name = "cbDatabase";
            this.cbDatabase.Size = new System.Drawing.Size(371, 33);
            this.cbDatabase.TabIndex = 24;
            this.cbDatabase.Validating += new System.ComponentModel.CancelEventHandler(this.cbDatabase_Validating);
            // 
            // cbServer
            // 
            this.cbServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbServer.FormattingEnabled = true;
            this.cbServer.Location = new System.Drawing.Point(119, 34);
            this.cbServer.Name = "cbServer";
            this.cbServer.Size = new System.Drawing.Size(371, 33);
            this.cbServer.TabIndex = 23;
            this.cbServer.Validating += new System.ComponentModel.CancelEventHandler(this.cbServer_Validating);
            // 
            // tbPassword
            // 
            this.tbPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPassword.Location = new System.Drawing.Point(1, 141);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.ReadOnly = true;
            this.tbPassword.Size = new System.Drawing.Size(131, 23);
            this.tbPassword.TabIndex = 13;
            this.tbPassword.TabStop = false;
            this.tbPassword.Text = "Password:";
            // 
            // tbDatabase
            // 
            this.tbDatabase.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbDatabase.Location = new System.Drawing.Point(3, 193);
            this.tbDatabase.Name = "tbDatabase";
            this.tbDatabase.ReadOnly = true;
            this.tbDatabase.Size = new System.Drawing.Size(129, 23);
            this.tbDatabase.TabIndex = 12;
            this.tbDatabase.TabStop = false;
            this.tbDatabase.Text = "Database:";
            // 
            // tbUsername
            // 
            this.tbUsername.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbUsername.Location = new System.Drawing.Point(3, 89);
            this.tbUsername.Name = "tbUsername";
            this.tbUsername.ReadOnly = true;
            this.tbUsername.Size = new System.Drawing.Size(129, 23);
            this.tbUsername.TabIndex = 11;
            this.tbUsername.TabStop = false;
            this.tbUsername.Text = "User Name:";
            // 
            // tbServer
            // 
            this.tbServer.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbServer.Location = new System.Drawing.Point(3, 37);
            this.tbServer.Name = "tbServer";
            this.tbServer.ReadOnly = true;
            this.tbServer.Size = new System.Drawing.Size(129, 23);
            this.tbServer.TabIndex = 10;
            this.tbServer.TabStop = false;
            this.tbServer.Text = "Server:";
            // 
            // errorProviderPPForm
            // 
            this.errorProviderPPForm.ContainerControl = this;
            // 
            // PPLoginFormView
            // 
            this.AcceptButton = this.bntOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(541, 296);
            this.Controls.Add(this.gbLogon);
            this.Name = "PPLoginFormView";
            this.Text = "PowerPath Login";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.PPLoginForm_Closing);
            this.Load += new System.EventHandler(this.PPLoginForm_Load);
            this.gbLogon.ResumeLayout(false);
            this.gbLogon.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderPPForm)).EndInit();
            this.ResumeLayout(false);

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

        //TODO: move these methods to Model
        #region "Move to Model"
        private void cbServer_Validating(object sender, CancelEventArgs e)
        {
            ComboBox cb =(ComboBox)sender;
            if (cb.Text.Length > 1)
            {
                errorProviderPPForm.SetError(cb, "");
            }
            else
            {
                hasValidationError = true;
                errorProviderPPForm.SetError(cb, "A Server name or IP address must be provided!");
            }
         }
        private void tbUserNameInput_Validating(object sender, CancelEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text.Length > 1)
            {
                errorProviderPPForm.SetError(tb, "");
            }
            else
            {
                hasValidationError = true;
                errorProviderPPForm.SetError(tb, "A valid User name must be provided!");
            }
        }
        private void tbPasswordInput_Validating(object sender, CancelEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text.Length > 1)
            {
                errorProviderPPForm.SetError(tb, "");
            }
            else
            {
                hasValidationError = true;
                errorProviderPPForm.SetError(tb, "A valid Password must be provided!");
            }
        }
        private void cbDatabase_Validating(object sender, CancelEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (cb.Text.Length > 1)
            {
                errorProviderPPForm.SetError(cb, "");
            }
            else
            {
                hasValidationError = true;
                errorProviderPPForm.SetError(cb, "A PowerPath database must be provided!");
            }
        }
        #endregion
    }
}
