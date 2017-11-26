using HomeMonitor.Generic.xml;
using HomeMonitor.logger;
using HomeMonitor.message;
using log4net;
using MemBus;
//HEST using HomeMonitor.Notification.logger;
//HEST using log4net;
//HEST using SecurityMonitor.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMonitor.model.sensor
{
    public class ChannelContact : Channel
    {
        private int _score = 0;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ILog alarmLog = LogManager.GetLogger(typeof(AlarmExtensions));
        private static readonly ILog stateLog = LogManager.GetLogger(typeof(AlarmExtensions)); //= LogManager.GetLogger("StateLogger");

        public ChannelContact(String thingId, string thingGroup, ChannelConfig config, IBus bus)
            : base(thingId, thingGroup, config, bus)
        {

        }
        
        /* //HEST
        public override void OnSystemEvent(object sender, SystemEventArgs args)
        {
            alarmLog.AlarmDebugFormat(UniqueId, "Sensor '{0}' received SystemEvent '{1}'", ThingId, args.eventId.ToString());
            switch (args.eventId)
            {
                case EnumSystemEvent.disarmed:
                    Disabled = false;
                    break;
                case EnumSystemEvent.armed:
                    if (GetScore() > 0)
                        Disabled = true;
                    break;
            }
        }
        */


        public override bool updateState(ChannelStateMessage csm)
        {
            bool hasChanged = false;
            String prevState = State;

            
            stateLog.StateEvent(UniqueId, prevState, csm.StateNew);

            switch (csm.StateNew.ToUpper())
            {
                case "OPEN":
                case "ON":
                    State = ContactStates.OPEN.ToString();
                    break;

                case "CLOSED":
                case "OFF":
                    State = ContactStates.CLOSED.ToString();
                    break;
            }
            if (prevState != csm.StateNew)
            {
                hasChanged = true;
            }
          
            return hasChanged;

        }
    }
}
