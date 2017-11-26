using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeMonitor.message;
using HomeMonitor.Generic.xml;
using MemBus;

namespace HomeMonitor.model.channel
{
    class ChannelText : Channel
    {
        //HEST private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ChannelText(String thingId, ThingConfig thingConfig, ChannelConfig config, IBus bus)
                : base(thingId, thingConfig, config, bus)
        {
            //State = "0";
        }

        public override bool updateState(ChannelStateMessage csm)
        {
            throw new NotImplementedException();
        }
    }
}
