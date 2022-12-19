using System.Collections.Generic;
using ControlPC.Method;

namespace ControlPC.WMI
{
    class Win32_PnPEntity : IWMI
    {
        Connection WMIConnection;

        public Win32_PnPEntity(Connection WMIConnection)
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
