using HomeMonitor.Generic.xml;
using HomeMonitror.Security.xml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace HomeMonitor.Security.xml
{


    //[Serializable]
    //[XmlRoot("sensor")]
    public abstract class AlarmSensorConfig : AlarmDeviceConfig
    {

        public AlarmSensorConfig()
        {
        }

        public override string GetName()
        {
            throw new NotImplementedException();
        }

        
    }

    [Serializable]
    [XmlRoot("pir")]
    public class AlarmPirSensorConfig : AlarmSensorConfig 
    {
        //[XmlAttribute("thing-id")]
        //public string ThingId { get; set; }

        //[XmlAttribute("channel-id")]
        //public string ChannelId { get; set; }

        public AlarmPirSensorConfig()
        {
        }

        public override string GetId() { 
            return "pir";
        }
    }



    [Serializable]
    [XmlRoot("perimeter")]
    public class AlarmPerimeterSensorConfig : AlarmSensorConfig
    {
        //[XmlAttribute("thing-id")]
        //public string ThingId { get; set; }

        //[XmlAttribute("channel-id")]
        //public string ChannelId { get; set; }



        public AlarmPerimeterSensorConfig()
        {
            MinSensitivity = 5;
            MaxSensitivity = 10;
        }

        public override string GetId()
        {
            return "perimeter";
        }
    }

    [Serializable]
    [XmlRoot("contact")]
    public class AlarmContactSensorConfig : AlarmSensorConfig
    {
        //[XmlAttribute("thing-id")]
        //public string ThingId { get; set; }

        //[XmlAttribute("channel-id")]
        //public string ChannelId { get; set; }



        public AlarmContactSensorConfig()
        {
            MinSensitivity = 5;
            MaxSensitivity = 10;
        }

        public override string GetId()
        {
            return "contact";
        }
    }
}
