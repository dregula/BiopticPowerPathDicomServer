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
    public partial class BPServer : IDisposable
    {
        private static ILog Log = LogManager.GetLogger("BPServer");

        private DicomServerConfiguration serverconfig;
        private ServerLogin serverlogin;
        private PPLoginForm pploginform;

        //TODO: decide how to create and use the dbconnection to Powerpath
        public BPServer(ApplicationContext appContext)
        {
            serverlogin = ConnectionHelpers.ServerLoginFromPowerPathRegistry();
            pploginform = new PPLoginForm(serverlogin);
            //pploginform.bntOK.Click += new System.EventHandler(this.pploginform_OK);
            pploginform.Closing += new System.ComponentModel.CancelEventHandler(this.pploginform_Closing);
            pploginform.Show();
            Application.Run(appContext);

        }
        public BPServer()
        {
            serverlogin = ConnectionHelpers.ServerLoginFromPowerPathRegistry();
            pploginform = new PPLoginForm(serverlogin);
            //pploginform.bntOK.Click += new System.EventHandler(this.pploginform_OK);
            pploginform.Closing += new System.ComponentModel.CancelEventHandler(this.pploginform_Closing);
            pploginform.Show();
        }   //MOCK: just to compile

        public BPServer(DicomServerConfiguration serverConfig, ServerLogin serverlogin)
        {
            this.serverconfig = serverConfig ?? throw new ArgumentNullException(nameof(serverConfig));
            serverlogin = serverlogin ?? throw new ArgumentNullException(nameof(serverlogin));
        }

        //TODO: only respond when the DB connection has been verified
        //private void pploginform_OK(object sender, EventArgs e)
        //{
        //    Button bnOK = (Button)sender;
        //    PPLoginForm ppform = (PPLoginForm)bnOK.FindForm();
        //    serverlogin = new ServerLogin(ppform.ConnectionString);
        //}
        private void pploginform_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            PPLoginForm ppform = (PPLoginForm)sender;
            serverlogin = ppform.ServerLogin;
            if(false == serverlogin.ValidDbConnection)
            {
                MessageBox.Show ("Failed to get a valid connection to PowerPath.\r\nModality Worklist (MWL) is unavailable!");
                Application.Exit();
            }
            this.RunServer();
        }

        /// <summary>
        /// IDisposable
        /// </summary>
        void IDisposable.Dispose()
        {
//TODO: expand to include raise form
        }


        public void RunServer()
        {
            using (DicomServer MWL_Server = new DicomServer())
            {
                MWL_Server.VerifyReceived += MWL_Server_VerifyReceived;
                MWL_Server.AssociationRequest += MWL_Server_AssociationRequest;
                MWL_Server.QueryReceived += MWL_Server_QueryReceived;
                try
                {
                    MWL_Server.Listen(serverconfig.Portnumber, serverconfig.IpAddress);
                }
                catch (Exception ex)
                {
                    Log.Error("Failed to set listening port: " + ex.Message);
                    throw;
                }

                //Put the following line back in to enable DicomObjects logging
                DicomObjects.DicomGlobal.LogToFile("dicom_log_files", 0x63);

                DicomGlobal.EventLogLevel = (DicomObjects.Enums.LogLevel)0x3F;
                DicomGlobal.LogEvent += DicomGlobal_LogEvent;
            }
         }
    }
}
