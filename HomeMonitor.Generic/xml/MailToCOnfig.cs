using HomeMonitor.Generic.xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HomeMonitor.Generic.xml
{
    [Serializable]
    [XmlRoot("to")]
    public class MailToConfig : GenericConfig
    {

        [XmlAttribute("address")]
        public String Address { get; set; }
        

        public override string GetId()
        {
            throw new NotImplementedException();
        }

        public override string GetName()
        {
            return "to";
        }

    }
}
