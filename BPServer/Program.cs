using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BiopticPowerPathDicomServer
{
    /// <summary>
    /// Dicom server connecting Bioptics X-Ray  workstation to PowerPath database
    /// </summary>
    /// <remarks>
    /// get DicomServer configuration synchronously
    /// get PowerPath connection configuration synchronously
    /// attempt db connection
    ///     Yes- start server
    ///     No- raise PP login form
    ///         attempt db connection
    ///         Yes- update Registry, close form & start server
    ///         No- repeat with existing login form until cancelled
    /// </remarks>
    class Program
    {
        static void Main(string[] args)
        {
            ApplicationContext appContext = new ApplicationContext();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(appContext);
            BPServer bpserver = new BPServer(appContext);
            Console.ReadKey(true);
        }
    }
}
