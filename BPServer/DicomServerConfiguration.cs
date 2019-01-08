using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.AccessControl;
using Microsoft.Win32;

namespace BiopticPowerPathDicomServer
{
    interface DicomServerConfigurationInterface
    {
        int Portnumber { get; set; }
        string IpAddress { get; set; }
        string IpAddressFamily { get; set; }
    }

    //TODO: ReImplement with backing variables and query the Registry ONCE!
    public partial class DicomServerConfiguration : DicomServerConfigurationInterface
    {
        private RegistryKey rkDicomObject;
        public DicomServerConfiguration()
        {
            RegistryRights rrBiopticVisionSCP = RegistryRights.EnumerateSubKeys | RegistryRights.QueryValues | RegistryRights.ReadKey;
            using (RegistryKey rkCUHive = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default))
            {
                rkDicomObject = rkCUHive.OpenSubKey(@"Software\Stanford\BiopticVisionSCP", rrBiopticVisionSCP);
                if (rkDicomObject == null)
                {
                    using (RegistryKey rkLKHive = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default))
                    {
                        rkDicomObject = rkLKHive.OpenSubKey(@"Software\Stanford\BiopticVisionSCP", rrBiopticVisionSCP);
                    }
                }
            }
        }

        // DataDource:
        //  Values specific to connecting to data in the source (PowerPath)
        //      ExamScheduledTable:
        //          The table in the PowerPath database which contains the X-Ray order data

        public string ExamScheduledTable
        {
            get
            {
                using (RegistryKey rkDicomObjectPort = rkDicomObject.OpenSubKey(@"DataSource"))
                {
                    return (string)rkDicomObjectPort.GetValue(@"ExamScheduledTable", "");
                }
            }
        }

        // Port:
        //     The TCP port on which to receive connections
        //
        //   Address:
        //     The address to listen on
        //
        //   IpAddressFamily:
        //     The address family to use (IPv4 or IPv6)
        //
        //   AcceptTls:

        public int Portnumber
        {
            get
            {
                using (RegistryKey rkDicomObjectPort = rkDicomObject.OpenSubKey(@"Port"))
                {
                    string strPortnumberraw = rkDicomObjectPort.GetValue(@"", 0).ToString();
                    int portnumber;
                    if (false == Int32.TryParse(strPortnumberraw, out portnumber))
                    {
                        //this.Log("Faled to convert portnomber registry string '" + strPortnumberraw + "' to an integer!");
                        return -1;
                    }
                    return portnumber;
                }
            }
            set
            {
                using (RegistryKey rkDicomObjectPort = rkDicomObject.OpenSubKey(@"Port"))
                {
                    rkDicomObjectPort.SetValue(@"", value);
                }
            }
        }

        public string IpAddress
        {
            get
            {
                using (RegistryKey rkDicomObjectPort = rkDicomObject.OpenSubKey(@"Port"))
                {
                    return (string)rkDicomObjectPort.GetValue(@"Address", "");
                }
            }
            set
            {
                using (RegistryKey rkDicomObjectPort = rkDicomObject.OpenSubKey(@"Port"))
                {
                    rkDicomObjectPort.SetValue(@"Address", value);
                }
            }
        }

        public string IpAddressFamily
        {
            get
            {
                using (RegistryKey rkDicomObjectPort = rkDicomObject.OpenSubKey(@"Port"))
                {
                    return (string)rkDicomObjectPort.GetValue(@"IpAddressFamily", "");
                }
            }
            set
            {
                using (RegistryKey rkDicomObjectPort = rkDicomObject.OpenSubKey(@"Port"))
                {
                    rkDicomObjectPort.SetValue(@"IpAddressFamily", value);
                }
            }
        }
    }
}
