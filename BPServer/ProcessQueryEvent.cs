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

        #region "DICOM Event Handler: Query"
        private void MWL_Server_QueryReceived(object sender, QueryReceivedArgs e)
        {
            #region "Tracing all calls to Server"
            Log.Trace("MWL_Server_QueryReceived with QueryRoot: " + e.Root.ToString());
            #endregion

            #region "Abort if not an MWL query"
            if (e.Root != QueryRoot.ModalityWorklist)
            {
                e.Errors.Add(Keyword.ErrorComment, "Invalid Query Root : None");
                e.Status = StatusCodes.InvalidQueryRoot;
                return;
            }
            #endregion

            DicomDataSetCollection NullSequence = new DicomDataSetCollection();
            DicomDataSet requestFromAssociation;
            DicomDataSet requestedProcedureStep = null;
            IEnumerable<ExamsScheduledTable> results = new List<ExamsScheduledTable>();
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

                using (var DBContext = new System.Data.Linq.DataContext(db))
                {
                    // Get the Imcoming Query Request
                    requestFromAssociation = e.RequestAssociation.Request;

                    #region "Build SQL query based on SCU request"
                    // the "where 1=1" makes the syntax of adding further conditions simpler, as all are then " AND x=y"
                    //TODO: refactor hard-coded table into Powerpath configuration item
                    string tableWithExamsScheduled = @"vwsu_scheduled_speciman_xrays";
                    if (this.dicomserverconfig.ExamScheduledTable.Length > 4)
                    {
                        tableWithExamsScheduled = dicomserverconfig.ExamScheduledTable;
                    }
                    string sql = "SELECT * from " + tableWithExamsScheduled + " Where 1=1";
                    SqlQueryUtils.AddCondition(ref sql, requestFromAssociation[Keyword.PatientID], "PatientID");
                    SqlQueryUtils.AddNameCondition(ref sql, requestFromAssociation[Keyword.PatientName], "surname", "Forename");

                    if (requestFromAssociation[Keyword.ScheduledProcedureStepSequence].ExistsWithValue)
                    {
                        DicomDataSetCollection requestedProcedureSteps;
                        //TODO: in what circumstances would there be more than one requested step?
                        requestedProcedureSteps = (DicomDataSetCollection)requestFromAssociation[Keyword.ScheduledProcedureStepSequence].Value;
                        requestedProcedureStep = requestedProcedureSteps[0];

                        // Required Matching keys
                        SqlQueryUtils.AddCondition(ref sql, requestedProcedureStep[Keyword.ScheduledStationAETitle], "ScheduledAET");
                        // always blank from PowerPath //SqlQueryUtils.AddCondition(ref sql, rq1[Keyword.PerformingPhysicianName], "PerformingPhysician");
                        SqlQueryUtils.AddCondition(ref sql, requestedProcedureStep[Keyword.Modality], "Modality");


                        // AddDateCondition looks for a leading or trailing -hyphen- 

                        // if only date is specified, then using standard matching
                        //but if both are specified, then MWL defines a combined match
                        //note: for PowerPath, there is no Scheduled Time that makes any sense
                        if (requestedProcedureStep[Keyword.ScheduledProcedureStepStartDate].ExistsWithValue && requestedProcedureStep[Keyword.ScheduledProcedureStepStartTime].ExistsWithValue) // if both Date and Time are specified
                        {
                            SqlQueryUtils.AddDateTimeCondition(ref sql, requestedProcedureStep[Keyword.ScheduledProcedureStepStartDate], requestedProcedureStep[Keyword.ScheduledProcedureStepStartTime], "ExamDateAndTime");
                        }
                        else if (requestedProcedureStep[Keyword.ScheduledProcedureStepStartDate].ExistsWithValue) // if Date is specified
                        {
                            SqlQueryUtils.AddDateCondition(ref sql, requestedProcedureStep[Keyword.ScheduledProcedureStepStartDate], "ExamDateAndTime");
                        }

                        // Optional (but commonly used) matching keys.
                        SqlQueryUtils.AddCondition(ref sql, requestedProcedureStep[Keyword.ScheduledProcedureStepLocation], "ExamRoom");
                        SqlQueryUtils.AddCondition(ref sql, requestedProcedureStep[Keyword.ScheduledProcedureStepDescription], "ExamDescription");
                    }

                    sql = sql + " Order by Surname, Forename";
                    Log.Debug("SQL query statement:" + Environment.NewLine + sql);
                    #endregion

                    #region "Execute Sql Query"
                    try
                    {
                        results = DBContext.ExecuteQuery<ExamsScheduledTable>(sql);
                    }
                    catch (Exception ex)
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
                    foreach (ExamsScheduledTable result in results)
                    {
                        rr1 = new DicomDataSet();
                        rr = new DicomDataSet();
                        rrs = new DicomDataSetCollection { rr1 };

                        if (requestedProcedureStep != null)
                        {
                            rr.Add(Keyword.ScheduledProcedureStepSequence, rrs);
                        }

                        // add results to  "main" dataset

                        AddResultItem(rr, requestFromAssociation, Keyword.AccessionNumber, result.AccessionNumber);    // T2
                        AddResultItem(rr, requestFromAssociation, Keyword.InstitutionName, result.HospitalName);
                        AddResultItem(rr, requestFromAssociation, Keyword.ReferringPhysicianName, result.ReferringPhysician); // T2

                        AddResultItem(rr, requestFromAssociation, Keyword.PatientName, result.Surname + "^" + result.Forename + "^^" + result.Title); //T1
                        AddResultItem(rr, requestFromAssociation, Keyword.PatientID, result.PatientID); // T1
                        AddResultItem(rr, requestFromAssociation, Keyword.PatientBirthDate, result.DateOfBirth); // T2
                        AddResultItem(rr, requestFromAssociation, Keyword.PatientSex, result.Sex); //T2

                        AddResultItem(rr, requestFromAssociation, Keyword.StudyInstanceUID, result.StudyUID); // T1

                        AddResultItem(rr, requestFromAssociation, Keyword.RequestingPhysician, result.ReferringPhysician); //T2
                        AddResultItem(rr, requestFromAssociation, Keyword.RequestedProcedureDescription, result.ExamDescription); //T1C

                        AddResultItem(rr, requestFromAssociation, Keyword.RequestedProcedureID, result.ProcedureID); // T1

                        // Scheduled Procedure Step sequence T1
                        // add results to procedure step dataset
                        // Return if requested
                        if (requestedProcedureStep != null)
                        {
                            AddResultItem(rr1, requestedProcedureStep, Keyword.ScheduledStationAETitle, result.ScheduledAET); // T1
                            AddResultItem(rr1, requestedProcedureStep, Keyword.ScheduledProcedureStepStartDate, result.ExamDateAndTime); //T1
                            AddResultItem(rr1, requestedProcedureStep, Keyword.ScheduledProcedureStepStartTime, result.ExamDateAndTime); //T1
                            AddResultItem(rr1, requestedProcedureStep, Keyword.Modality, result.Modality); // T1

                            AddResultItem(rr1, requestedProcedureStep, Keyword.ScheduledPerformingPhysicianName, result.PerformingPhysician); //T2
                            AddResultItem(rr1, requestedProcedureStep, Keyword.ScheduledProcedureStepDescription, result.ExamDescription); // T1C
                            AddResultItem(rr1, requestedProcedureStep, Keyword.ScheduledProcedureStepID, result.ProcedureStepID); // T1
                            AddResultItem(rr1, requestedProcedureStep, Keyword.ScheduledStationName, result.ExamRoom); //T2
                            AddResultItem(rr1, requestedProcedureStep, Keyword.ScheduledProcedureStepLocation, result.ExamRoom); //T2
                        }

                        // Put blanks in for unsupported fields which are type 2 (i.e. must have a value even if NULL)
                        // In a real server, you may wish to support some or all of these, but they are not commonly supported

                        AddResultItem(rr, requestFromAssociation, Keyword.ReferencedStudySequence, NullSequence);
                        AddResultItem(rr, requestFromAssociation, Keyword.Priority, "");
                        AddResultItem(rr, requestFromAssociation, Keyword.PatientTransportArrangements, "");
                        AddResultItem(rr, requestFromAssociation, Keyword.AdmissionID, "");
                        AddResultItem(rr, requestFromAssociation, Keyword.CurrentPatientLocation, "");
                        AddResultItem(rr, requestFromAssociation, Keyword.ReferencedPatientSequence, NullSequence);
                        AddResultItem(rr, requestFromAssociation, Keyword.PatientWeight, "");
                        AddResultItem(rr, requestFromAssociation, Keyword.ConfidentialityConstraintOnPatientDataDescription, "");
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
                Log.Error("Error processing MWL request -- " + ex.Message);
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

        #region "helper functions"
        internal static void AddResultItem(DicomDataSet ResultDataSet, DicomDataSet request, Keyword keyword, object v)
        {
            // trace-level logging
            if (null != keyword.ToString() && keyword.ToString().Length > 2)
            {
                Log.Trace("Adding a result item of type: " + keyword.ToString());
            }
            // Only return fields (keywords) which have been requested
            if (request[keyword].Exists)
            {
                if (v == null)
                    v = "";
                ResultDataSet.Add(keyword, v);
            }
        }
        #endregion
    }
}