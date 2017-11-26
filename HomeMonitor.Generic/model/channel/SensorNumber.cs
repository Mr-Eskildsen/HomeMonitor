using HomeMonitor.Generic.xml;
using MemBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMonitor.model.sensor
{
    public abstract class ChannelNumber : Channel
    {
        //HEST private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ChannelNumber(String thingId, ThingConfig thingConfig, ChannelConfig config, IBus bus)
                : base(thingId, thingConfig, config, bus)
        {
            //State = "0";
        }
        
    }
}
