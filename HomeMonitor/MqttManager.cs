using HomeMonitor.Generic.xml;
//ÆSEL using HomeMonitor.Generic.interfaces;
using HomeMonitor.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using HomeMonitor.message;
using MemBus;
using HomeMonitor.Generic.interfaces;
using HomeMonitor.interfaces;
using log4net;
using HomeMonitor.logger;

namespace HomeMonitor.Mqtt
{
    public class MqttManager : IMqttManager
    {

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ILog alarmLog = LogManager.GetLogger(typeof(AlarmExtensions));
        private static readonly ILog stateLog = LogManager.GetLogger(typeof(AlarmExtensions));


        IBus _bus = null;
        private static MqttManager _instance = null;
        private List<IManager> managers = new List<IManager>();

        public static MqttManager Instance { get { return _instance; } }



        private MqttClient client = null;
        private List<string> _baseIncommingTopics = new List<string>();

        //private string _baseIncommingTopic = string.Empty;
        private string _baseOutgoingAlarmTopic = string.Empty;
        private string _baseOutgoingDeviceTopic = string.Empty;

        private MqttManager(IBus bus, MqttConfig mqttCfg)
        {
            _instance = this;
            _bus = bus;

            foreach(MqttSubscription incomming in mqttCfg.IncommingTopics)
            {
                _baseIncommingTopics.Add(incomming.Topic);
            }
            //_baseIncommingTopic = VerifyTopic(mqttCfg.IncommingTopic);
            _baseOutgoingAlarmTopic = VerifyTopic(mqttCfg.OutgoingAlarmBusTopic);
            _baseOutgoingDeviceTopic = VerifyTopic(mqttCfg.OutgoingDeviceBusTopic);

            // create client instance 
            client = new MqttClient(mqttCfg.host);

            // register to message received 
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;


            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            // subscribe to the topic "/home/temperature" with QoS 2 
/*            if (!string.IsNullOrEmpty(_baseIncommingTopic))
            {
                MqttSubscribe(_baseIncommingTopic);
            }
  */
            foreach(string topic in _baseIncommingTopics)
            {
                MqttSubscribe(topic);
            }

            _bus.Subscribe<MqttPublishMessage>(msg => PublishToMqttBus(msg.PublishChannel, msg.Subtopic, msg.Payload));
        }


        public static MqttManager Create(IBus bus,  MqttConfig config)
        {
            return new MqttManager(bus, config);
        }



        private string VerifyTopic(String topic)
        {
            if ((topic == null) || (topic==string.Empty))
            {
                return string.Empty;
            }
            else if (topic.EndsWith("/"))
            {
                return topic;
            }
            return topic + "/";
        }


        protected void MqttSubscribe(String topic)
        {
            Task.Run(() =>
            {
                log.DebugFormat("Subscribing to Topic='{0}'", topic);
                client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
            });
        }


        protected void PublishToMqttBus(MqttPublishChannel channel, string subTopic, String Payload)
        {
            string baseTopic = string.Empty;

            switch (channel)
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
                log.ErrorFormat("MQTT message could not be send, because no valid BaseTopic was specified. Topic='{0}', Payload='{1}'", baseTopic, Payload);
                return;
            }
            string topic = baseTopic + subTopic;

            log.DebugFormat("MQTT sending message. Topic='{0}', Payload='{1}'", topic, Payload);
            client.Publish(topic, Encoding.ASCII.GetBytes(Payload), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
        }


        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string Payload = Encoding.Default.GetString(e.Message);
            string Topic = e.Topic;
            string[] arr = e.Topic.Split('/');

            //TODO:: Make detection of thingId and Channel ID more dynamic
            String thingId = arr[arr.Length - 2];
            String channelId = arr[arr.Length - 1];

            SecurityStateMessage message = null;

            log.InfoFormat("MQTT Message received. Topic='{0}' Payload='{1}' ThingId='{2}' ChannelId='{3}'", Topic, Payload, thingId, channelId );
            //System Command!
            if (thingId == "system") /*&& (channelId == "command")*/
            {
                string[] tokens = Payload.Split(':');
                string command = tokens[0];
                int sensitivity;

                switch (command)
                {
                    case "arm":
                        sensitivity = Convert.ToInt32(tokens[1]);
                        //Actually it is disarmed (sensitivity = 0)
                        if (sensitivity == 0)
                        {
                            message = new SecuritySystemStateMessage(AlarmSystemCommand.disarm, string.Empty, string.Empty);
                        }
                        else
                        {
                            message = new SecuritySystemStateMessage(AlarmSystemCommand.arm, sensitivity.ToString(), string.Empty);
                        }
                        break;
                    case "disarm":
                        message = new SecuritySystemStateMessage(AlarmSystemCommand.disarm, string.Empty, string.Empty);
                        //SecurityAlarmMgr.Instance.DisarmSystem();
                        break;
                    case "sensitivity":
                        sensitivity = Convert.ToInt32(tokens[1]);
                        message = new SecuritySystemStateMessage(AlarmSystemCommand.sensitivity, sensitivity.ToString(), string.Empty);
                        break;
                    default:
                        //throw new NotImplementedException();
                        log.InfoFormat("Unknown MQTT message arrived Message='{0}', Payload='{1}'", e.Topic, e.Message);
                        break;
                }



            }
            else
            {
                Thing thing = ThingRegistry.GetThing(thingId);


                //Thing doesn't exist in Registry
                if (thing == null)
                {
                    log.WarnFormat("ThingId='{0}' not found for MQTT Message Topic='{1}'", thingId, e.Topic);
                    return;
                }

                Channel ch = thing.getChannel(channelId);
                if (ch == null)
                {
                    log.WarnFormat("ThingId='{0}' doen't have Channel '{1}' (MQTT Message Topic='{2}' Payload='{3}'", thingId, channelId, e.Topic, e.Message);
                    return;
                }

                //Channel state has changed -> Publish
                message = new ChannelStateReceived(string.Empty, ch, true, Payload);
            }
        

            if (message != null) {
                Instance.Publish(message);
            }

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
