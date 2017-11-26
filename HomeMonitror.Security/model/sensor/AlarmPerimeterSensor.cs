using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeMonitor.events;
using HomeMonitor.model;
using HomeMonitor.Security.xml;
using MemBus;
using HomeMonitor.Security;

namespace HomeMonitror.Security.model.sensor
{
    public class AlarmPerimeterSensor : AlarmSensor
    {


        protected AlarmPerimeterSensor(AlarmSensorConfig config, AlarmZone zone, Channel ch, IBus bus)
            : base (config, zone, ch, bus)
        {
            //Set default value
            if ((MinSensitivity == 0) && (MaxSensitivity == 10))
            {
                MinSensitivity = 5;
                MaxSensitivity = 10;
            }

        }


        public static AlarmSensor CreateSensor(AlarmSensorConfig config, AlarmZone zone, Channel ch, IBus bus)
        {
            return (AlarmSensor)new AlarmPerimeterSensor(config, zone, ch, bus);
        }



        public override int GetScore()
        {
            int _score = ((Sensitivity >= MinSensitivity) && (Sensitivity <= MaxSensitivity)) ? 100 : 0;
            return ((Sensitivity>= MinSensitivity) && (Sensitivity <= MaxSensitivity)) ? 100 : 0;
      }


        public override bool isPerimeter()
        {
            return true;
        }
    }
}
