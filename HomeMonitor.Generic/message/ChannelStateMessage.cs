using HomeMonitor.message;
using HomeMonitor.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMonitor.message
{
    public class ChannelStateMessage : SecurityStateMessage
    {

        public string ChannelId { get; private set; }
        public bool IsExternalState { get; private set; }

        public ChannelStateMessage(string instanceId, Channel channel, bool externalState, SwitchStates newState)
            : base(instanceId, ThingRegistry.CalculateUniqueChannelId(channel.ThingId, channel.Id), channel.ThingId, newState.ToString(), null)
        {
            Initialize(channel.Id, externalState);
        }


        public ChannelStateMessage(string instanceId, Channel channel, bool externalState, ContactStates newState)
            : base(instanceId, ThingRegistry.CalculateUniqueChannelId(channel.ThingId, channel.Id), channel.ThingId, newState.ToString(), null)
        {
            Initialize(channel.Id, externalState);
        }


        public ChannelStateMessage(string instanceId, Channel channel, bool externalState, string newState, string prevState = null)
            : base(instanceId, ThingRegistry.CalculateUniqueChannelId(channel.ThingId, channel.Id), channel.ThingId, newState, prevState)
        {
            Initialize(channel.Id, externalState);
        }


        private void Initialize(string channelId, bool externalState)
        {
            IsExternalState = externalState;
            ChannelId = channelId;

        }
    }
}
