using log4net;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMonitor.logger
{

    

    public static class AlarmExtensions
    {
        
        public const int AlarmAlertLevelValue = 50020;
        public const int AlarmUrgentLevelValue = 50015;
        public const int AlarmNotifyLevelValue = 50013;
        public const int AlarmVerboseLevelValue = 50010;

        public const int AlarmStateLevelValue = 50000;

        public static readonly log4net.Core.Level AlarmAlertLevel = new log4net.Core.Level(AlarmAlertLevelValue, "ALARM");
        public static readonly log4net.Core.Level AlarmUrgentLevel = new log4net.Core.Level(AlarmUrgentLevelValue, "URGENT");
        public static readonly log4net.Core.Level AlarmNotifyLevel = new log4net.Core.Level(AlarmNotifyLevelValue, "NOTIFY");
        public static readonly log4net.Core.Level AlarmStateLevel = new log4net.Core.Level(AlarmStateLevelValue, "STATE");
        public static readonly log4net.Core.Level AlarmVerboseLevel = new log4net.Core.Level(AlarmVerboseLevelValue, "VERBOSE");

        public static void Initialize()
        {
            log4net.LogManager.GetRepository().LevelMap.Add(AlarmExtensions.AlarmAlertLevel);
            log4net.LogManager.GetRepository().LevelMap.Add(AlarmExtensions.AlarmUrgentLevel);
            log4net.LogManager.GetRepository().LevelMap.Add(AlarmExtensions.AlarmNotifyLevel);
            log4net.LogManager.GetRepository().LevelMap.Add(AlarmExtensions.AlarmVerboseLevel);
            log4net.LogManager.GetRepository().LevelMap.Add(AlarmExtensions.AlarmStateLevel);
        }

        public static void AlarmAlert(this ILog log, string id, string message)
        {
            ThreadContext.Properties["ID"] = id;
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
                AlarmAlertLevel, message, null);
        }

        public static void AlarmAlertFormat(this ILog log, string id, string message, params object[] args)
        {
            ThreadContext.Properties["ID"] = id;
            string formattedMessage = string.Format(message, args);
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
                AlarmAlertLevel, formattedMessage, null);
        }

        public static void AlarmWarn(this ILog log, string id, string message)
        {
            ThreadContext.Properties["ID"] = id;
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
                AlarmUrgentLevel, message, null);
        }

        public static void AlarmWarnFormat(this ILog log, string id, string message, params object[] args)
        {
            ThreadContext.Properties["ID"] = id;
            string formattedMessage = string.Format(message, args);
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
                AlarmUrgentLevel, formattedMessage, null);
        }
        
        public static void StateEvent(this ILog log, string Id, string prevState, string newState)
        {
            string message = string.Format("'{0}': State changed from '{1}' to '{2}'", Id, prevState, newState);
            ThreadContext.Properties["ID"] = Id;
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
                AlarmStateLevel, string.Format("'{0}': State changed from '{1}' to '{2}'", Id, prevState, newState), null);
          
        }
        
        public static void StateEventFormat(this ILog log, string Id, string message, params object[] args)
        {
            ThreadContext.Properties["ID"] = Id;
            string formattedMessage = string.Format(message, args);
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
                AlarmStateLevel, formattedMessage, null);
        }
        

        public static void AlarmInfo(this ILog log, string Id, string message)
        {
            ThreadContext.Properties["ID"] = Id;
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
                AlarmNotifyLevel, message, null);
        }

        public static void AlarmInfoFormat(this ILog log, string Id, string message, params object[] args)
        {
            ThreadContext.Properties["ID"] = Id;
            string formattedMessage = string.Format(message, args);
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
                AlarmNotifyLevel, formattedMessage, null);
        }

        public static void AlarmDebug(this ILog log, string Id, string message)
        {
            ThreadContext.Properties["ID"] = Id;
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
                AlarmVerboseLevel, message, null);
        }

        public static void AlarmDebugFormat(this ILog log, string Id, string message, params object[] args)
        {
            ThreadContext.Properties["ID"] = Id;
            string formattedMessage = string.Format(message, args);
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
                AlarmVerboseLevel, formattedMessage, null);
        }
    }
}
