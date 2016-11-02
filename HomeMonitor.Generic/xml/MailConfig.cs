
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using System.ComponentModel;
using HomeMonitor.logger;
using HomeMonitor.Generic.xml;

namespace HomeMonitror.Security.xml
{
    [Serializable]
    [XmlRoot("mail")]
    public class MailConfig : NotificationItem
    {
        public MailConfig()
        {
            Subject = "HomeMonitor Notification";
        }

        [XmlAttribute("host")]
        public String Host { get; set; }

        [XmlAttribute("port")]
        public int Port { get; set; }


        [XmlAttribute("user-id")]
        public String UserId { get; set; }

        [XmlAttribute("password")]
        public String Password { get; set; }

        [XmlAttribute("enable-ssl")]
        public bool enableSSL { get; set; }

        [XmlAttribute("from")]
        public String From { get; set; }

        [XmlElement("to")]
        public List<MailToConfig> Receivers { get; set; }

        [XmlAttribute("subject")]
        public String Subject { get; set; }


        [XmlIgnore]
        public NotificationType levelMin { get; set; }

        [XmlIgnore]
        public NotificationType levelMax { get; set; }


        //[DefaultValue(NotificationType.Alert)]
        [XmlAttribute("levelMin")]
        public string LevelMinAsString
        {
            get { return levelMin.ToString(); }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    levelMin = NotificationType.Info;
                }
                else
                {
                    levelMin = (NotificationType)Enum.Parse(typeof(NotificationType), value);
                }
            }
        }

        //[DefaultValue(NotificationType.Alert)]
        [XmlAttribute("levelMax")]
        public string LevelMaxAsString
        {
            get { return levelMax.ToString(); }
            set
            {
                
                if (string.IsNullOrEmpty(value))
                {
                    levelMax = NotificationType.Alert;
                }
                else
                {
                    levelMax = (NotificationType)Enum.Parse(typeof(NotificationType), value);
                }
            }
        }


        public override string GetId()
        {
            throw new NotImplementedException();
        }

        public override string GetName()
        {
            return "mail";
        }

    }
}
