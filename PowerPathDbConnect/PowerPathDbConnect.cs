using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using Microsoft.Win32;


namespace BiopticPowerPathDicomServer
{
    interface PowerPathDbConnectInterface
    {
        string ConnectionString { get; }
        string Server { get; set; }
        string UserID { get; set; }
        string Password { get; set; }
        List<string> RecentServers { get; set; }
        List<string> AvailableDatabases { get; set; }
    }

    public class PowerPathDbConnect : PowerPathDbConnectInterface
    {
        private static ILog Log = LogManager.GetLogger("PowerPathDbConnect");

        private RegistryKey rkPowerPath;
        private List<string> listRegistryLoginValueNames = new List<string>();
        private List<string> listRegistryLoginSubKeys = new List<string>();
        private List<string> listServers = new List<string>();
        private List<string> listDatabases = new List<string>();
        private string password = "";

        public PowerPathDbConnect()
        {

            using (RegistryKey rkCU = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default))
            {
                this.rkPowerPath = rkCU.OpenSubKey(@"Software\Tamtron\PowerPath Client\Login Info");
                listRegistryLoginSubKeys.AddRange(rkPowerPath.GetSubKeyNames());
            }
            builder = new SqlConnectionStringBuilder();
            builder.DataSource = getRegistryStringValue(@"Server", rkPowerPath);
            builder.UserID = getRegistryStringValue(@"Login Name", rkPowerPath);
            builder.Password = getRegistryStringValue(@"Password", rkPowerPath);
            builder.InitialCatalog = getRegistryStringValue(@"Database", rkPowerPath);
            if (listRegistryLoginSubKeys.Contains(@"Servers"))
            {
                RegistryKey rkServers = rkPowerPath.OpenSubKey(@"Servers", false);
                if (null != rkServers)
                {
                    foreach (string valueName in rkServers.GetValueNames())
                    {
                        listServers.Add(getRegistryStringValue(valueName, rkServers));
                    }
                }
            }
        }

        private string getRegistryStringValue(string strValueName, RegistryKey key)
        {
            if (null == key) return "";
            if (strValueName.Length < 4) return "";
            List<string> listRegistryValueNames = new List<string>();
            listRegistryValueNames.AddRange(key.GetValueNames());
            if (false == listRegistryValueNames.Contains(strValueName)) return "";
            RegistryValueKind valKind = key.GetValueKind(strValueName);
            // room to refactor for binary password...
            switch (valKind)
            {
                case RegistryValueKind.String:
                    return (string)key.GetValue(strValueName, "");
                default:
                    return "";
            }
        }

        private SqlConnectionStringBuilder builder;
        public SqlConnectionStringBuilder Builder
        {
            get { return builder; }
            set { builder = value; }
        }


        public string ConnectionString
        {
            get
            {
                return builder.ConnectionString;
            }
        }

        public string Database
        {
            get
            {
                return builder.InitialCatalog;
            }
            set
            {
                builder.InitialCatalog = value;
                //rkPowerPath.SetValue(@"Database", (string)value);
            }
        }

        public string Server
        {
            get
            {
                return builder.DataSource;
            }
            set
            {
                builder.DataSource = value;
                //rkPowerPath.SetValue(@"Server", (string)value);
            }
        }

        public string UserID
        {
            get
            {
                return builder.UserID;
            }
            set
            {
                builder.UserID = value;
                //rkPowerPath.SetValue(@"Login Name", (string)value);
            }
        }

        public string Password
        {
            get { return builder.Password; }
            set { builder.Password = value; }
        }


        public List<string> RecentServers
        {
            get
            {
                return listServers;
            }
            set
            {
                //TODO: compare internal array against registry values?
                //or add a "new" server to existing list in registry
            }
        }
        public List<string> AvailableDatabases
        {
            get
            {
                // mock code
                return listDatabases; ;
            }
            set
            {
                //TODO: implement find available databases
            }
        }

        public string FeedbackFromCheckConnectionBuilder()
        {
            if (this.Server.Length < 1)
            {
                return @"Please enter the database server name or IP address!";
            }
            if (this.UserID.Length < 1)
            {
                return @"Username must be entered!";
            }
            if (this.Password.Length < 1)
            {
                return @"Password must be entered!";
            }
            return "";
        }

//TODO: pass-in a builder, so we can log all details, but only show user the error...
        public static string FeedbackFromTestPowerPathConnect(string strConnection)
        {
            try
            {
                using (SqlConnection db = new SqlConnection(strConnection))
                {
                    db.Open();
                    /* If we reached here, that means the connection to the database was successful. */
                    return "";
                }
            }
            catch (SqlException se)
            {
                //TODO: if server connected, but n=wrong database, then do something...
                string strSqlException = "Sql Error connecting to PowerPath"
                //    + " with connectionstring: '" + strConnection
                    + " with error: " + se.Message;
                Log.Warn(strSqlException);
                return strSqlException;
            }
            catch (Exception ex)
            {
                string strException = "Error connecting to PowerPath"
                //    + " with connectionstring: '" + strConnection
                    + " with error: " + ex.Message;
                Log.Error(strException);
                return strException;
            }
        }

        static public async Task<string> AsyncFeedbackFromTestPowerPathConnect(string strConnection)
        {
            if (strConnection.Length < 4) return "";
            try
            {
                using (SqlConnection db = new SqlConnection(strConnection))
                {
                    await db.OpenAsync();
                    /* If we reached here, that means the connection to the database was successful. */
                    return "";
                }
            }
            catch (SqlException se)
            {
                //TODO: if server connected, but n=wrong database, then do something...
                string strSqlException = "Sql Error connecting to PowerPath"
                //    + " with connectionstring: '" + strConnection
                    + " with error: " + se.Message;
                Log.Warn(strSqlException);
                return strSqlException;
            }
            catch (Exception ex)
            {
                string strException = "Error connecting to PowerPath"
                //    + " with connectionstring: '" + strConnection
                    + " with error: " + ex.Message;
                Log.Error(strException);
                return strException;
            }
        }
    }
}
