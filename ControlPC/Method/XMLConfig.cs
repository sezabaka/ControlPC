using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlPC.Method
{
    class XMLConfig
    {
        public static List<string> GetSettings(string WMIClassName)
        {
            string xmlFilePath = System.IO.Directory.GetCurrentDirectory() + "\\settings.xml";
            List<string> alPropertyNames = new List<string>();
            System.Xml.XmlDocument xmldoc = new System.Xml.XmlDocument();
            xmldoc.Load(xmlFilePath);
            System.Xml.XmlNode properties = xmldoc.SelectSingleNode("//" + WMIClassName);

            for (int i = 0; i < properties.ChildNodes.Count; i++)
                alPropertyNames.Add(properties.ChildNodes[i].InnerText);
            return alPropertyNames;
        }
    }
}
