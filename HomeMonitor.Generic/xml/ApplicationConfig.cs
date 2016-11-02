using HomeMonitor.Generic.xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HomeMonitor.xml
{
    [Serializable]
    [XmlRoot("application")]
    public class ApplicationConfig : GenericConfig
    {

        public ApplicationConfig()
        {
            //notifications = new List<NotificationItem>();
            //Zones = new List<ZoneConfig>();
        }

        [XmlElement("mqtt", typeof(MqttConfig))]
        public MqttConfig mqttConfig { get; set; }

        /*
        [XmlArrayItem("file", typeof(FileLoggerConfig))]
        [XmlArrayItem("mail", typeof(MailConfig))]
        [XmlArrayItem("sms", typeof(SmsConfig))]
        [XmlArray("notifications")]
        public List<NotificationItem> notifications { get; set; }

        //public DateTime? PurchaseDate { get; set; }

        // for each subclass of ShoppingItem you need
        // to specify the correspondent XML element to generate
        //[XmlArrayItem("cd", typeof(CD))]
        //[XmlArrayItem("book", typeof(Book))]
        [XmlArrayItem("zone", typeof(ZoneConfig))]
        [XmlArray("zones")]
        //        [XmlIgnore]
        public List<ZoneConfig> Zones { get; set; }
        */
        public override string GetId()
        {
            //TODO:: HEST
            throw new NotImplementedException();
        }

        public override string GetName()
        {
            //TODO:: HEST
            throw new NotImplementedException();
        }
        /*
        public void ReadXml(XmlReader reader)
        {

            if (reader.MoveToContent() == XmlNodeType.Element && reader.LocalName == "security")
            {

                String Test = reader.ReadInnerXml();


                if (reader.MoveToContent() == XmlNodeType.Element && reader.LocalName == "notification")
                {
                    reader.Read(); // Skip ahead to next node
                    while (reader.MoveToContent() == XmlNodeType.Element && Type.GetType(reader.LocalName).IsSubclassOf(typeof(NotificationItem)))
                    {
                        String Test1 = reader.ReadInnerXml();
                        //Type NotificationType = (NotificationItem)Type.GetType(reader.LocalName);
                        //Animal a = AnimalType.Assembly.CreateInstance(reader.LocalName);
                        //a.ReadXml(reader);
                        //Animals.Add(a.Key, a);
                        reader.Read(); // Skip to next animal (if there is one)
                    }
                }

                //                _title = reader["Title"];
                //              _start = DateTime.FromBinary(Int64.Parse(reader["Start"]));
                //            _stop = DateTime.FromBinary(Int64.Parse(reader["Stop"]));
                reader.Read();
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            throw new NotImplementedException();
        }*/
    }

}

