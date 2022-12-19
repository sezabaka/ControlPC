using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace ControlPC.Method
{
    class WMIReader
    {
        public static IList<string> GetPropertyValues(Connection WMIConnection,
                                                      string SelectQuery,
                                                      string className,
                                                        bool ChangeRoot = false)
        {
            ManagementScope connectionScope;

            if (!ChangeRoot)
                connectionScope = WMIConnection.GetConnectionScope;
            else
                connectionScope = WMIConnection.GetConnectionScope2;

            List<string> alProperties = new List<string>();
            SelectQuery msQuery = new SelectQuery(SelectQuery);
            ManagementObjectSearcher searchProcedure = new ManagementObjectSearcher(connectionScope, msQuery);

            try
            {
                if (!ChangeRoot)
                {
                    foreach (ManagementObject item in searchProcedure.Get())
                    {
                        foreach (string property in XMLConfig.GetSettings(className))
                        {
                            try { alProperties.Add(property + ": " + item[property].ToString()); }
                            catch (SystemException) { /* ignore error */ }
                        }
                    }
                }
                else
                {
                    foreach (ManagementObject item in searchProcedure.Get())
                    {
                        foreach (string property in XMLConfig.GetSettings(className))
                        {
                            try
                            {
                                ushort[] lu = (ushort[])item[property];
                                List<byte> lb = new List<byte>();

                                for (int i = 0; i < lu.Length; i++)
                                {
                                    if (lu[i] != 0)
                                        lb.Add((byte)lu[i]);
                                }

                                string ss = Encoding.ASCII.GetString(lb.ToArray());
                                alProperties.Add(ss);
                            }
                            catch (SystemException) { /* ignore error */ }
                        }
                    }
                }
            }
            catch
            {
                /* Do Nothing */
            }

            return alProperties;
        }
    }
}
