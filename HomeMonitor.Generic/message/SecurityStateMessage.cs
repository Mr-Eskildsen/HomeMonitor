using HomeMonitor.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMonitor.message
{
    public class SecurityStateMessage : GenericMessage
    {
        public string ThingId { get; private set; }
        public string StateNew { get; private set; }
        public string StatePrev { get; private set; }

        public bool SetStatePrev(string prevState)
        {
            if (StatePrev != null)
                return false;
            StatePrev = prevState;
            return true;
        } 

        public SecurityStateMessage(string instanceId, string id, string thingId, string newState, string prevState)
            : base(instanceId, id)
        {
            ThingId = thingId;
            StateNew = newState;
            StatePrev = prevState;

        }
    }
}
