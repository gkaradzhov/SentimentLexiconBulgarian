using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SentimentLexicon
{
    class XmlReader
    {
        public static Grabo ReadData()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Grabo));
            using (FileStream fileStream = new FileStream("grabo.xml", FileMode.Open))
            {
                Grabo result = (Grabo)serializer.Deserialize(fileStream);
                return result;
            }
        }
    }
}
