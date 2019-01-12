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

     public partial class DicomServerConfiguration : DicomServerConfigurationInterface
    {
        private static readonly ILog Log
       = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //note: always seek to open this parent key; unless you cache it's subkeys
        public RegistryKey RkBiopticVisionSCP
        {
            get
            {
                RegistryKey rkbiopticvisionscp = null;
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

        //note: ExamScheduledTable string can be cached
        private string examscheduledtable;
        public string ExamScheduledTable
        {
            get
            {
                if (null == examscheduledtable)
                {
                    try
                    {
                        using (RegistryKey rkBiopticVisionSCPDataSource = RkBiopticVisionSCP.OpenSubKey(@"DataSource"))
                        {
                            try
                            {
                                examscheduledtable = (string)rkBiopticVisionSCPDataSource.GetValue(@"ExamScheduledTable", "");
                            }
                            catch (Exception ex)
                            {
                                Log.Error("Failed to open ExamScheduledTable value from BiopticVisionSCP configuration key: " + ex.Message);
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        Log.Fatal("Could not open required registry key: " + ex.Message);
                    }
                }
                //DEBUG: 2019-01-11 just until Registry values are stabalized
                if(null == examscheduledtable || examscheduledtable.Length < 4 )
                    examscheduledtable = @"vwsu_scheduled_speciman_xrays";

                return examscheduledtable;
            }
        }

        // Port: (can be cached)
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
                    try
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
                    catch(Exception ex)
                    {
                        Log.Error(@"Failed to get a required registry key: " + ex.Message);
                    }
                }
                //DEBUG: 2019-01-11 just until Registry values are stablized
                if (portnumber < 1024)
                    portnumber =11112;

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
                //DEBUG: 2019-01-11 just until Registry values are stablized
                if (null==ipaddress || ipaddress.Length < 4)
                    ipaddress = @"127.0.0.1";

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
                //DEBUG: 2019-01-11 just until Registry values are stablized
                if (null == ipaddressfamily || ipaddressfamily.Length < 4)
                    ipaddressfamily = @"IPv4";

                return ipaddressfamily;
            }
        }
    }
}
