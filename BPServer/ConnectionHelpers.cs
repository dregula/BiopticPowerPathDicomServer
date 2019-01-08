using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BiopticPowerPathDicomServer
{
    public static class ConnectionHelpers
    {
        public static PowerPathLoginConfig ServerLoginFromPowerPathRegistry()
        {
            PowerPathLoginConfig serverlogin = new PowerPathLoginConfig();
            List<string> listRegistryLoginValueNames = new List<string>();
            List<string> listRegistryLoginSubKeys = new List<string>();
             RegistryKey rkPowerPath = null;
            using (RegistryKey rkCU = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default))
            {
                rkPowerPath = rkCU.OpenSubKey(@"Software\Tamtron\PowerPath Client\Login Info");
                listRegistryLoginSubKeys.AddRange(rkPowerPath.GetSubKeyNames());
            }
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = getRegistryStringValue(@"Server", rkPowerPath);
            builder.UserID = getRegistryStringValue(@"Login Name", rkPowerPath);
            builder.Password = getRegistryStringValue(@"Password", rkPowerPath);
            builder.InitialCatalog = getRegistryStringValue(@"Database", rkPowerPath);
            serverlogin = new PowerPathLoginConfig(builder);
            if (listRegistryLoginSubKeys.Contains(@"Servers"))
            {
                RegistryKey rkServers = rkPowerPath.OpenSubKey(@"Servers", false);
                if (null != rkServers)
                {
                    foreach (string valueName in rkServers.GetValueNames())
                    {
                        serverlogin.ListServers.Add(getRegistryStringValue(valueName, rkServers));
                    }
                }
            }
//TODO: add Server to listServers as required
            return serverlogin;
       }

        public static string getRegistryStringValue(string strValueName, RegistryKey key)
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

        public static string FeedbackFromTestDatabaseConnect(PowerPathLoginConfig serverlogin)
        {
            // ping first, as this is quicker
            //CONSIDER: refactor ping and Sql-db-connect into two async methods and wait for whomever comes first.
            if(false == PingHost(serverlogin.DataSource))
            {
                return "Server not found at: '" + serverlogin.DataSource + @"'!";
            }
            try
            {
                using (SqlConnection db = new SqlConnection(serverlogin.ConnectionString))
                {
                    db.Open();
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
