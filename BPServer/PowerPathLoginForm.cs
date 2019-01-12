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
    public partial class PPLoginForm : Form, IBindableComponent
    {
        private static readonly ILog Log
       = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private PowerPathLoginConfig serverlogin;
        public PowerPathLoginConfig ServerLogin
        {
            get
            {
                return serverlogin.Copy();
            }
        }

        public string ConnectionString
        {
            get { return serverlogin.ConnectionString; }
        }

        private IContainer components;
        LinesDataSource dsUserFeedback = new LinesDataSource();
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
        private TextBox tbFeedback;
        private SplitContainer splitContainerLogin;
        private ErrorProvider errorProviderPPForm;
        private TextBox tbUserNameInput;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Builder"></param>
        public PPLoginForm(PowerPathLoginConfig serverLogin)
        {
            this.serverlogin = serverLogin.Copy();
            InitializeComponent();
            BindControls();
        }

        private void BindControls()
        {
            // note: a List can serve as a datasource, but the individual builder object cannot
            this.cbServer.DataSource = serverlogin.ListServers;
            this.cbServer.DataBindings.Add(new Binding("Text", new List<PowerPathLoginConfig> { serverlogin }, "DataSource"));
            this.tbUserNameInput.DataBindings.Add(new Binding("Text", new List<PowerPathLoginConfig> { serverlogin }, "UserID"));
            this.tbPasswordInput.DataBindings.Add(new Binding("Text", new List<PowerPathLoginConfig> { serverlogin }, "Password"));
            this.cbServer.DataSource = serverlogin.ListDatabases;
            this.cbDatabase.DataBindings.Add(new Binding("Text", new List<PowerPathLoginConfig> { serverlogin }, "InitialCatalog"));
            this.tbFeedback.DataBindings.Add("Lines", dsUserFeedback, "LinesArray");
        }

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
            this.tbFeedback = new System.Windows.Forms.TextBox();
            this.splitContainerLogin = new System.Windows.Forms.SplitContainer();
            this.errorProviderPPForm = new System.Windows.Forms.ErrorProvider(this.components);
            this.gbLogon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLogin)).BeginInit();
            this.splitContainerLogin.Panel1.SuspendLayout();
            this.splitContainerLogin.Panel2.SuspendLayout();
            this.splitContainerLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderPPForm)).BeginInit();
            this.SuspendLayout();
            // 
            // tbPreamble
            // 
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
            this.gbLogon.Size = new System.Drawing.Size(541, 295);
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
            this.bntOK.Click += new System.EventHandler(this.bntOK_Click);
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
            // tbFeedback
            // 
            this.tbFeedback.CausesValidation = false;
            this.tbFeedback.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbFeedback.Location = new System.Drawing.Point(0, 0);
            this.tbFeedback.Multiline = true;
            this.tbFeedback.Name = "tbFeedback";
            this.tbFeedback.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbFeedback.Size = new System.Drawing.Size(541, 119);
            this.tbFeedback.TabIndex = 19;
            this.tbFeedback.TabStop = false;
            // 
            // splitContainerLogin
            // 
            this.splitContainerLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerLogin.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerLogin.Location = new System.Drawing.Point(0, 0);
            this.splitContainerLogin.Name = "splitContainerLogin";
            this.splitContainerLogin.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerLogin.Panel1
            // 
            this.splitContainerLogin.Panel1.Controls.Add(this.gbLogon);
            // 
            // splitContainerLogin.Panel2
            // 
            this.splitContainerLogin.Panel2.Controls.Add(this.tbFeedback);
            this.splitContainerLogin.Size = new System.Drawing.Size(541, 418);
            this.splitContainerLogin.SplitterDistance = 295;
            this.splitContainerLogin.TabIndex = 18;
            // 
            // errorProviderPPForm
            // 
            this.errorProviderPPForm.ContainerControl = this;
            // 
            // PPLoginForm
            // 
            this.AcceptButton = this.bntOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(541, 418);
            this.Controls.Add(this.splitContainerLogin);
            this.Name = "PPLoginForm";
            this.Text = "PowerPath Login";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.PPLoginForm_Closing);
            this.Load += new System.EventHandler(this.PPLoginForm_Load);
            this.gbLogon.ResumeLayout(false);
            this.gbLogon.PerformLayout();
            this.splitContainerLogin.Panel1.ResumeLayout(false);
            this.splitContainerLogin.Panel2.ResumeLayout(false);
            this.splitContainerLogin.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLogin)).EndInit();
            this.splitContainerLogin.ResumeLayout(false);
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

        private void bntOK_Click(object sender, EventArgs e)
        {
            hasValidationError = false;
            bool validated = this.ValidateChildren(ValidationConstraints.TabStop);
            if (true == hasValidationError)
            {
                hasValidationError = false;
                return;
            }
            string strFeedbackFromTestPowerPathConnect = FeedbackFromTestDatabaseConnect(serverlogin);
            if (strFeedbackFromTestPowerPathConnect.Length > 0)
            {
                Log.Trace(@"Attempt to Database-connect: " + strFeedbackFromTestPowerPathConnect);
                this.dsUserFeedback.Lines.Add(strFeedbackFromTestPowerPathConnect);
                serverlogin.ValidDbConnection = false;
            }
            else // sucessfully connected to db!
            {
                serverlogin.ValidDbConnection = true;
                Log.Trace(@"Successfully connected to PowerPath!");
                this.dsUserFeedback.Lines.Add(@"Successfully connected to PowerPath!");
                this.Close();
            }
        }

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

        // https://stackoverflow.com/questions/23321567/how-to-bind-textbox-lines-to-bindingliststring-in-winforms-in-c
        private class LinesDataSource : INotifyPropertyChanged
        {
            private BindingList<string> lines = new BindingList<string>();
            public LinesDataSource()
            {
                lines.ListChanged += (sender, e) => OnPropertyChanged("LinesArray");
            }
            public BindingList<string> Lines
            {
                get { return lines; }
            }
            public string[] LinesArray
            {
                get
                {
                    return lines.ToArray();
                }
            }
            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string FeedbackFromTestDatabaseConnect(PowerPathLoginConfig serverlogin)
        {
            // ping first, as this is quicker
            //CONSIDER: refactor ping and Sql-db-connect into two async methods and wait for whomever comes first.
            if (false == PingHost(serverlogin.DataSource))
            {
                return "Server not found at: '" + serverlogin.DataSource + @"'!";
            }
            else
            {
                Log.Trace(@"Ping succeeded to " + serverlogin.DataSource);
            }
            try
            {
                using (SqlConnection db = new SqlConnection(serverlogin.ConnectionString))
                {
                    db.Open();
                    Log.Trace(@"Successful test-connect to PowerPath database: " + db.Database);
                    /* If we reached here, that means the connection to the database was successful. */
                    return "";
                }
            }
            catch (SqlException se)
            {
                // TODO: if server connected, but n = wrong database, then do something...
                switch (se.Number)
                {
                    case 53:    // server unavailble
                                //TODO: ping test?
                        break;
                    default:
                        break;
                }
                string strSqlException = "Sql Error connecting to PowerPath"
                    + " with connectionstring: '" + serverlogin.ConnectionString
                    + " with error: " + se.Message;
                //Log.Warn(strSqlException);
                return strSqlException;
            }
            catch (Exception ex)
            {
                string strException = "Error connecting to PowerPath"
                    + " with connectionstring: '" + serverlogin.ConnectionString
                    + " with error: " + ex.Message;
                //Log.Error(strException);
                return strException;
            }
        }
        // addapted from https://stackoverflow.com/questions/11800958/using-ping-in-c-sharp
        public static bool PingHost(string nameOrAddress)
        {
            using (System.Net.NetworkInformation.Ping pinger = new System.Net.NetworkInformation.Ping())
            {
                System.Net.NetworkInformation.PingReply reply = pinger.Send(nameOrAddress);
                return (reply.Status == System.Net.NetworkInformation.IPStatus.Success);


            }
        }

    }
}

