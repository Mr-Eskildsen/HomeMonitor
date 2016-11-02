using HomeMonitor.model;
using HomeMonitror.Security.xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeMonitor.events;
using System.Threading;
using HomeMonitor.Security;
using HomeMonitor.message;
using MemBus;
using log4net;
using HomeMonitor.logger;
using HomeMonitor.model.device;

namespace HomeMonitror.Security.model.sensor
{
    public class AlarmSwitchTimerDevice : AlarmSwitchDevice
    {
        private static readonly ILog alarmLog = LogManager.GetLogger(typeof(AlarmExtensions));
        private static readonly ILog stateLog = LogManager.GetLogger(typeof(AlarmExtensions));
        protected int Timeout { get; set; }

        protected AlarmSwitchTimerDevice(AlarmSwitchConfig config, Zone zone, Channel ch, MemBus.IBus bus)
            : base (config, zone, ch, bus)
        {
            Timeout = config.Timeout;
        }


        public static AlarmDevice CreateDevice(AlarmSwitchConfig config, Zone zone, Channel ch, MemBus.IBus bus)
        {
            return (AlarmDevice)new AlarmSwitchTimerDevice(config, zone, ch, bus);
        }



        public override void HandleEvent(object sender, HomeMonitorEventArgs args)
        {
            //alarmLog.AlarmInfoFormat(channel.Name, "Halløjsovs");
        }




        public override bool TurnOn()
        {
            Task.Run(() =>
            {
            DeviceSwitch curDevice = null;
                try
                {
                    curDevice = (DeviceSwitch)channel;
                    if (curDevice.TurnOn())
                    {
                        Thread.Sleep(Timeout * 1000);
                        curDevice.TurnOff();
                    }
                }
                catch(Exception ex)
                {

                }
                
            });
            return true;

        }

        public override bool TurnOff()
        {
            return true;
        }

            new protected void Subscribe(ISubscriber subscriber)
        {
            _bus.Subscribe(subscriber);
        }

        
    }
}
