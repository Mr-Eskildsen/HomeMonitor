using HomeMonitor.Generic.xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMonitor.Generic.xml
{
    public abstract class NotificationItem : GenericConfig
    {

        public NotificationItem()
        {
            MinSensitivity = 6;
            MaxSensitivity = 10;
        }


    }
}
