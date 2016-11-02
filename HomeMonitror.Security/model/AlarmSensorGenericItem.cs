using HomeMonitor.events;
using HomeMonitor.model;
using HomeMonitror.Security.events;
using MemBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMonitror.Security.model
{
    public abstract class AlarmSensorGenericItem : AlarmGenericItem
    {
        public Channel channel { get; private set; }

        public AlarmSensorGenericItem(Channel ch, IBus bus)
            : base(bus)
        {
            channel = ch;
            channel.SubscribeEventHandler(HandleEvent);
        }




        public abstract void HandleEvent(object sender, HomeMonitorEventArgs args);

    }
}
