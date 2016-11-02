using HomeMonitor;
using HomeMonitor.logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMonitorConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            /*
             * // In order to set the level for a logger and add an appender reference you
// can then use the following calls:
SetLevel("Log4net.MainForm", "ALL");
AddAppender("Log4net.MainForm", CreateFileAppender("appenderName", "fileName.log"));
    }
*/

            LogFactory.ConfigureLog4Net();

            HomeMonitorMgr mgr = HomeMonitorMgr.Instance;
        }





    }
}
