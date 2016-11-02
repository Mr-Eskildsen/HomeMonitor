using HomeMonitor.message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMonitor.events
{
     public class HomeMonitorEventArgs : EventArgs
    {
        //public EnumSystemEvent eventId { get; protected set; }
        public GenericMessage Message { get; private set; }

        public HomeMonitorEventArgs(GenericMessage message)
        {
            Message = message;
        }
    }

}
