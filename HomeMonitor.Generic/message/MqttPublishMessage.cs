using HomeMonitor.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMonitor.message
{
    public enum MqttMessageDirection
    {
        incomming=0,
        outgoing=1
    };

    public enum MqttPublishChannel
    {
        alarm = 0,
        device = 1
    };

    public class MqttPublishMessage : GenericMessage
    {
        public MqttPublishChannel PublishChannel { get; private set; }
        public string Subtopic { get; set;  }
        public string Payload { get; private set; }

        public MqttPublishMessage(MqttPublishChannel publishChannel, MqttMessageDirection direction, string subtopic, string payload)
            : base("", "")
        {
            if (direction==MqttMessageDirection.incomming)
                Subtopic = subtopic;
            else
                Subtopic = subtopic;
            Payload = payload;
            PublishChannel = publishChannel;
        }

        public MqttPublishMessage(MqttPublishChannel publishChannel, Channel ch, string payload) 
            : base(ch.UniqueId, ch.UniqueId)
        {
            Subtopic = ch.Subtopic;
            Payload = payload;
            PublishChannel = publishChannel;

        }
    }
}
