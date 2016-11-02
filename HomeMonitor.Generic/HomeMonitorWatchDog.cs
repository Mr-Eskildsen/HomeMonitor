using HomeMonitor.logger;
using HomeMonitor.message;
using log4net;
using MemBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HomeMonitor
{
    public class HomeMonitorWatchDog
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ILog alarmLog = LogManager.GetLogger(typeof(AlarmExtensions));
        private static readonly ILog stateLog = LogManager.GetLogger(typeof(AlarmExtensions));

        protected IBus _bus = null;

        private static readonly Lazy<HomeMonitorWatchDog> _instance = new Lazy<HomeMonitorWatchDog>(() => new HomeMonitorWatchDog());
        public static HomeMonitorWatchDog Instance { get { return _instance.Value; } }
        //private static readonly HomeMonitorWatchDog _instance = new HomeMonitorWatchDog();


        //public static HomeMonitorWatchDog Instance { get { return _instance; } }

        private HomeMonitorWatchDog()
        {

        }

        public static void Initialize(IBus bus)
        {
            Instance._Initialize(bus);
        }


        private void _Initialize(IBus bus)
        {

            log.Debug("Initialisere 'WatchDog'");
            _bus = bus;

            Run();
        }




        protected void Run() {

            Task task = new Task(SendIsAlivePing);
            log.Info("Starting IsAlive task from HomeMonitorWatchDog");
            task.Start();
        }


        protected void SendIsAlivePing() {

            while (true)
            {
                try
                {
                    _bus.Publish(new MqttPublishMessage(MqttPublishChannel.alarm, MqttMessageDirection.outgoing, "system/isalive", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                    //Sleep 1 minute
                    Thread.Sleep(60 * 1000);

                }

                catch (Exception ex)
                {
                    log.ErrorFormat("Exception occurred in Watchdog task. Task is restarted. Exception='{0}'", ex.Message);
                }

            }
        }


    }
}
