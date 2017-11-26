using HomeMonitor.events;
using HomeMonitor.Generic.xml;
using HomeMonitor.logger;
using HomeMonitor.message;
using HomeMonitor.model.device;
using HomeMonitor.model.sensor;
using MemBus;
using MemBus.Subscribing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMonitor.model
{

    public abstract class Channel : GenericItem
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly log4net.ILog alarmLog = log4net.LogManager.GetLogger(typeof(AlarmExtensions));
        private static readonly log4net.ILog stateLog = log4net.LogManager.GetLogger(typeof(AlarmExtensions));

        protected IBus _bus = null;

        public string ThingId { get; private set; }
        ChannelType Type { get; set; }

        public bool Disabled { get; protected set; }
        public string State { get; protected set; }
        public string UniqueId { get { return ThingRegistry.CalculateUniqueChannelId(ThingId, Id); } }

        public string Subtopic { get;  private set;}

        protected Channel(string thingId, string thingGroup, string channelId, ChannelType type, IBus bus)
        {
            _Initialize(thingId, thingGroup, channelId, type, bus);
        }

        protected Channel(string thingId, string thingGroup, ChannelConfig config, IBus bus)
        {
            _Initialize(thingId, thingGroup, config.GetId(), config.ChannelType, bus);
        }

        private void _Initialize(string thingId, string thingGroup, string channelId, ChannelType channelType, IBus bus)
        {
            Disabled = true;
            ThingId = thingId;
            Id = channelId.ToLower();
            _bus = bus;
            
            //if (config.Direction == "out")
            //    //Subtopic = string.Format(@"{0}/{1}/{2}/{3}/{4}", config.Direction, thingConfig.Group, config.ChannelType.ToString().ToLower(), thingConfig.Id, config.GetId().ToLower());
            //Subtopic = string.Format(@"{0}/{1}/{2}/{3}", thingGroup, channelType.ToString().ToLower(), thingId, channelId.ToLower());

        }

        public void StateChangedEvent(ChannelStateMessage csm)
        {
            if (csm.StatePrev==null)
                csm.SetStatePrev (State);

            if (updateState(csm))
                NotifyEvent(new HomeMonitorEventArgs(csm));
            
        }


        public static Channel Create(String thingId, String thingGroup, ChannelConfig config, IBus bus)
        {
            Channel channel = null;
            switch (config.ChannelType)
            {
                case ChannelType.Battery:
                    channel = new ChannelBattery(thingId, thingGroup, config, bus);
                    break;

                case ChannelType.Contact:
                    channel = new ChannelContact(thingId, thingGroup, config, bus);
                    break;
                    
                case ChannelType.Tamper:
                    channel = new ChannelTamper(thingId, thingGroup, config, bus);
                    break;

                case ChannelType.Switch:
                    channel = new DeviceSwitch(thingId, thingGroup, config, bus);
                    break;

            }

            if (channel!=null)
                channel.Subscribe(bus);

            return channel;
        }

        
        protected void Notify()
        {
            if (!Disabled)
            {
                //HEST _NotifyStateChangedEvent(new StateChangedEventArgs(ThingId, Id));
            }

        }
        
        public abstract bool updateState(ChannelStateMessage csm);


        private void Subscribe(IBus bus)
        {
                        
            bus.Subscribe<ChannelStateReceived>(msg => StateChangedEvent(msg), new ShapeToFilter<ChannelStateReceived>(csm => csm.ThingId==ThingId && csm.ChannelId==Id));
        }
    }
}
 