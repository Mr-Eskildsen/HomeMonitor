using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeMonitor.Generic.xml;
using HomeMonitor.message;
using MemBus;
using log4net;


namespace HomeMonitor.model.device
{
  

    public class DeviceSwitch : Channel
    {

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private SwitchStates internalState = SwitchStates.OFF;
        private SwitchStates externalState = SwitchStates.OFF;
        //private bool lockedExternal = false;
        private int internalActivateCount = 0;
        private int externalActivateCount = 0;

        public DeviceSwitch(string thingId, ThingConfig thingConfig, ChannelConfig config, IBus bus) : base(thingId, thingConfig, config, bus)
        {
            State = SwitchStates.OFF.ToString();
        }

        public override bool updateState(ChannelStateMessage csm)
        {
            
            SwitchStates stateNew = (SwitchStates)Enum.Parse(typeof(SwitchStates), csm.StateNew);
            if (stateNew == SwitchStates.ON)
                externalActivateCount++;
            else
                externalActivateCount--;

            if (externalActivateCount < 0)
                externalActivateCount = 0;

            //if ((stateNew == SwitchStates.ON) && (stateNew != internalState))
            /*
            if ((csm.IsExternalState) && (csm.StateNew!=State))
            {
                lockedExternal = true;
            }
            else
            {
                lockedExternal = false;
            }
            */
            externalState = stateNew;

            if (csm.StateNew != State)
            {
                State = csm.StateNew;
            }
            return true;
        }
        public bool IsControlledExternal()
        {
            return (internalActivateCount < externalActivateCount) ? true : false;
        }

        public bool TurnOn()
        {
            //The switch is seems to be turned on by someething else than me
            //if ((externalState == SwitchStates.ON) && internalState == SwitchStates.OFF) {
            if (IsControlledExternal()) { 
                log.InfoFormat("Channel '{0}' could not be Turned on, because internalState is 'OFF' but the channal is reported as having state 'ON' by Z-Wave", UniqueId);
                return false;
            }


            if (internalActivateCount == 0)
            {
                internalActivateCount++;
            }
            else if (internalActivateCount != 0)
            {
                internalActivateCount++;
            }


            if (internalActivateCount >= 1)
            {
                Publish(SwitchStates.ON);
                return true;
            }
            return false;
                        
        }

        public void TurnOff()
        {   
            internalActivateCount--;

            if (internalActivateCount <= 0)
            {
                internalActivateCount = 0;
                Publish(SwitchStates.OFF);
            }
        }

        protected void Publish(SwitchStates state)
        {
            if (IsControlledExternal())
            //if ((externalState == SwitchStates.ON) && (internalState != externalState))
            {
                log.InfoFormat("Channel '{0}' could not be controlled because external state is 'ON'.", UniqueId);
                return;
            }

            if (internalState != state)
            {
                internalState = state;
                State = state.ToString().ToUpper();
                _bus.Publish(new MqttPublishMessage(MqttPublishChannel.device, this, State));
                log.DebugFormat("Channel '{0}' state was changed to state '{1}'", UniqueId, state);
            }
        }

    }
}
