using HomeMonitor.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMonitor.message
{
    
    public class ChannelStatePublish : ChannelStateMessage
    {
        public ChannelStatePublish(string instanceId, Channel channel, bool externalState, SwitchStates newState)
            : base(instanceId, channel, externalState, newState)
        {
        }


        public ChannelStatePublish(string instanceId, Channel channel, bool externalState, ContactStates newState)
            : base(instanceId, channel, externalState, newState)
        {

        }


        public ChannelStatePublish(string instanceId, Channel channel, bool externalState, string newState, string prevState = null)
            : base(instanceId, channel, externalState, newState, prevState)
        {
        }
    }
}
