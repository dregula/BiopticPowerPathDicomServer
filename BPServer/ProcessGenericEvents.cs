using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using DicomObjects;
using DicomObjects.Enums;
using DicomObjects.EventArguments;

namespace BiopticPowerPathDicomServer
{
    public partial class BPServer
    {
        internal static class StatusCodes
        {
            public const int Success = 0x0;
            public const int GeneralError = 0xC000;
            public const int Pending = 0xFF00;
            public const int InvalidQueryRoot = 0xC001;
        }

        #region "Process C-ECHO"
        private void MWL_Server_VerifyReceived(object sender, VerifyReceivedArgs e)
        {
            e.Status = 0;
        }
        #endregion

        #region "Process internal Log Event"
        private void DicomGlobal_LogEvent(DicomObjects.EventArguments.LogEventArgs e)
        {
            switch ((DicomObjects.Enums.LogLevel)e.Level)
            {
                case DicomObjects.Enums.LogLevel.L_ERROR:
                    Log.Error(e.Text);
                    break;
                case DicomObjects.Enums.LogLevel.L_WARN:
                    Log.Warn(e.Text);
                    break;
                case DicomObjects.Enums.LogLevel.L_DEBUG:
                    Log.Debug(e.Text);
                    break;
                default:
                    Log.Debug(e.Text);
                    break;
            }
        }
        #endregion
    }
}
