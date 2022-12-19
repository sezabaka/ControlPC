using System.Collections.Generic;

namespace ControlPC.WMI
{
    interface IWMI
    {
        IList<string> GetPropertyValues();
    }
}
