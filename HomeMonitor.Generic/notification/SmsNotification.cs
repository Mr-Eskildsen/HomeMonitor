using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMonitor.notification
{
    public class SmsNotification : NotificationGeneric
    {
        public override void notifyAlert(string Message)
        {
            throw new NotImplementedException();
        }

        public override void notifyInfo(string Message)
        {
            throw new NotImplementedException();
        }

        public override void notifyWarning(string Message)
        {
            throw new NotImplementedException();
        }
    }
}
