using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DicomObjects;
using DicomObjects.Enums;
using Common.Logging;
using System.Reflection;
using System.Windows.Forms;

namespace BiopticPowerPathDicomServer
{
    public partial class BPServer
    {
        private static ILog Log = LogManager.GetLogger("BPServer");

        private DicomServerConfiguration dicomserverconfig;
        private PowerPathLoginConfig powerpathloginconfig;
        private PPLoginForm pploginform;

        private DicomServer MWL_Server = null;

        private ApplicationContext appcontext;

        public ApplicationContext appContext
        {
            get { return appcontext; }
            set { appcontext = value; }
        }

        ~BPServer()
        {
            if(null != MWL_Server)
            {
                MWL_Server.UnlistenAll();
                MWL_Server.Dispose();
                MWL_Server = null;
            }
            if(null != pploginform && !pploginform.IsDisposed)
            {
                pploginform.Dispose();
                pploginform = null;
            }
        }

        #region "Constructors"
        public BPServer(ApplicationContext AppContext)
        {
            this.appcontext = AppContext;
            dicomserverconfig = new DicomServerConfiguration();
            powerpathloginconfig = new PowerPathLoginConfig();
            pploginform = new PPLoginForm(powerpathloginconfig);
            pploginform.Closing += new System.ComponentModel.CancelEventHandler(this.pploginform_Closing);
            //note: Application.Run(pploginform) terminates when the form disposes
            pploginform.Show();
            Application.Run(AppContext);
        }
        #endregion

        /// <summary>
        /// Start the BPServer AFTER the PowerPath Db Form closes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pploginform_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            PPLoginForm ppform = (PPLoginForm)sender;
            powerpathloginconfig = ppform.ServerLogin;
            if(false == powerpathloginconfig.ValidDbConnection)
            {
                MessageBox.Show ("Failed to get a valid connection to PowerPath.\r\nModality Worklist (MWL) is unavailable!");
                
                Application.Exit();
                return;
            }
            else
            {
                //drop messaging from form thread?
                //this.appContext.ExitThread();
            }

            this.RunServer();
        }

        public void RunServer()
        {
            try
            {
                MWL_Server = new DicomServer();
            }
            catch(Exception ex)
            {
                Log.Fatal("Failed to create MWL_Server!" + ex.Message);
                return;
            }

            try
            {
                MWL_Server.VerifyReceived += MWL_Server_VerifyReceived;
                MWL_Server.AssociationRequest += MWL_Server_AssociationRequest;
                MWL_Server.QueryReceived += MWL_Server_QueryReceived;
            }
            catch (Exception ex)
            {
                Log.Fatal("Failed to connect DicomServer events!" + ex.Message);
            }

            try
            {
                MWL_Server.Listen(dicomserverconfig.Portnumber, dicomserverconfig.IpAddress);
            }
            catch (Exception ex)
            {
                Log.Error("Failed to set listening port at portnumber:" + dicomserverconfig.Portnumber 
                    + " on interface:" + dicomserverconfig.IpAddress + ". " + ex.Message);
            }

            //DicomObjects.DicomGlobal.LogEvent
            //Put the following line back in to enable DicomObjects logging
            DicomObjects.DicomGlobal.LogToFile("dicom_log_files", 0x63);

            // define the messages sent from the DicomSever to the Logger
            //  L_ERROR = 1,
            //  L_WARN = 2,
            //   Miscelaneous informational messages
                // L_LOG = 4,
            //  Detailed informational messages
                // L_DETAILED = 8,
            //  Interpetted version of incoming attributes (file and network)
                // L_ELEMENTSIN = 16,
            //  Interpetted version of outgoing attributes (file and network)
                // L_ELEMENTSOUT = 32,
            DicomGlobal.EventLogLevel = (DicomObjects.Enums.LogLevel)0x3F; //dec 63
            DicomGlobal.LogEvent += DicomGlobal_LogEvent;
            Log.Trace("MWL_Server started!");
         }
    }
}
