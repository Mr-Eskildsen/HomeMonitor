using HomeMonitor.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMonitor.message.alert
{
    public class SecuritySensorAlertMessage : SecurityAlertMessage
    {
        public Channel channel { get; private set; }
        public string StatePrev { get; private set; }
        public string StateNew { get; private set; }

        public SecuritySensorAlertMessage(string instanceId, Channel ch, string statePrev, string stateNew) : base(instanceId, ch.UniqueId)
        {
            channel = ch;
            StatePrev = statePrev;
            StateNew = stateNew;
        }
    }
}
