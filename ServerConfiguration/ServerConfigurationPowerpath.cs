using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
namespace BiopticPowerPathDicomServer
{
    public partial class ServerConfiguration
    {
        private List<string> listServers = new List<string>();
        private List<string> listDatabases = new List<string>();
//private string[] arrServers = null;
//private string[] arrDatabases = null;
        private string password;

        private SqlConnectionStringBuilder builder;
        public SqlConnectionStringBuilder Builder
        {
            get { return builder; }
            set { builder = value;  }
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
                return (string)rkPowerPath.GetValue(@"Database", "");
            }
            set
            {
                rkPowerPath.SetValue(@"Database", (string)value);
            }
        }

        public string Server
        {
            get
            {
                return (string)rkPowerPath.GetValue(@"Server", "");
            }
            set
            {
                rkPowerPath.SetValue(@"Server", (string)value);
            }
        }

        public string UserID
        {
            get
            {
                return (string)rkPowerPath.GetValue(@"Login Name", "");
            }
            set
            {
                rkPowerPath.SetValue(@"Login Name", (string)value);
            }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }


        public List<string> RecentServers
        {
            get
            {
                RegistryKey rkServers = rkPowerPath.OpenSubKey(@"Servers", false);
                if (rkServers == null || rkServers.ValueCount < 1) { return null; }
                int i = 0;
                foreach (string valueName in rkServers.GetValueNames())
                {
                    listServers.Add((string)rkServers.GetValue(valueName));
                    i++;
                }
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
    }
}
