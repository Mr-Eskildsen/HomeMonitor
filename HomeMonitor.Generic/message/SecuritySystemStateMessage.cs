using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMonitor.message
{

    public enum AlarmSystemCommand
    {
        arm=1,
        disarm=2,
        sensitivity=3,
        presence =4
    }

    public class SecuritySystemStateMessage : SecurityStateMessage
    {
        public AlarmSystemCommand Command { get; private set; }
        public int SensitivityNew { get { return Convert.ToInt32(StateNew); } }

        public SecuritySystemStateMessage(AlarmSystemCommand cmd, string newState, string prevState)
            : base( string.Empty,"", "", newState, prevState)
        {
            Command = cmd;
        }
    }
}
