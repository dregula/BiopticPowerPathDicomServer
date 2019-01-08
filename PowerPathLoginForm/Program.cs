using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BiopticPowerPathDicomServer
{
    static class Program
    {
        /// <summary>
        /// This form is typically called from another console application
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ServerLogin serverlogin = ConnectionHelpers.ServerLoginFromPowerPathRegistry();
            Application.Run(new PPLoginForm(serverlogin));
        }
    }
}
