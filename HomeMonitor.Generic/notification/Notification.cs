using HomeMonitor.logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMonitor.notification
{
    public class Notification
    {
        public NotificationType Type { get; protected set; }
        public DateTime EventDateTime { get; private set; }
        public String Message { get; protected set; }

        public Notification(NotificationType type, String message)
        {
            EventDateTime = DateTime.Now;
            Type = type;
            Message = message;
        }

    }
}
    
