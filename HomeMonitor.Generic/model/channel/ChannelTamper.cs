using HomeMonitor.Generic.xml;
using MemBus;
//HEST using HomeMonitor.Notification.logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMonitor.model.sensor
{
    public class ChannelTamper : ChannelContact
    {
        //HEST private static readonly log4net.ILog alarmLog = log4net.LogManager.GetLogger(typeof(AlarmExtensions));

        public ChannelTamper(String thingId, ThingConfig thingConfig,  ChannelConfig config, IBus bus)
            : base(thingId, thingConfig, config, bus)
        {
            //State = ContactStates.CLOSED.ToString();
        }
        
    }

}
