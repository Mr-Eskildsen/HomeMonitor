using System;

namespace HomeMonitor.interfaces
{
    public interface INotificationGeneric
    {

        void notifyAlert(String Message);
        void notifyWarning(String Message);
        void notifyInfo(String Message);
    }
}
