using HomeMonitor.Generic.xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HomeMonitror.Security.xml
{
    
    public abstract class AlarmDeviceConfig : GenericConfig
    {
        [XmlAttribute("thing-id")]
        public string ThingId { get; set; }

        [XmlAttribute("channel-id")]
        public string ChannelId { get; set; }

        public AlarmDeviceConfig()
        {
            MinSensitivity = 2;
            MaxSensitivity = 10;
        }
    }
}
