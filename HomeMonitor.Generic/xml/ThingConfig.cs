using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace HomeMonitor.Generic.xml
{
    public class ThingConfig : GenericConfig
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("description")]
        public string Description { get; set; }

        [XmlAttribute("group")]
        public string Group { get; set; }
        


        [XmlElement("channel", typeof(ChannelConfig))]
        public List<ChannelConfig> Channels { get; set; }

        public override string GetId()
        {
            return Id; 
        }

        public override string GetName()
        {
            return Name;
        }

        
    }
}
