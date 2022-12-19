using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlPC.Method;

namespace ControlPC.WMI
{
    class Win32_NetworkAdapterConfiguration : IWMI
    {
        Connection WMIConnection;

        string mac;

        public Win32_NetworkAdapterConfiguration(Connection WMIConnection, string mac)
        {
            this.WMIConnection = WMIConnection;
            this.mac = mac;
        }

        public IList<string> GetPropertyValues()
        {
            string className = System.Text.RegularExpressions.Regex.Match(
                                  this.GetType().ToString(), "Win32_.*").Value;

            return WMIReader.GetPropertyValues(WMIConnection,
                                               "SELECT * FROM " + className + " WHERE MacAddress = '" + mac + "'",
                                               className);
        }
    }
}

