using Common.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiopticPowerPathDicomServer
{
    public class PowerPathConfigurationModel
    {
        private static readonly ILog Log
          = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private PowerPathConfigurationViewModel _powerPathConfiguration;
        public PowerPathConfigurationViewModel PowerPathConfiguration
        {
            get
            {
                return _powerPathConfiguration;
            }
        }

        public PowerPathConfigurationModel()
        {
            _powerPathConfiguration = new PowerPathConfigurationViewModel()
            {
                ValidDbConnection = false,
                Builder = new SqlConnectionStringBuilder(),
                ListServers = new List<string>(),
                ListDatabases = new List<string>()
            };
            PowerPathConfigurationFromRegistry.InitializeFromPowerPathRegistry(ref _powerPathConfiguration);
        }

        public void ValidateDbConnection()
        {
            string strFeedbackFromTestPowerPathConnect = FeedbackFromTestDatabaseConnect(_powerPathConfiguration);
            if (strFeedbackFromTestPowerPathConnect.Length > 0)
            {
                Log.Trace(@"Attempt to Database-connect: " + strFeedbackFromTestPowerPathConnect);
                _powerPathConfiguration.ValidDbConnection = false;
            }
            else // sucessfully connected to db!
            {
                _powerPathConfiguration.ValidDbConnection = true;
                Log.Trace(@"Successfully connected to PowerPath!");
                //TODO: how to close the form from this model...
                // ?https://www.codeproject.com/Tips/499977/%2FTips%2F499977%2FClosing-View-from-ViewModel
                //this.Close();
            }
        }
        public void Quit()
        {
            //TODO: how to close the form from this model...
            //this.Close();
        }

        private string FeedbackFromTestDatabaseConnect(PowerPathConfigurationViewModel serverlogin)
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
