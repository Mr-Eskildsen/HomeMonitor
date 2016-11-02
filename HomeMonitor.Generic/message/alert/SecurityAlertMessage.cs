using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMonitor.message.alert
{
    public abstract class SecurityAlertMessage : GenericMessage
    {
        public SecurityAlertMessage(string instanceId, string id) : base(instanceId, id)
        {
        }
    }
}
