using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace BiopticPowerPathDicomServer
{
    public static class RegistryHelper
    {
        public static string getRegistryStringValue(string strValueName, RegistryKey key)
        {
            if (null == key) return "";
            if (strValueName.Length < 4) return "";
            List<string> listRegistryValueNames = new List<string>();
            listRegistryValueNames.AddRange(key.GetValueNames());
            if (false == listRegistryValueNames.Contains(strValueName)) return "";
            RegistryValueKind valKind = key.GetValueKind(strValueName);
            // room to refactor for binary password...
            switch (valKind)
            {
                case RegistryValueKind.String:
                    return (string)key.GetValue(strValueName, "");
                default:
                    return "";
            }
        }


     }
}
