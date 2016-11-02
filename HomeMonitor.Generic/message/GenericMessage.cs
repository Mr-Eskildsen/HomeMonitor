using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMonitor.message
{
    public abstract class GenericMessage : IBusMessage
    {
        public string Id { get; protected set; }
        public string InstanceId { get; protected set; }
        


        public GenericMessage(string instanceId, string id)
        {
            Id = id;
            InstanceId = instanceId;
        }
    }
}
