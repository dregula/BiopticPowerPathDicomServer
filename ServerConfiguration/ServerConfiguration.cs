using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace BiopticPowerPathDicomServer
{
    public partial class ServerConfiguration : DicomServerConfigurationInterface, PowerPathConfigurationInterface
    {
        private RegistryKey rkDicomObject;
        private RegistryKey rkPowerPath;
 
        public ServerConfiguration()
        {
//TODO: use only values from HKLM!
            using (RegistryKey rkHive = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default))
            {
                rkDicomObject = rkHive.OpenSubKey(@"Software\Stanford\BiopticVisionSCP");
                if (rkDicomObject == null)
                {
                    rkDicomObject = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default).OpenSubKey(@"Software\Stanford\BiopticVisionSCP");
                }
                if (rkDicomObject == null)
                {
                    CreateNewUserScpRegistry();
                }
            }

            using (RegistryKey rkCU = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default))
            {
                this.rkPowerPath = rkCU.OpenSubKey(@"Software\Tamtron\PowerPath Client\Login Info");
            }
        }

//TODO: copy from HKLM?
        private void CreateNewUserScpRegistry()
        {
            using (RegistryKey rkHive = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default))
            {
                rkDicomObject = rkHive.CreateSubKey(@"Software\Stanford\BiopticVisionSCP");
                using (RegistryKey rkPort = rkDicomObject.CreateSubKey(@"Port"))
                {
                    rkPort.SetValue("", "11112");
                    rkPort.SetValue(@"Address", "127.0.0.1");
                    // rkPort.SetValue(@"IpAddressFamily", "IP4");
                    // rkPort.SetValue(@"AcceptTls", 1);
                }
            }
        }
    }
}
