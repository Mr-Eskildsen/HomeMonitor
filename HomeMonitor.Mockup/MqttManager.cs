using HomeMonitor.Generic.xml;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HomeMonitor.message;
using MemBus;
using HomeMonitor.Generic.interfaces;
using HomeMonitor.interfaces;
using HomeMonitor.Mockup;
using HomeMonitor.xml;

namespace HomeMonitor.Mqtt
{

    public enum MqttQosLevel
    {
         QOS_LEVEL_AT_LEAST_ONCE = 1,
         QOS_LEVEL_AT_MOST_ONCE = 0,
         QOS_LEVEL_EXACTLY_ONCE = 2
}
    public class MqttManager : IMqttManager
    {
        IBus _bus = null;
        private static MqttManager _instance = null;
        private List<IManager> managers = new List<IManager>();

        public static MqttManager Instance { get { return _instance; } }



        private MqttClientMockup client = null;

        private List<string> _baseIncommingTopics = new List<string>();
        //private string _baseIncommingTopic = string.Empty;
        private string _baseOutgoingAlarmTopic = string.Empty;
        private string _baseOutgoingDeviceTopic = string.Empty;


        private MqttManager(IBus bus, MqttConfig mqttCfg)
        {
            _instance = this;
            _bus = bus;
            

            foreach (MqttSubscription incomming in mqttCfg.IncommingTopics)
            {
                _baseIncommingTopics.Add(incomming.Topic);
            }
            //_baseIncommingTopic = VerifyTopic(mqttCfg.IncommingTopic);


            _baseOutgoingAlarmTopic = VerifyTopic(mqttCfg.OutgoingAlarmBusTopic);
            _baseOutgoingDeviceTopic = VerifyTopic(mqttCfg.OutgoingDeviceBusTopic);


            // create client instance 
            //UNDULAT client = new MqttClientMockup(mqttCfg.host);

            // register to message received 
            //UNDULAT client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;


            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            // subscribe to the topic "/home/temperature" with QoS 2 
            /*            if (!string.IsNullOrEmpty(_baseIncommingTopic))
                        {
                            MqttSubscribe(_baseIncommingTopic);
                        }
              */
            foreach (string topic in _baseIncommingTopics)
            {
                MqttSubscribe(topic);
            }


            _bus.Subscribe<MqttPublishMessage>(msg => PublishToMqttBus( msg.PublishChannel, msg.Subtopic, msg.Payload));
        }


        public static MqttManager Create(IBus bus,  MqttConfig config)
        {
            return new MqttManager(bus, config);
        }


        protected void MqttSubscribe(String topic)
        {
            Task.Run(() =>
            {
                client.Subscribe(new string[] { topic }, MqttQosLevel.QOS_LEVEL_AT_LEAST_ONCE);
            });
        }


        private string VerifyTopic(string topic)
        {
            if (topic.EndsWith("/"))
            {
                return topic;
            }
            return topic + "/";
        }

                

        protected void PublishToMqttBus( MqttPublishChannel channel, string subTopic, String Payload)
        {
            string baseTopic = string.Empty;

            switch(channel)
            {
                case MqttPublishChannel.alarm:
                    baseTopic = _baseOutgoingAlarmTopic;
                    break;

                case MqttPublishChannel.device:
                    baseTopic = _baseOutgoingDeviceTopic;
                    break;
            }
            if (baseTopic == string.Empty)
            {
                return;
            }
            string topic = baseTopic +  subTopic;
            client.Publish(topic, Encoding.ASCII.GetBytes(Payload), MqttQosLevel.QOS_LEVEL_AT_LEAST_ONCE, true);
        }

        


        protected void Publish(IBusMessage message)
        {
            _bus.Publish(message);
        }

        protected void Subscribe(ISubscriber subscriber)
        {
            _bus.Subscribe(subscriber);
            
        }
    }
}
