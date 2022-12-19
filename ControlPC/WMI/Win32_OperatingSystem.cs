using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlPC.Method;

namespace ControlPC.WMI
{
    class Win32_OperatingSystem : IWMI
    {
        Connection WMIConnection;

        public Win32_OperatingSystem(Connection WMIConnection)
        {
            this.WMIConnection = WMIConnection;
        }

        public IList<string> GetPropertyValues()
        {
            string className = System.Text.RegularExpressions.Regex.Match(
                                  this.GetType().ToString(), "Win32_.*").Value;

            return WMIReader.GetPropertyValues(WMIConnection,
                                               "SELECT * FROM " + className,
                                               className);
        }
    }
}
