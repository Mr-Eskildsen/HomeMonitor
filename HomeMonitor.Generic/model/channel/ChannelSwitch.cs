using HomeMonitor.Generic.xml;
using HomeMonitor.logger;
using HomeMonitor.message;
using log4net;
using MemBus;
using System;

namespace HomeMonitor.model.channel
{
    public class ChannelSwitch : Channel
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ILog alarmLog = LogManager.GetLogger(typeof(AlarmExtensions));
        private static readonly ILog stateLog = LogManager.GetLogger(typeof(AlarmExtensions)); //= LogManager.GetLogger("StateLogger");

        public ChannelSwitch(String thingId, string thingGroup, string channelId, ChannelType channelType, IBus bus)
            : base(thingId, thingGroup, channelId, channelType, bus)
        {

        }

        public override bool updateState(ChannelStateMessage csm)
        {
            throw new System.NotImplementedException();
        }
    }
}
