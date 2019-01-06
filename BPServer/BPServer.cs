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

        private SqlConnection db;
        public SqlConnection dbPP
        {
            get { return db; }
            set { db = value; }
        }
        private ServerConfiguration serverconfig;

        public ServerConfiguration ServerConfig
        {
            get { return serverconfig; }
            set { serverconfig = value; }
        }

//TODO: decide how to create and use the dbconnection to Powerpath
        public BPServer() { }   //MOCK: just to compile

        public BPServer(SqlConnection dbPP, ServerConfiguration serviceConfig)
        {
            this.dbPP = dbPP ?? throw new ArgumentNullException(nameof(dbPP));
            ServerConfig = serviceConfig ?? throw new ArgumentNullException(nameof(serviceConfig));
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
                    MWL_Server.Listen(ServerConfig.Portnumber, ServerConfig.IpAddress);
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
