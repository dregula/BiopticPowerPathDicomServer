using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Win32;
using Common.Logging;

namespace BiopticPowerPathDicomServer
{
    public class PowerPathConfiguration
    {
        private static readonly ILog Log
       = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private SqlConnectionStringBuilder builder;
        private List<string> listServers = new List<string>();
        private List<string> listDatabases = new List<string>();

        #region "Constructors"
        public PowerPathConfiguration()
        {
            validdbconnection = false;
        }
        #endregion

        #region "Properties"
        private bool validdbconnection;
        public bool ValidDbConnection
        {
            get { return validdbconnection; }
            set { validdbconnection =value; }
        }

        private string scheduledexamstable;
        public string ScheduledExamsTable
        {
            get { return scheduledexamstable; }
            set { scheduledexamstable = value; }
        }

        public SqlConnectionStringBuilder Builder
        {
            get
            {
                if (null == builder)
                {
                    builder = new SqlConnectionStringBuilder();
                    InitializeFromPowerPathRegistry();
                    //TRY: 2019-04-06
                    builder.ApplicationName = @"PowerPath Client";
                    builder.WorkstationID = Environment.MachineName + @"\" + Environment.UserName;
                    //2019-04-06:  Connecting to a mirrored SQL Server instance using the ApplicationIntent ReadOnly connection option is not supported.   .ApplicationIntent = ApplicationIntent.ReadOnly;
                }
                return builder;
            }
            set { builder = value; }
        }
        public PowerPathConfiguration Copy()
        {
            PowerPathConfiguration cp = new PowerPathConfiguration();
            cp.Builder = this.Builder;
            cp.ListDatabases = new List<string>(listDatabases);
            cp.ListServers = new List<string>(listServers);
            cp.ValidDbConnection = this.ValidDbConnection;
            return cp;
        }
        //TODO: reconcile Server property and ListServers property
        public List<string> ListServers
        {
            get { return listServers; }
            set { listServers = value; }
        }
        public List<string> ListDatabases
        {
            get { return listDatabases; }
            set { listDatabases = value; }
        }

        public string ConnectionString
        {
            get { return builder.ConnectionString; }
            set { builder.ConnectionString = value; }
        }
        public string DataSource
        {
            get { return builder.DataSource; }
            set { builder.DataSource = value; }
        }
        public string UserID
        {
            get { return builder.UserID; }
            set { builder.UserID = value; }
        }
        public string Password
        {
            get { return builder.Password; }
            set { builder.Password = value; }
        }
        public string InitialCatalog
        {
            get { return builder.InitialCatalog; }
            set { builder.InitialCatalog = value; }
        }
        public string WorkstationID
        {
            get { return builder.WorkstationID; }
            set { builder.WorkstationID = value; }
        }
        public bool IntegratedSecurity
        {
            get { return builder.IntegratedSecurity; }
            set { builder.IntegratedSecurity = value; }
        }
        #endregion


        private void InitializeFromPowerPathRegistry()
        {
            List<string> listRegistryLoginSubKeys = new List<string>();
            RegistryKey rkPowerPath = null;
            using (RegistryKey rkCU = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default))
            {
                try
                {
                    rkPowerPath = rkCU.OpenSubKey(@"Software\Tamtron\PowerPath Client\Login Info");
                    listRegistryLoginSubKeys.AddRange(rkPowerPath.GetSubKeyNames());
                }
                catch (Exception ex)
                {
                    Log.Error("Failed to open Powerpath Login configuration from Current user Registry: " + ex.Message);
 //TODO: reconsider throw...
                    throw;
                }
            }
            try
            {

                builder.DataSource = RegistryHelper.getRegistryStringValue(@"Server", rkPowerPath);
                builder.UserID = RegistryHelper.getRegistryStringValue(@"Login Name", rkPowerPath);
                builder.Password = RegistryHelper.getRegistryStringValue(@"Password", rkPowerPath);
                builder.InitialCatalog = RegistryHelper.getRegistryStringValue(@"Database", rkPowerPath);
            }
            catch (Exception ex)
            {
                Log.Error("Failed to get one or more required values from Powerpath Login key: " + ex.Message);
                //TODO: reconsider throw...
                throw;
            }

            if (listRegistryLoginSubKeys.Contains(@"Servers"))
            {
                try
                {
                    RegistryKey rkServers = rkPowerPath.OpenSubKey(@"Servers", false);
                    if (null != rkServers)
                    {
                        foreach (string valueName in rkServers.GetValueNames())
                        {
                            this.ListServers.Add(RegistryHelper.getRegistryStringValue(valueName, rkServers));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Warn("Failed to get Servers values from Powerpath Login key: " + ex.Message);
                    //TODO: reconsider throw...
                    throw;
                }

            }
        }
    }
}
