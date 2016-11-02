using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using MemBus;
using MemBus.Configurators;
using MemBus.Subscribing;
using HomeMonitor.interfaces;
using HomeMonitror.Security;
using HomeMonitor.config;
using HomeMonitor.xml;
using HomeMonitor.Mqtt;
using HomeMonitor.Generic.xml;
using HomeMonitor.model;
using HomeMonitor.message;
using HomeMonitor.Generic.interfaces;

namespace HomeMonitor
{
    public class HomeMonitorMgr : IManager
    {
        private static readonly Lazy<HomeMonitorMgr> _instance = new Lazy<HomeMonitorMgr>(() => new HomeMonitorMgr());
        private List<IManager> managers = new List<IManager>();

        public static HomeMonitorMgr Instance { get { return _instance.Value; } }


        private IBus bus = null;
        private MqttManager mqttMgr;
        private HomeMonitorWatchDog watchDog;

        private HomeMonitorMgr()
        {
            //Start EventBus
            bus = BusSetup.StartWith<Conservative>().Apply<FlexibleSubscribeAdapter>(a => a.RegisterMethods("Handle")).Construct();


            watchDog = HomeMonitorWatchDog.Instance;
            HomeMonitorWatchDog.Initialize(bus);

            ApplicationConfig config = ConfigFactory.LoadAppConfig("homemonitor.xml");
            ThingsConfig thingsConfig = ConfigFactory.LoadConfigFromXMLFile<ThingsConfig>("things.xml");

            ThingRegistry.Initialize(bus, thingsConfig);

            SecurityAlarmMgr.Initialize(bus);
            managers.Add(SecurityAlarmMgr.Instance);



            mqttMgr = MqttManager.Create(bus, config.mqttConfig);

        }

        public void Publish(IBusMessage message)
        {
            bus.Publish(message);
        }


        public void Subscribe(ISubscriber subscriber)
        {
            bus.Subscribe(subscriber);
        }
    }

    
}
