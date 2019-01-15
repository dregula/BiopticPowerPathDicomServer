using Common.Logging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiopticPowerPathDicomServer
{
    public class PowerPathConfigurationFromRegistry
    {
        private static readonly ILog Log
          = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void InitializeFromPowerPathRegistry(ref PowerPathConfigurationViewModel ppc)
        {
            List<string> listRegistryLoginSubKeys = new List<string>();
            RegistryKey rkPowerPath = null;
            using (RegistryKey rkCU = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default))
            {
                try
                {
                    rkPowerPath = rkCU.OpenSubKey(@"Software\Tamtron\PowerPath Client\Login Info");
                    listRegistryLoginSubKeys.AddRange(rkPowerPath.GetSubKeyNames());
                }
                catch (Exception ex)
                {
                    Log.Error("Failed to open Powerpath Login configuration from Current user Registry: " + ex.Message);
                    //TODO: reconsider throw...
                    throw;
                }
            }
            try
            {

                ppc.DataSource = RegistryHelper.getRegistryStringValue(@"Server", rkPowerPath);
                ppc.UserID = RegistryHelper.getRegistryStringValue(@"Login Name", rkPowerPath);
                ppc.Password = RegistryHelper.getRegistryStringValue(@"Password", rkPowerPath);
                ppc.InitialCatalog = RegistryHelper.getRegistryStringValue(@"Database", rkPowerPath);
            }
            catch (Exception ex)
            {
                Log.Error("Failed to get one or more required values from Powerpath Login key: " + ex.Message);
                //TODO: reconsider throw...
                throw;
            }

            if (listRegistryLoginSubKeys.Contains(@"Servers"))
            {
                try
                {
                    RegistryKey rkServers = rkPowerPath.OpenSubKey(@"Servers", false);
                    if (null != rkServers)
                    {
                        foreach (string valueName in rkServers.GetValueNames())
                        {
                            ppc.ListServers.Add(RegistryHelper.getRegistryStringValue(valueName, rkServers));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Warn("Failed to get Servers values from Powerpath Login key: " + ex.Message);
                    //TODO: reconsider throw...
                    throw;
                }
            }
        }
    }
}
