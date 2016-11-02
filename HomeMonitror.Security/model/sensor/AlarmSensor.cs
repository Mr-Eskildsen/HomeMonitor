using HomeMonitor.events;
using HomeMonitor.logger;
using HomeMonitor.message;
using HomeMonitor.message.alert;
using HomeMonitor.model;
using HomeMonitor.Security;
using HomeMonitor.Security.xml;
using log4net;
using MemBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMonitror.Security.model.sensor
{
    public abstract class AlarmSensor : AlarmDevice
    {


        private static readonly ILog alarmLog = LogManager.GetLogger(typeof(AlarmExtensions));
        private static readonly ILog stateLog = LogManager.GetLogger(typeof(AlarmExtensions));

        
        public abstract int GetScore();
        public abstract Boolean isPerimeter();

        protected AlarmSensor(AlarmSensorConfig config, Zone zone, Channel ch, IBus bus)
            : base(config, zone, ch, bus)
        {
        }

        public override bool IsSensor() { return true; }


        public override void HandleEvent(object sender, HomeMonitorEventArgs args)
        {
            ChannelStateMessage msg = (ChannelStateMessage)(args.Message);
            //TODO:: log4net
            //Console.WriteLine("HandleEvent called for ThingId='{0}', ChannelId='{1}' - State changed from '{2}' to '{3}'", msg.ThingId, msg.ChannelId, msg.StatePrev, msg.StateNew);
            //int sen = Sensitivity;
            int _calculatedScore = GetScore();

            if (Disabled == true)
            {

                if (msg.StatePrev == null)
                {
                    alarmLog.AlarmDebugFormat(channel.Name, "Zone='{0}', Sensor='{1}': Sensor has been enabled, It was not initialized previously. (Prev value=null)", ZoneName, channel.UniqueId);
                }
                else
                {
                    alarmLog.AlarmInfoFormat(channel.Name, "Zone = '{0}', Sensor '{1}' has been enabled, It was disabled previously.", ZoneName, channel.UniqueId);
                }

                Disabled = false;
            }


            if (_calculatedScore == 0)
            {
            }
            else if ((Disabled == false) && (msg.StateNew =="OPEN"))
            {
                //PONY
                //RaiseAlert(channel.ThingId, channel.Id);
                
                Publish(new SecuritySensorAlertMessage(InstanceId, channel, msg.StatePrev, msg.StateNew));

            }
            else if (Disabled == false)
            {

            }
        }
    
    }
}
