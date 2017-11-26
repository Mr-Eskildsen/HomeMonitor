//HEST using SecurityMonitor.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeMonitor.Generic.xml;
using HomeMonitor.model;
using HomeMonitor.Security.xml;
using HomeMonitror.Security.model.sensor;
using HomeMonitor.logger;
using HomeMonitror.Security.model;
using MemBus;
using HomeMonitor.message.alert;
using HomeMonitor.message;
using MemBus.Subscribing;
using log4net;
using HomeMonitror.Security.xml;
using HomeMonitor.model.channel;

namespace HomeMonitor.Security
{
    public class AlarmZone : AlarmGenericItem
    {
        //private static string CHANNEL_STATE = "state";
        private static string CHANNEL_ALARM = "alarm";
        private static string CHANNEL_STATUS = "status";
        private static string THING_GROUP_ALARM = "alarm";

        private static readonly log4net.ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly log4net.ILog alarmLog = LogManager.GetLogger(typeof(AlarmExtensions));


        public string Id { get; private set; }
        public string Name { get; private set; }

        
        Dictionary<String, AlarmSensor> sensors = new Dictionary<String, AlarmSensor>();
        Dictionary<String, AlarmSwitchDevice> devices = new Dictionary<String, AlarmSwitchDevice>();


        /*
         * AlarmState   Switch
         * Status       Text
         * LastAlarm    DateTime
         */ 

        public AlarmZone(ZoneConfig config, IBus bus)
            : base(bus)
        {
            
            bus.Subscribe<SecuritySensorAlertMessage>(msg => SensorAlertEvent(msg), new ShapeToFilter<SecuritySensorAlertMessage>(alertMsg => (sensors.Where(x => ((x.Value.isActive()) && (x.Value.InstanceId == alertMsg.InstanceId))).Count() > 0)));

            
            Id = config.Id;
            Name = config.Name;

            string thingId = config.GetId();
            string thingName = config.GetName();
            string thingDescription = String.Empty;
            string thingGroup = THING_GROUP_ALARM;
            
            
            //Create Thing that represents a Zone
            Thing alarmZoneThing = ThingRegistry.CreateThing(thingId, thingName, thingDescription);

            //TODO:: Move creation to Channel.Create method
            alarmZoneThing.addChannel(new ChannelSwitch(thingId, thingGroup, CHANNEL_ALARM, ChannelType.Switch, bus));
            alarmZoneThing.addChannel(new ChannelText(thingId, thingGroup, CHANNEL_STATUS, ChannelType.Text, bus));
            alarmZoneThing.addChannel(new ChannelSwitch(thingId, thingGroup, CHANNEL_STATUS, ChannelType.DateTime, bus));


            

            //Load sensors and map to channels
            foreach (AlarmDeviceConfig deviceConfig in config.Devices) {
                Channel channel = ThingRegistry.GetChannel(deviceConfig.ThingId, deviceConfig.ChannelId);

                if (channel == null)
                {
                    //TODO:: HANDLE IF CHannel not defined!
                    log.ErrorFormat("Error occurred when creating Security Zone '{0}'. The Thing'{1}:{2}' could not be found in the Thing Registry", Id, deviceConfig.ThingId, deviceConfig.ChannelId);
                }
                else
                {

                    AlarmDevice curDevice = AlarmDevice.Create(deviceConfig, this, channel, bus);
                    AddChild(curDevice);
                    if (curDevice.IsSensor())
                        sensors.Add(channel.UniqueId, (AlarmSensor)curDevice);
                    else
                        devices.Add(channel.UniqueId, (AlarmSwitchDevice)curDevice);
                }
                
            }
        }

        private void SensorAlertEvent(SecuritySensorAlertMessage msg)
        {
            string message = string.Format("Zone '{0}' was alerted by sensor {1}", Name, msg.channel.UniqueId);
            alarmLog.AlarmInfoFormat(Id, message);

            List<AlarmSwitchDevice> deviceList = devices.Where(device => (device.Value.isActive())).Select(device => device.Value).ToList();
            foreach(AlarmSwitchDevice device in deviceList)
            {
                device.TurnOn();
            }
        }

        public int GetScore()
        {
            return GetPerimeterScore();
        }


        public int GetPerimeterScore()
        {
            int maxValue = 0;
            //var myList = channels.Where(kvp => kvp.Value.GetScore() == 100);


            int count = sensors.Where(x => (x.Value.GetScore() != 0) && (x.Value.isPerimeter() == true) && (x.Value.isActive())).Select(x => x.Key).Count();
            
            if (count!=0)
                maxValue = sensors.Where(x => (x.Value.GetScore() != 0) && (x.Value.isPerimeter() == true) && (x.Value.isActive())).Select(x => x.Value.GetScore()).Max();
            else
                maxValue = 0;
            
            

            //            List<String> activatedPerimeterSensors = channels.Where(x => (x.Value.GetScore() != 0) && (x.Value.isPerimeter == true)).Sum(o => o.Value.GetScore()) / ;
            //.Select(x => x.Key)
            //.ToList();
                
            //activatedPerimeterSensors.AsEnumerable().Sum(o => o.Get);
            return maxValue;
        }

       
    }
}
