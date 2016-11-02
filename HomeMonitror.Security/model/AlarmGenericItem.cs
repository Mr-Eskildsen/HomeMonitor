using HomeMonitor.logger;
using HomeMonitor.message;
using HomeMonitor.notification;
using HomeMonitror.Security.events;
using log4net;
using MemBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMonitror.Security.model
{
    public abstract class AlarmGenericItem
    {
        protected IBus _bus = null;
        private static readonly ILog alarmLog = LogManager.GetLogger(typeof(AlarmExtensions));

        private List<AlarmGenericItem> arrChildren = new List<AlarmGenericItem>();
        public int Sensitivity { get; private set; }
        public bool Armed { get; private set; }

        public string InstanceId { get; private set; }


        
        protected AlarmGenericItem(IBus bus)
        {
            InstanceId = Guid.NewGuid().ToString();
            _bus = bus;
        }

        protected event EventHandler<SecurityAlarmAlertEventArgs> alertEventHandler;

        protected void RaiseAlert(string Category, string Message)
        {
            alarmLog.AlarmAlertFormat(Category, Message);
            NotificationFactory.NotifyAlert(Message);
        }


        protected void ClearChildren()
        {
            //item.alertEventHandler += OnRaiseAlert;
            arrChildren.Clear();
        }
        protected void AddChild(AlarmGenericItem item)
        {
            //item.alertEventHandler += OnRaiseAlert;
            arrChildren.Add(item);
        }

        public virtual void OnSystemArm(int sensitivity)
        { }

        public virtual void OnSystemDisarm()
        { }

        public virtual void OnSystemSensitivityChanged(int prevSensitivity, int newSensitivity)
        { }

        public void SystemArmed(int sensitivity)
        {
            
            foreach (AlarmGenericItem child in arrChildren)
            {
                child.SystemArmed(sensitivity);
            }
            
            Armed = true;
            Sensitivity = sensitivity;
            OnSystemArm(sensitivity);
        }

        public void SystemDisarmed()
        {
            foreach (AlarmGenericItem child in arrChildren)
            {
                child.SystemDisarmed();
            }

            Armed = false;
            Sensitivity = 0;
            OnSystemDisarm();
            
        }

        public void SystemSensitivityChanged(int prevSensitivity, int newSensitivity)
        {
            foreach (AlarmGenericItem child in arrChildren)
            {
                child.SystemSensitivityChanged(prevSensitivity, newSensitivity);
            }
            Armed = true;
            Sensitivity = newSensitivity;
            OnSystemSensitivityChanged(prevSensitivity, newSensitivity);
        }

        protected void Publish(IBusMessage message)
        {
            _bus.Publish(message);
        }

        protected void Subscribe(ISubscriber subscriber)
        {
            _bus.Subscribe(subscriber);
        }


    }
}
