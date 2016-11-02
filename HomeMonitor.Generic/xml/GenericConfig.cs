using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace HomeMonitor.Generic.xml
{
    public abstract class GenericConfig 

    {
        public abstract String GetId();
        public abstract String GetName();
        //public XmlSchema GetSchema() { return null; }
        [XmlAttribute("min-sensitivity")]
        public int MinSensitivity { get; set; }

        [XmlAttribute("max-sensitivity")]
        public int MaxSensitivity { get; set; }

        public GenericConfig()
        {
            MinSensitivity = 2;
            MaxSensitivity = 10;
        }
    }
}
