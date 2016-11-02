using HomeMonitor.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HomeMonitor.message;
using MemBus;
using HomeMonitor.Security.xml;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using HomeMonitor.Security;
using System.Reflection;
using MemBus.Subscribing;
using HomeMonitor.events;
using HomeMonitor.logger;
using HomeMonitror.Security.model.sensor;
using HomeMonitor.model;
using log4net;
using HomeMonitror.Security.model;
using HomeMonitror.Security.events;
using HomeMonitor.message.alert;
using HomeMonitror.Security.xml;
using HomeMonitor.notification;
using HomeMonitor.Generic.interfaces;

namespace HomeMonitror.Security
{
    public sealed class SecurityAlarmMgr : AlarmGenericItem, IManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ILog alarmLog = LogManager.GetLogger(typeof(AlarmExtensions));
        private static readonly ILog stateLog = LogManager.GetLogger(typeof(AlarmExtensions));


        private static readonly Lazy<SecurityAlarmMgr> _instance = new Lazy<SecurityAlarmMgr>(() => new SecurityAlarmMgr());

        private volatile bool canArmHysteresis = true;

        private List<Zone> zones = new List<Zone>();

        //        protected event EventHandler<SecurityAlarmSystemEventArgs> systemEventHandler;



            /*
        protected override void OnRaiseAlert(object sender, SecurityAlarmAlertEventArgs e)
        {

            alarmLog.AlarmAlertFormat(e.ThingId, "Sensor '{0}:{1}' has raised an ALERT", e.ThingId, e.ChannelId);
        }
        */

        //HEST private SecurityConfig config;
        private int _sensitivity = -2;

        public static SecurityAlarmMgr Instance { get { return _instance.Value; } }

        public static bool ClearData()
        {
            return Instance._ClearData();
        }

        public static void Initialize(IBus bus, SecurityConfig securityConfig = null)
        {
            Instance._Initialize(bus, securityConfig);
        }


        public static ILog GetAlarmLogger()
        {
            return alarmLog;
        }

        private SecurityAlarmMgr()
            : base(null)
        {
        }


        private bool _ClearData()
        {
            SystemDisarmed();

            _sensitivity = -2;
            zones.Clear();
            ClearChildren();
            return true;
        }

        private void _Initialize(IBus bus, SecurityConfig securityConfig) {
            SecurityConfig config = null;
            log.Debug("Initialisere 'SecurityAlarmMgr'");
            _bus = bus;

            //Listen to mesage to the alarm
            bus.Subscribe<SecuritySystemStateMessage>(msg => StateChangedEvent(msg), new ShapeToFilter<SecuritySystemStateMessage>(csm => (true) ));
            
            //Subscribe to all security alarms
            bus.Subscribe<SecurityAlertMessage>(msg => SecurityAlertEvent(msg));


            if (securityConfig == null)
            {
                config = LoadConfig();
            }
            else
            {
                config = securityConfig;
            }

            try { 
                foreach (ZoneConfig zoneConfig in config.Zones)
                {
                    log.DebugFormat("'LoadConfig': Procssesing zone '{0}'", zoneConfig.Name);
                    Zone zone = new Zone(zoneConfig, _bus);

                    zones.Add(zone);
                    AddChild(zone);

                    //instance._AddZone(zone.Id, zone);
                }
                NotificationFactory.Create(_bus, config.notifications);

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public Zone GetZoneById(string id)
        {
            return zones.Single<Zone>(zone => zone.Id == id);
        }

        public void ArmSystem(int sensitivity)
        {
            bool wasArmed = isArmed();
            int prevSensitivity = _sensitivity;
            int newSensitivity = sensitivity;

            if (_updateSystemStatus(sensitivity))
            {
                foreach (Zone zone in zones)
                    if (wasArmed)
                    {
                        zone.SystemSensitivityChanged(prevSensitivity, newSensitivity);
                        alarmLog.AlarmInfoFormat("SecurityAlarmMgr", "Sensitivity changed from '{0}' => '{1}'", prevSensitivity, newSensitivity);
                    }
                    else
                    {
                        zone.SystemArmed(sensitivity);
                        alarmLog.AlarmInfoFormat("SecurityAlarmMgr", "Alarm is ARMED (Sensitivity='{0}')", sensitivity);
                    }
                
                
            }

        }

        public void DisarmSystem()
        {
            if (_updateSystemStatus(0))
            {
                
                foreach (Zone zone in zones)
                    zone.SystemDisarmed();

                alarmLog.AlarmInfo("SecurityAlarmMgr", "Alarm is DISARMED");
            }
        }

        

        private bool isArming(int newSensitivity)
        {
            return ((_sensitivity == 0) && (newSensitivity != 0)) ? true : false;
        }

        private bool isDisarming(int newSensitivity)
        {
            return ((_sensitivity != 0) && (newSensitivity == 0)) ? true : false;
        }

        private bool isDisarmed()
        {
            return (_sensitivity <= 0) ? true : false;
        }

        private bool isArmed()
        {
            return !isDisarmed();
        }


        public int getSensitivity()
        {
            if (isDisarmed())
                return 0;
            return _sensitivity;
        }

        private bool _updateSystemStatus(int sensitivity)
        {
            bool res = false;
            bool canArm = false;

            //No Change in sensitivity
            if (sensitivity == _sensitivity)
                return res;

            //Disarmed and no hysteresis
            if ((canArmHysteresis == true) && (isDisarmed()))
            {
                canArm = true;
            }
            //Hysteresis is active
            else if ((canArmHysteresis == false) && (isDisarming(sensitivity)))
            {
                canArm = false;
            }
            //Sensitivity just changed
            else if (!isDisarmed() && !isArming(sensitivity) && (_sensitivity != sensitivity))
            {
                canArm = true;
            }



            if (canArm)
            {
                if (isArming(sensitivity))
                {
                    Task task = new Task(ArmingHysteresis);
                    task.Start();
                }
                _sensitivity = sensitivity;


                _bus.Publish(new MqttPublishMessage(MqttPublishChannel.alarm, MqttMessageDirection.outgoing, "system/armed", _sensitivity.ToString()));
                _bus.Publish(new MqttPublishMessage(MqttPublishChannel.alarm, MqttMessageDirection.outgoing, "system/status", "OK"));

                res = true;
            }
            return res;
        }


        protected void ArmingHysteresis()
        {
            canArmHysteresis = false;
            //Five minutes
            Thread.Sleep(30 * 1000);

            //HEST alarmLog.AlarmDebug("SecurityAlarmMgr", "Alarm arm/disarm hysteresis timed out!");
            canArmHysteresis = true;
        }



        private SecurityConfig LoadConfig()
        {
            
            SecurityConfig config = null;
            try
            {
                // Create an instance of the XmlSerializer specifying type and namespace.
                XmlSerializer serializer = new XmlSerializer(typeof(SecurityConfig));


                string appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string configDir = Path.Combine(appDir, "config");
                string filename = Path.Combine(configDir, "security.xml");

                log.InfoFormat("'LoadConfig': Indlæser konfigurationsfilen '{0}' ", filename);

                // A FileStream is needed to read the XML document.
                FileStream fs = new FileStream(filename, FileMode.Open);
                XmlReader reader = XmlReader.Create(fs);


                // Use the Deserialize method to restore the object's state.

                config = (SecurityConfig)serializer.Deserialize(reader);

                fs.Close();


            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return config;            

        }


        /*
         * 
         * 
         * */
        public void SecurityAlertEvent(SecurityAlertMessage msg)
        {
            string Category = msg.Id;
            string Message = string.Format("Sensor '{0}' has raised an ALERT", msg.Id);
            RaiseAlert(Category, Message);
            
        }

        protected void StateChangedEvent(SecuritySystemStateMessage msg)
        {
            switch(msg.Command)
            {
                case AlarmSystemCommand.sensitivity:
                case AlarmSystemCommand.arm:
                    ArmSystem(msg.SensitivityNew);
                    break;
                case AlarmSystemCommand.disarm:
                    DisarmSystem();
                    break;
                //case AlarmSystemCommand.presence:
                //    break;
                default:
                    log.ErrorFormat("'StateChangedEvent': Unknown comamnd received '{0}'", msg.Command);

                    throw new NotImplementedException();

            }
        }

        void IManager.Publish(IBusMessage message)
        {
            _bus.Publish(message);
        }

        void IManager.Subscribe(ISubscriber subscriber)
        {
            _bus.Subscribe(subscriber);
        }
    }
}

