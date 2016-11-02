using HomeMonitor.Generic.xml;
//using HomeMonitor.message;
using HomeMonitor.model;
using MemBus;
using MemBus.Subscribing;
//HEST using HomeMonitor.Notification.logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMonitor.model.sensor
{
    public abstract class SensorGeneric : Channel
    {
        public abstract int GetScore();


        public SensorGeneric(String thingId, ThingConfig thingConfig, ChannelConfig config, IBus bus)
            : base(thingId, thingConfig, config, bus)
        {
        }



    }
}
