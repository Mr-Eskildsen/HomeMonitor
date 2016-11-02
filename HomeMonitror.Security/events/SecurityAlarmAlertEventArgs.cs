using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMonitror.Security.events
{
    public class SecurityAlarmAlertEventArgs : EventArgs
    {
        //public EnumSystemEvent eventId { get; protected set; }
        //public GenericMessage Message { get; private set; }

        public string ThingId { get; private set; }
        public string ChannelId { get; private set; }

        public SecurityAlarmAlertEventArgs(string thingId, string channelId)
        {
            ThingId = thingId;
            ChannelId = channelId;
        }
    }
}

