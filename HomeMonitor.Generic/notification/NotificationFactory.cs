using HomeMonitor.Generic.xml;
using HomeMonitor.interfaces;

using HomeMonitror.Security.xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemBus;

namespace HomeMonitor.notification
{
    public class NotificationFactory
    {
        private List<INotificationGeneric> subscribers = new List<INotificationGeneric>();

        private static readonly Lazy<NotificationFactory> _instance = new Lazy<NotificationFactory>(() => new NotificationFactory());

        protected bool IsInitialized { get; set; }

        public static NotificationFactory Instance
        {
            get
            {
                if (!_instance.Value.IsInitialized)
                    return null;
                return _instance.Value;
            }
        }

        public static void Create(IBus bus, List<NotificationItem> items)
        {
            _instance.Value._Create(bus, items);
        }

        public static void NotifyAlert(String Message)
        {
            if (Instance!=null)
                Instance.notifyAlert(Message);
        }

        public static void NotifyWarning(String Message)
        {
            if (Instance != null)
                Instance.notifyWarning(Message);
        }
        public static void NotifyInfo(String Message)
        {
            if (Instance != null)
                Instance.notifyInfo(Message);
        }

        protected void _Create(IBus bus, List<NotificationItem> items)
        {

            foreach (NotificationItem item in items)
            {
                switch (item.GetName())
                {
                    case "mail":
                        subscribers.Add(new MailNotification(bus, (MailConfig)item));
                        break;
                    case "sms":
                        subscribers.Add(new SmsNotification());
                        break;
/*                    case "file":
                        subscribers.Add(new FileLogger());
                        break;
*/
                }

            }
            IsInitialized = true;
        }


        protected void notifyAlert(String Message)
        {
            foreach (INotificationGeneric item in subscribers)
            {
                item.notifyAlert(Message);
            }
        }

        protected void notifyInfo(String Message)
        {
            foreach (INotificationGeneric item in subscribers)
            {
                item.notifyInfo(Message);

            }
        }

        protected void notifyWarning(String Message)
        {
            foreach (INotificationGeneric item in subscribers)
            {
                item.notifyWarning(Message);

            }
        }
    }
}
