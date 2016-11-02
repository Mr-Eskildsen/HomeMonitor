using log4net.Appender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Core;


namespace HomeMonitor.logger
{
    public class MailAppender : AppenderSkeleton
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
            string str = (string)loggingEvent.MessageObject;
                switch (loggingEvent.Level.Value) 
            {
                case AlarmExtensions.AlarmUrgentLevelValue:
                case AlarmExtensions.AlarmAlertLevelValue:
                    //string str = (string)loggingEvent.MessageObject;
                    //HEST NotificationFactory.NotifyAlert(str);
                    break;
                default:
                   // throw new NotImplementedException();
                    break;
            }
            
        }
    }
}
