using HomeMonitor.helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMonitor.model
{

    public class StateEnum
    {
    }

    public enum ContactStates
    {
        [StringValue("CLOSED")]
        CLOSED = 0,

        [StringValue("OPEN")]
        OPEN = 1
    };

    public enum SwitchStates 
    {
        [StringValue("OFF")]
        OFF = 0,

        [StringValue("ON")]
        ON = 1
    };

}
