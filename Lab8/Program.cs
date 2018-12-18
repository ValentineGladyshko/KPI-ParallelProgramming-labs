using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Lab8
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = new Uri(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)).LocalPath;
            XmlSchemaSet schema = new XmlSchemaSet();
            schema.Add("", path + "../../../MobileTariffs.xsd");
            XmlReader rd = XmlReader.Create(path + "../../../MobileTariffs.xml");
            XDocument doc = XDocument.Load(rd);
            doc.Validate(schema, ValidationEventHandler);

            XmlSerializer serializer = new XmlSerializer(typeof(MobileTariffs));

            using (FileStream stream = File.OpenRead(path + "../../../MobileTariffs.xml"))
            {
                MobileTariffs dezerializedList = (MobileTariffs)serializer.Deserialize(stream);

                var jsonItems = JsonConvert.SerializeObject(dezerializedList);
                System.IO.File.WriteAllText(path + "../../../MobileTariffs.txt", jsonItems);
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            XmlSeverityType type = XmlSeverityType.Warning;
            if (Enum.TryParse<XmlSeverityType>("Error", out type))
            {
                if (type == XmlSeverityType.Error) throw new Exception(e.Message);
            }
        }
    }
}
