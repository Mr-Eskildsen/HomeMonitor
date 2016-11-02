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
    [XmlRoot("mqtt")]
    public class MqttConfig : GenericConfig
    {
        [XmlAttribute("host")]
        public String   host { get; set; }

        [XmlAttribute("port")]
        public String   port { get; set; }

        [XmlAttribute("qos")]
        public int      qos { get; set; }

        //[XmlAttribute("topic-in")]
        //public String IncommingTopic { get; set; }
        [XmlElement("mqtt-subscribe")]
        public List<MqttSubscription> IncommingTopics { get; set; }
        //public String IncommingTopics { get; set; }

        [XmlAttribute("topic-out-alarm")]
        public String OutgoingAlarmBusTopic { get; set; }

        [XmlAttribute("topic-out-device")]
        public String OutgoingDeviceBusTopic { get; set; }

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
