using HomeMonitor.Generic.xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HomeMonitror.Security.xml
{
    public class AlarmSwitchConfig : AlarmDeviceConfig
    {

        public AlarmSwitchConfig()
        {
            Timeout = 120;
        }
        /*
        [XmlAttribute("thing-id")]
        public string ThingId { get; set; }

        [XmlAttribute("channel-id")]
        public string ChannelId
        {
            get; set;
        }
*/
        [XmlAttribute("timeout")]
        public int Timeout { get; set; }


        public override string GetId()
        {
            return "switch";
        }

    
        public override string GetName()
        {
            throw new NotImplementedException();
        }
    }
}
