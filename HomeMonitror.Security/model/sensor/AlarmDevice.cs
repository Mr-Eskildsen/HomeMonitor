using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeMonitor.model;
using HomeMonitor.Security.xml;
using HomeMonitor.events;
using HomeMonitor.message;
using log4net;
using HomeMonitor.logger;
using MemBus;
using HomeMonitor.message.alert;
using HomeMonitror.Security.xml;
using HomeMonitor.Security;

namespace HomeMonitror.Security.model.sensor
{
    public abstract class AlarmDevice : AlarmSensorGenericItem
    {

        private static readonly ILog alarmLog = LogManager.GetLogger(typeof(AlarmExtensions));
        private static readonly ILog stateLog = LogManager.GetLogger(typeof(AlarmExtensions));

        public bool Disabled { get; protected set; }
        public int MinSensitivity { get; protected set; }
        public int MaxSensitivity { get; protected set; }
        public string ZoneName { get; protected set; }

        public virtual bool IsSensor() { return false; }

        protected AlarmDevice(AlarmDeviceConfig config, Zone zone, Channel ch, IBus bus)
            : base(ch, bus)
        {
            Disabled = false;
            MinSensitivity = config.MinSensitivity;
            MaxSensitivity = config.MaxSensitivity;
            ZoneName = zone.Name;
        }



        public static AlarmDevice Create(AlarmDeviceConfig config, Zone zone, Channel ch, IBus bus)
        {
            AlarmDevice device = null;
            
            switch (config.GetId())
            {
                case "switch":
                    device = AlarmSwitchTimerDevice.CreateDevice((AlarmSwitchConfig)config, zone, ch, bus);
                    break;
                case "perimeter":
                    device = AlarmPerimeterSensor.CreateSensor((AlarmSensorConfig)config, zone, ch, bus);
                    break;
                case "pir":
                    device = AlarmPirSensor.CreateSensor((AlarmSensorConfig)config, zone, ch, bus);
                    break;
            }
            return device;

        }


        public bool isActive()
        {
            return ((MinSensitivity <= Sensitivity) && (MaxSensitivity  >= Sensitivity));
        }


        public override void OnSystemArm(int sensitivity)
        {
            if (channel.State == ContactStates.OPEN.ToString())
            {
                if (Disabled==false)
                    alarmLog.AlarmInfoFormat(channel.Name, "Sensor '{0}' has been disabled", channel.UniqueId);
                Disabled = true;
            }
            else if (Disabled==true)
            {
                Disabled = false;
            }
        }

        
    }   
}
