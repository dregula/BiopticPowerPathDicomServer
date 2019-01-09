using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.AccessControl;
using Microsoft.Win32;
using Common.Logging;

namespace BiopticPowerPathDicomServer
{
    interface DicomServerConfigurationInterface
    {
        int Portnumber { get; }
        string IpAddress { get; }
        string IpAddressFamily { get;}
    }

    //TODO: ReImplement with backing variables and query the Registry ONCE!
    public partial class DicomServerConfiguration : DicomServerConfigurationInterface
    {
        private static ILog Log = LogManager.GetLogger("DicomServerConfiguration");

        private RegistryKey rkbiopticvisionscp = null;
        public RegistryKey RkBiopticVisionSCP
        {
            get
            {
                if(null== rkbiopticvisionscp)
                {
                    RegistryRights rrBiopticVisionSCP = RegistryRights.EnumerateSubKeys | RegistryRights.QueryValues | RegistryRights.ReadKey;
                    using (RegistryKey rkCUHive = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default))
                    {
                        try
                        {
                            rkbiopticvisionscp = rkCUHive.OpenSubKey(@"Software\Stanford\BiopticVisionSCP", rrBiopticVisionSCP);
                        }
                        catch(Exception ex)
                        {
                            Log.Error("Failed to open BiopticVisionSCP configuration from Local Machine Registry: " + ex.Message);
                        }
                        if (rkbiopticvisionscp == null)
                        {
                            using (RegistryKey rkLKHive = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default))
                            {
                                try
                                {
                                    rkbiopticvisionscp = rkLKHive.OpenSubKey(@"Software\Stanford\BiopticVisionSCP", rrBiopticVisionSCP);
                                }
                                catch (Exception ex)
                                {
                                    Log.Error("Failed to open BiopticVisionSCP configuration from Current User Registry: " + ex.Message);
                                }
                            }
                        }
                    }
                }
                return rkbiopticvisionscp;
            }
        }

        public DicomServerConfiguration()
        {
        }

        // DataDource:
        //  Values specific to connecting to data in the source (PowerPath)
        //      ExamScheduledTable:
        //          The table in the PowerPath database which contains the X-Ray order data

        private string examscheduledtable;
        public string ExamScheduledTable
        {
            get
            {
                if (null == examscheduledtable)
                {
                    using (RegistryKey rkBiopticVisionSCP = RkBiopticVisionSCP.OpenSubKey(@"DataSource"))
                    {
                        try
                        {
                            examscheduledtable = (string)rkBiopticVisionSCP.GetValue(@"ExamScheduledTable", "");
                        }
                        catch (Exception ex)
                        {
                            Log.Error("Failed to open ExamScheduledTable value from BiopticVisionSCP configuration key: " + ex.Message);
                        }
                    }
                }
                return examscheduledtable;
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

        private int portnumber = -1;
        public int Portnumber
        {
            get
            {
                if (portnumber < 1022)
                {
                    using (RegistryKey rkDicomObjectPort = RkBiopticVisionSCP.OpenSubKey(@"Port"))
                    {
                        string strPortnumberraw = "";
                        try
                        {
                            strPortnumberraw = rkDicomObjectPort.GetValue(@"", 0).ToString();
                        }
                        catch (Exception ex)
                        {
                            Log.Error("Failed to open Portnumber value from BiopticVisionSCP configuration key: " + ex.Message);
                        }
                        if (false == Int32.TryParse(strPortnumberraw, out portnumber))
                        {
                            Log.Error("Failed to convert portnomber registry string '" + strPortnumberraw + "' to an integer!");
                        }
                    }
                }
                return portnumber;
            }
        }

        private string ipaddress= "";
        public string IpAddress
        {
            get
            {
                if (ipaddress.Length < 8)
                {
                    try
                    {
                        using (RegistryKey rkDicomObjectPort = RkBiopticVisionSCP.OpenSubKey(@"Port"))
                        {
                            ipaddress = (string)rkDicomObjectPort.GetValue(@"Address", "");
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Failed to open IpAddress value from BiopticVisionSCP configuration key: " + ex.Message);
                    }
                }
                return ipaddress;
            }
         }

        private string ipaddressfamily = "";
        public string IpAddressFamily
        {
            get
            {
                if (ipaddressfamily.Length < 2)
                {
                    try
                    {
                        using (RegistryKey rkDicomObjectPort = RkBiopticVisionSCP.OpenSubKey(@"Port"))
                        {
                            ipaddressfamily = (string)rkDicomObjectPort.GetValue(@"IpAddressFamily", "");
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Failed to open IpAddressFamily value from BiopticVisionSCP configuration key: " + ex.Message);
                    }
                }
                return ipaddressfamily;
            }
        }
    }
}
