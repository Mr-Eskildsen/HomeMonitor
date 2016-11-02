//HEST using HomeMonitor.Notification.logger;
//HEST using SecurityMonitor.events;

using HomeMonitor.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMonitor.model
{
    
    public enum ItemState
    {
        Uninitialized = 0,
        Initialized = 1,
        Activated = 2,
        Disabled = 99
    }
    

    public abstract class GenericItem
    {
        //HEST private static readonly log4net.ILog alarmlog = log4net.LogManager.GetLogger(typeof(AlarmExtensions));

        //HEST public ItemState State { get; protected set; }

        protected bool Activated { get; private set; }

        //HEST protected event EventHandler<StateChangedEventArgs> stateChangedEvent;
        protected event EventHandler<HomeMonitorEventArgs> eventHandler;

        public GenericItem()
        {
            //HEST State = ItemState.Uninitialized;
        }


        public void SubscribeEventHandler(EventHandler<HomeMonitorEventArgs> handler)
        {
            eventHandler += handler;
        }



        //HEST  protected abstract GenericItem[] GetChildren();

        //HEST public void SystemEvent(object sender, SystemEventArgs args)
        //HEST {
        //HEST            
        //HEST     switch (args.eventId)
        //HEST    {
        //HEST case EnumSystemEvent.armed:
        //HEST     Activated = true;
        //HEST     break;

        //HEST case EnumSystemEvent.disarmed:
        //HEST Activated = false;
        //HEST   break;
        //HEST  }
        //HEST  OnSystemEvent(sender, args);
        //HEST  }




        protected void NotifyEvent(HomeMonitorEventArgs  args)
        {
            if (eventHandler != null)
            {
                eventHandler(this, args);
             }
        }


        public String Id { get; protected set; }
        public String Name { get; protected set; }


        //HEST public void NotifyChildren()
        //HEST {
        //HEST GenericItem[] children = GetChildren();
        //HEST if (children != null)
        //HEST {
        //HEST foreach (GenericItem child in GetChildren())
        //HEST {
        //HEST child.NotifyChildren();
        //HEST }
        //HEST }
        //HEST }

    }
}

