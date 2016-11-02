﻿using HomeMonitor.exception;
using HomeMonitor.Generic.xml;
using HomeMonitor.message;
using MemBus;
//HEST using HomeMonitor.Notification.logger;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMonitor.model.sensor
{
    public class SensorBattery : SensorNumber
    {
        //HEST private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //HEST private static readonly log4net.ILog alarmLog = log4net.LogManager.GetLogger(typeof(AlarmExtensions));

        public SensorBattery(String thingId, ThingConfig thingConfig, ChannelConfig config, IBus bus) 
            : base(thingId, thingConfig, config, bus)
        {
            //State = "0";
        }
        

        public override int GetScore()
        {
            return Convert.ToInt32(State);
        }

        
         public override bool updateState(ChannelStateMessage csm)
        {
            bool hasChanged = false;

            int newState;
            string prevState = State;

            bool parsed = Int32.TryParse(csm.StateNew, out newState);
            if (parsed)
            {
                if ((newState > 100) || (newState < 0))
                    throw new SensorValueOutOfRangeException();
                State = csm.StateNew;

                hasChanged = true;
            }
            else
            {
                throw new SensorValueNotNumericException();
            }
            return hasChanged;
        }

    }
}
