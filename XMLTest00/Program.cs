using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace XMLTest00
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlDocument doc;
            XmlTextReader reader;
//            List<String> test0 = new List<String>();

            doc = new XmlDocument();
            reader = new XmlTextReader("XML Test00.xml");
            doc.Load(reader);

            XmlNodeList elementList = doc.SelectNodes("/strings/string");

            foreach (XmlNode stringElement in elementList)
            {
                XmlNode id = stringElement.SelectSingleNode("id");
                Console.WriteLine(id.InnerText);
            }
        }
    }
}
