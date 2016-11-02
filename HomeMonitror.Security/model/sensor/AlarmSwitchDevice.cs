using HomeMonitor.model;
using HomeMonitror.Security.xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeMonitor.Security;
using MemBus;
using HomeMonitor.message;

namespace HomeMonitror.Security.model.sensor
{
    public abstract class AlarmSwitchDevice : AlarmDevice
    {
        private AlarmSwitchConfig config;
        private Zone zone;
        private Channel ch;
        private IBus bus;

        protected AlarmSwitchDevice(AlarmSwitchConfig config, Zone zone, Channel ch, MemBus.IBus bus)
            : base (config, zone, ch, bus)
        {

        }
        public abstract bool TurnOn();
        public abstract bool TurnOff();
        
        new protected void Publish(IBusMessage message)
        {
            _bus.Publish(message);
        }

    }
}
