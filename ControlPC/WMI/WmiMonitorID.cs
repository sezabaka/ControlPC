using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlPC.Method;

namespace ControlPC.WMI
{
    class WmiMonitorID : IWMI
    {
        Connection WMIConnection;

        public WmiMonitorID(Connection WMIConnection)
        {
            this.WMIConnection = WMIConnection;
        }

        public IList<string> GetPropertyValues()
        {
            string className = "WmiMonitorID";

            return WMIReader.GetPropertyValues(WMIConnection,
                                               "SELECT * FROM " + className,
                                               className, true);
        }
    }
}
