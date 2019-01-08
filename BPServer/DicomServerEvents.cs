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

        #region "DICOM Event Handlers"
        private void DicomGlobal_LogEvent(DicomObjects.EventArguments.LogEventArgs e)
        {
            switch((DicomObjects.Enums.LogLevel)e.Level)
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

        internal static void AddResultItem(DicomDataSet DataSet, DicomDataSet request, Keyword keyword, object v)
        {
            // Only send items which have been requested
            if (request[keyword].Exists)
            {
                if (v == null)
                    v = "";
                DataSet.Add(keyword, v);
            }
        }

        private void MWL_Server_AssociationRequest(object sender, AssociationRequestArgs e)
        {
            Log.Info("Association Request from " + e.Association.CallingAET + " At " + e.Association.RemoteIP);
            // Reject any irrelevant Contexts			
            foreach (DicomContext context in e.Contexts)
            {
                if (context.AbstractSyntax != DicomObjects.DicomUIDs.SOPClasses.ModalityWorklistQRFIND
                    && context.AbstractSyntax != DicomObjects.DicomUIDs.SOPClasses.Verification)
                {
                    context.Reject(3); // 3- Abstract Syntax Not Supported
                }
            }
        }

        private void MWL_Server_VerifyReceived(object sender, VerifyReceivedArgs e)
        {
            e.Status = 0;
        }


        private void MWL_Server_QueryReceived(object sender, QueryReceivedArgs e)
        {
            #region "Abort if not an MWL query"
            if (e.Root != QueryRoot.ModalityWorklist)
            {
                e.Errors.Add(Keyword.ErrorComment, "Invalid Query Root : None");
                e.Status = StatusCodes.InvalidQueryRoot;
                return;
            }
            #endregion

            DicomDataSetCollection NullSequence = new DicomDataSetCollection();
            DicomDataSet rq;
            DicomDataSet rq1 = null;
            IEnumerable<ExamsScheduled> results = new List<ExamsScheduled>();
            int status = 0;
            System.Data.SqlClient.SqlConnection db = null;
            try
            {
                #region "Open PowerPath db"
                try
                {
                    db = new System.Data.SqlClient.SqlConnection(powerpathloginconfig.ConnectionString);
                    db.Open();
                }
                catch (SqlException se)
                {
                    Log.Error("Sql Error opening Database -- " + se.Message);
                    status = StatusCodes.GeneralError; // Error, unable to process!
                    return;
                }
                #endregion

                using (var DBContext = new DataContext(db))
                {
                    // Get the Imcoming Query Request
                    rq = e.RequestAssociation.Request;

                    #region "Build SQL query based on SCU request"
                    // the "where 1=1" makes the syntax of adding further conditions simpler, as all are then " AND x=y"
                    string sql = "SELECT * from ExamsScheduled Where 1=1";
                    Utils_SQL.AddCondition(ref sql, rq[Keyword.PatientID], "PatientID");
                    Utils_SQL.AddNameCondition(ref sql, rq[Keyword.PatientName], "surname", "Forename");

                    if (rq[Keyword.ScheduledProcedureStepSequence].ExistsWithValue)
                    {
                        DicomDataSetCollection rqs;

                        rqs = (DicomDataSetCollection)rq[Keyword.ScheduledProcedureStepSequence].Value;
                        rq1 = rqs[0];

                        // Required Matching keys
                        Utils_SQL.AddCondition(ref sql, rq1[Keyword.ScheduledStationAETitle], "ScheduledAET");
                        Utils_SQL.AddCondition(ref sql, rq1[Keyword.PerformingPhysicianName], "PerformingPhysician");
                        Utils_SQL.AddCondition(ref sql, rq1[Keyword.Modality], "Modality");

                        // if only date is specified, then using standard matching
                        //but if both are specified, then MWL defines a combined match
                        //TODO: decide whether we want to ever enable date matching
                        if (true)   //!DisableDateMatching.Checked)
                        {
                            if (rq1[Keyword.ScheduledProcedureStepStartDate].ExistsWithValue && rq1[Keyword.ScheduledProcedureStepStartTime].ExistsWithValue) // if both Date and Time are specified
                            {
                                Utils_SQL.AddDateTimeCondition(ref sql, rq1[Keyword.ScheduledProcedureStepStartDate], rq1[Keyword.ScheduledProcedureStepStartTime], "ExamDateAndTime");
                            }
                            else if (rq1[Keyword.ScheduledProcedureStepStartDate].ExistsWithValue) // if Date is specified
                            {
                                Utils_SQL.AddDateCondition(ref sql, rq1[Keyword.ScheduledProcedureStepStartDate], "ExamDateAndTime");
                            }
                        }

                        // Optional (but commonly used) matching keys.
                        Utils_SQL.AddCondition(ref sql, rq1[Keyword.ScheduledProcedureStepLocation], "ExamRoom");
                        Utils_SQL.AddCondition(ref sql, rq1[Keyword.ScheduledProcedureStepDescription], "ExamDescription");
                    }

                    sql = sql + " Order by Surname, Forename";
                    Log.Debug("SQL query statement:" + Environment.NewLine + sql);
                    #endregion

                    #region "Execute Sql Query"
                    try
                    {
                        results = DBContext.ExecuteQuery<ExamsScheduled>(sql);
                    }
                    catch(Exception ex)
                    {
                        Log.Error("Error executing SQL: '" + sql + "'. " + ex.Message);
                        status = StatusCodes.GeneralError;
                        return;
                    }
                    #endregion

                    #region "Parse results"
                    DicomDataSet rr1;
                    DicomDataSet rr;
                    DicomDataSetCollection rrs;
                    foreach (ExamsScheduled result in results)
                    {
                        rr1 = new DicomDataSet();
                        rr = new DicomDataSet();
                        rrs = new DicomDataSetCollection { rr1 };

                        if (rq1 != null)
                        {
                            rr.Add(Keyword.ScheduledProcedureStepSequence, rrs);
                        }

                        // add results to  "main" dataset

                        AddResultItem(rr, rq, Keyword.AccessionNumber, result.AccessionNumber);    // T2
                        AddResultItem(rr, rq, Keyword.InstitutionName, result.HospitalName);
                        AddResultItem(rr, rq, Keyword.ReferringPhysicianName, result.ReferringPhysician); // T2

                        AddResultItem(rr, rq, Keyword.PatientName, result.Surname + "^" + result.Forename + "^^" + result.Title); //T1
                        AddResultItem(rr, rq, Keyword.PatientID, result.PatientID); // T1
                        AddResultItem(rr, rq, Keyword.PatientBirthDate, result.DateOfBirth); // T2
                        AddResultItem(rr, rq, Keyword.PatientSex, result.Sex); //T2

                        AddResultItem(rr, rq, Keyword.StudyInstanceUID, result.StudyUID); // T1

                        AddResultItem(rr, rq, Keyword.RequestingPhysician, result.ReferringPhysician); //T2
                        AddResultItem(rr, rq, Keyword.RequestedProcedureDescription, result.ExamDescription); //T1C

                        AddResultItem(rr, rq, Keyword.RequestedProcedureID, result.ProcedureID); // T1

                        // Scheduled Procedure Step sequence T1
                        // add results to procedure step dataset
                        // Return if requested
                        if (rq1 != null)
                        {
                            AddResultItem(rr1, rq1, Keyword.ScheduledStationAETitle, result.ScheduledAET); // T1
                            AddResultItem(rr1, rq1, Keyword.ScheduledProcedureStepStartDate, result.ExamDateAndTime); //T1
                            AddResultItem(rr1, rq1, Keyword.ScheduledProcedureStepStartTime, result.ExamDateAndTime); //T1
                            AddResultItem(rr1, rq1, Keyword.Modality, result.Modality); // T1

                            AddResultItem(rr1, rq1, Keyword.ScheduledPerformingPhysicianName, result.PerformingPhysician); //T2
                            AddResultItem(rr1, rq1, Keyword.ScheduledProcedureStepDescription, result.ExamDescription); // T1C
                            AddResultItem(rr1, rq1, Keyword.ScheduledProcedureStepID, result.ProcedureStepID); // T1
                            AddResultItem(rr1, rq1, Keyword.ScheduledStationName, result.ExamRoom); //T2
                            AddResultItem(rr1, rq1, Keyword.ScheduledProcedureStepLocation, result.ExamRoom); //T2
                        }

                        // Put blanks in for unsupported fields which are type 2 (i.e. must have a value even if NULL)
                        // In a real server, you may wish to support some or all of these, but they are not commonly supported

                        AddResultItem(rr, rq, Keyword.ReferencedStudySequence, NullSequence);
                        AddResultItem(rr, rq, Keyword.Priority, "");
                        AddResultItem(rr, rq, Keyword.PatientTransportArrangements, "");
                        AddResultItem(rr, rq, Keyword.AdmissionID, "");
                        AddResultItem(rr, rq, Keyword.CurrentPatientLocation, "");
                        AddResultItem(rr, rq, Keyword.ReferencedPatientSequence, NullSequence);
                        AddResultItem(rr, rq, Keyword.PatientWeight, "");
                        AddResultItem(rr, rq, Keyword.ConfidentialityConstraintOnPatientDataDescription, "");
                        Log.Debug("Sending response ..");
                        // Send Reponse Back
                        e.SendResponse(rr, StatusCodes.Pending);
                        Log.Info("PowerPath Database Disconnected Normally");
                    }
                    #endregion
                }
                
            }
            catch (Exception ex)
            {
                Log.Error("Error processing MWL rewuest -- " + ex.Message);
                status = StatusCodes.GeneralError; // Error, unable to process!
            }
            finally
            {
                if (null != db)
                {
                    try
                    {
                        db.Close();
                    }
                    catch { /* ignore an SqlException here */ }
                    db.Dispose();
                }
                e.Status = status;
            }
        }
        #endregion
    }
}
