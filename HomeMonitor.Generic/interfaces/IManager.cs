using HomeMonitor.message;
using MemBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMonitor.Generic.interfaces
{
    public interface IManager
    {
        void Publish(IBusMessage message);
        void Subscribe(ISubscriber subscriber);
    }

}
