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
using System.Threading.Tasks;


namespace BiopticPowerPathDicomServer
{
    public partial class PPLoginForm : Form, IViewFor<PPLoginViewModel>
    {
        private static ILog Log = LogManager.GetLogger("PPLoginForm");

        private SqlConnectionStringBuilder builder;

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
        private TextBox tbFeedback;
        private SplitContainer splitContainerLogin;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Builder"></param>
        public PPLoginForm(SqlConnectionStringBuilder Builder)
        {
            builder = new SqlConnectionStringBuilder(Builder.ConnectionString);
            VM = new PPLoginViewModel();
            VM.Builder = builder;
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
//TODO: refactor this into form control validation
        private string FeedbackFromCheckConnectionBuilder()
        {
            if (this.cbServer.Text.Length < 1)
            {
                return @"Please enter the database server name or IP address!";
            }
            if (this.tbUserNameInput.Text.Length < 1)
            {
                return @"Username must be entered!";
            }
            if (this.tbPasswordInput.Text.Length < 1)
            {
                return @"Password must be entered!";
            }
            return "";
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
            this.tbFeedback = new System.Windows.Forms.TextBox();
            this.splitContainerLogin = new System.Windows.Forms.SplitContainer();
            this.gbLogon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLogin)).BeginInit();
            this.splitContainerLogin.Panel1.SuspendLayout();
            this.splitContainerLogin.Panel2.SuspendLayout();
            this.splitContainerLogin.SuspendLayout();
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
            // 
            // tbUserNameInput
            // 
            this.tbUserNameInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbUserNameInput.Location = new System.Drawing.Point(119, 86);
            this.tbUserNameInput.Name = "tbUserNameInput";
            this.tbUserNameInput.Size = new System.Drawing.Size(371, 30);
            this.tbUserNameInput.TabIndex = 22;
            // 
            // cbDatabase
            // 
            this.cbDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbDatabase.FormattingEnabled = true;
            this.cbDatabase.Location = new System.Drawing.Point(119, 190);
            this.cbDatabase.Name = "cbDatabase";
            this.cbDatabase.Size = new System.Drawing.Size(371, 33);
            this.cbDatabase.TabIndex = 24;
            // 
            // cbServer
            // 
            this.cbServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbServer.FormattingEnabled = true;
            this.cbServer.Location = new System.Drawing.Point(119, 34);
            this.cbServer.Name = "cbServer";
            this.cbServer.Size = new System.Drawing.Size(371, 33);
            this.cbServer.TabIndex = 23;
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
            // PPLoginForm
            // 
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

        //REFACTOR: into reactive binding
        private void bntOK_Click(object sender, EventArgs e)
        {
            string strTestConnectionValues = FeedbackFromCheckConnectionBuilder();
            if (strTestConnectionValues.Length > 0)
            {
                Log.Info(strTestConnectionValues);
                this.tbFeedback.Text = strTestConnectionValues;
                return;
            }
            else
            {
                Log.Info(@"Attempting connection to database.");
                this.tbFeedback.Text = @"Attempting connection to database.";
            }
            //DEBUG!            Task<string> taskFeedbackFromTestPowerPathConnect = PowerPathDbConnect.FeedbackFromTestPowerPathConnect(builder.ConnectionString);
            //            taskFeedbackFromTestPowerPathConnect.Wait();
            //           string strFeedbackFromTestPowerPathConnect = taskFeedbackFromTestPowerPathConnect.Result.ToString();
            string strFeedbackFromTestPowerPathConnect = ConnectionHelpers.FeedbackFromTestDatabaseConnect(builder);
            if (strFeedbackFromTestPowerPathConnect.Length > 0)
            {
                this.tbFeedback.Text = strFeedbackFromTestPowerPathConnect;
            }
        }
    }
}

