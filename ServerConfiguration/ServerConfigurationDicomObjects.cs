using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace BiopticPowerPathDicomServer
{
    public partial class ServerConfiguration
    {
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
