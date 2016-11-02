using HomeMonitor.Generic.xml;
using HomeMonitor.Security.xml;
using HomeMonitror.Security.xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace HomeMonitror.Security.xml
{
    [Serializable]
    [XmlRoot("security")]
    public class SecurityConfig : GenericConfig//, IXmlSerializable
    {

        public SecurityConfig()
        {
        }

        
        [XmlArrayItem("mail", typeof(MailConfig))]
        [XmlArrayItem("sms", typeof(SmsConfig))]
        [XmlArray("notifications")]
        public List<NotificationItem> notifications { get; set; }

        
        [XmlArrayItem("zone", typeof(ZoneConfig))]
        [XmlArray("zones")]
        public List<ZoneConfig> Zones { get; set; }
        
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
