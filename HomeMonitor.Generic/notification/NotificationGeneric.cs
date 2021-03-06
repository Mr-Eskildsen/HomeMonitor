﻿using HomeMonitor.interfaces;
using HomeMonitor.logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMonitor.notification
{
    public abstract class NotificationGeneric : INotificationGeneric
    {
        public NotificationType MinLevel { get; protected set; }
        public NotificationType MaxLevel { get; protected set; }

        public abstract void notifyAlert(string Message);
        public abstract void notifyWarning(string Message);
        public abstract void notifyInfo(string Message);
    }
}
