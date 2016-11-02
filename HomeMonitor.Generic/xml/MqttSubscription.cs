using HomeMonitor.xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace HomeMonitor.Generic.xml
{
    [Serializable]
    [XmlRoot("mqtt-subscribe")]
    public class MqttSubscription : GenericConfig
    {
        [XmlAttribute("topic")]
        public String Topic { get; set; }

        public override string GetId()
        {
            throw new NotImplementedException();
        }

        public override string GetName()
        {
            throw new NotImplementedException();
        }


    }
}

