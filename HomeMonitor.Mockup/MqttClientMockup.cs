using HomeMonitor.Mqtt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMonitor.Mockup
{
    public class MqttClientMockup
    {

        // current message identifier generated
        private ushort messageIdCounter = 0;

        
        private ushort GetMessageId()
        {
            // if 0 or max UInt16, it becomes 1 (first valid messageId)
            this.messageIdCounter = ((this.messageIdCounter % UInt16.MaxValue) != 0) ? (ushort)(this.messageIdCounter + 1) : (ushort)1;
            return this.messageIdCounter;
        }


        /*
        public event ConnectionClosedEventHandler ConnectionClosed;
        public event MqttMsgPublishedEventHandler MqttMsgPublished;
        public event MqttMsgPublishEventHandler MqttMsgPublishReceived;
        public event MqttMsgSubscribedEventHandler MqttMsgSubscribed;
        public event MqttMsgUnsubscribedEventHandler MqttMsgUnsubscribed;
        */
        public byte Connect(string clientId)
        {
            return 1;
        }
        /*        public byte Connect(string clientId, string username, string password);
                public byte Connect(string clientId, string username, string password, bool cleanSession, ushort keepAlivePeriod);
                public byte Connect(string clientId, string username, string password, bool willRetain, byte willQosLevel, bool willFlag, string willTopic, string willMessage, bool cleanSession, ushort keepAlivePeriod);
                public void Disconnect();
          */
        public ushort Publish(string topic, byte[] message)
        {
            return GetMessageId();
        }

        public ushort Publish(string topic, byte[] message, MqttQosLevel qosLevel, bool retain)
        {
            return GetMessageId();
        }

        public ushort Subscribe(string[] topics, MqttQosLevel qosLevels)
        {
            return GetMessageId();
        }

        public ushort Unsubscribe(string[] topics)
        {
            return GetMessageId();
        }
        /*
        public delegate void ConnectionClosedEventHandler(object sender, EventArgs e);
        public delegate void MqttMsgPublishedEventHandler(object sender, MqttMsgPublishedEventArgs e);
        public delegate void MqttMsgPublishEventHandler(object sender, MqttMsgPublishEventArgs e);
        public delegate void MqttMsgSubscribedEventHandler(object sender, MqttMsgSubscribedEventArgs e);
        public delegate void MqttMsgUnsubscribedEventHandler(object sender, MqttMsgUnsubscribedEventArgs e);
        */

        
    }
}
