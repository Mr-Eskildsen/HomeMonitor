using HomeMonitor.Generic.xml;
using HomeMonitror.Security.xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;


namespace HomeMonitor.Security.xml
{

    [Serializable]

    [XmlRoot("zone")]
    public class ZoneConfig : GenericConfig
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlArrayItem("switch", typeof(AlarmSwitchConfig))]
        [XmlArrayItem("contact", typeof(AlarmContactSensorConfig))]
        [XmlArrayItem("perimeter", typeof(AlarmPerimeterSensorConfig))]
        [XmlArrayItem("pir", typeof(AlarmPirSensorConfig))]
        [XmlArray("devices")]
        public List<AlarmDeviceConfig> Devices { get; set; }


        public ZoneConfig()
        {
        }

        public override string GetId()
        {
            return Id;
        }

        public override string GetName()
        {
            return "zone";
        }
        
    }

}
