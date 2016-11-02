using HomeMonitor.Generic.xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HomeMonitror.Security.xml
{
    [Serializable]
    [XmlRoot("sms")]
    public class SmsConfig : NotificationItem
    {
        public override string GetId()
        {
            throw new NotImplementedException();
        }

        public override string GetName()
        {
            return "sms";
        }

    }
}
