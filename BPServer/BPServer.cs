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

namespace BiopticPowerPathDicomServer
{
    public partial class BPServer : IDisposable
    {
        private static ILog Log = LogManager.GetLogger("BPServer");

        private DicomServerConfiguration serverconfig;
        private PowerPathDbConnect powerpathdbconnect;


        //TODO: decide how to create and use the dbconnection to Powerpath
        public BPServer() { }   //MOCK: just to compile

        public BPServer(DicomServerConfiguration serverConfig, PowerPathDbConnect powerpathDbConnect)
        {
            this.serverconfig = serverConfig ?? throw new ArgumentNullException(nameof(serverConfig));
            powerpathdbconnect = powerpathDbConnect ?? throw new ArgumentNullException(nameof(powerpathdbconnect));
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
