using System;
using System.Collections.Generic;
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
        private void MWL_Server_AssociationRequest(object sender, AssociationRequestArgs e)
        {
            Log.Info("Association Request from " + e.Association.CallingAET + " At " + e.Association.RemoteIP);
            Log.Trace("Called AET: " + e.Association.CalledAET + ", we are listed as: " + this.dicomserverconfig.AETitle);

            #region "Reject unwanted Contexts"		
            foreach (DicomContext context in e.Contexts)
            {
                if (context.AbstractSyntax != DicomObjects.DicomUIDs.SOPClasses.ModalityWorklistQRFIND
                    && context.AbstractSyntax != DicomObjects.DicomUIDs.SOPClasses.Verification)
                {
                    Log.Trace("Association Request context rejected: " + context.ToString());
                    context.Reject(3); // 3- Abstract Syntax Not Supported
                }
                Log.Trace("Association Request context accepted: " + context.ToString());
            }
            #endregion
        }
    }
}
