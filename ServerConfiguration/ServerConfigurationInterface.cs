using System;
using System.Collections.Generic;
using System.Text;

namespace BiopticPowerPathDicomServer
{
    interface PowerPathConfigurationInterface
    {
        string ConnectionString { get;}
        string Server { get; set; }
        string UserID { get; set; }
        string Password { get; set; }
        List<string> RecentServers { get; set; }
        List<string> AvailableDatabases { get; set; }
    }

    interface DicomServerConfigurationInterface
    {
        int Portnumber { get; set; }
        string IpAddress { get; set; }
        string IpAddressFamily { get; set; }
    }
}
