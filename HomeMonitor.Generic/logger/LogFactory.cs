using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System.IO;
using log4net.Core;
using log4net.Filter;

namespace HomeMonitor.logger
{
    public static class LogFactory
    {


        // Set the level for a named logger
        public static void SetLevel(string loggerName, string levelName)
        {
            ILog log = LogManager.GetLogger(loggerName);
            Logger l = (Logger)log.Logger;

            l.Level = l.Hierarchy.LevelMap[levelName];
        }

        // Add an appender to a logger
        private static void AddAppender(string loggerName, IAppender appender)
        {
            ILog log = LogManager.GetLogger(loggerName);
            Logger l = (Logger)log.Logger;

            l.AddAppender(appender);
        }
        /*
        // Create a new file appender
        public static IAppender CreateFileAppender(string name, string fileName)
        {
            FileAppender appender = new
                FileAppender();
            appender.Name = name;
            appender.File = fileName;
            appender.AppendToFile = true;

            PatternLayout layout = new PatternLayout();
            layout.ConversionPattern = "%d [%t] %-5p %c [%x] - %m%n";
            layout.ActivateOptions();

            appender.Layout = layout;
            appender.ActivateOptions();

            return appender;
        }
        */
        public static MailAppender CreateMailAppender(string AppenderName)
        {
            //Create the rolling file appender for the "normal log"
            MailAppender appender = new MailAppender();
            appender.Name = AppenderName;
            return appender;
        }

        public static RollingFileAppender CreateRollingFileAppender(string AppenderName, string logfileName)
        {
            //Create the rolling file appender for the "normal log"
            RollingFileAppender appender = new RollingFileAppender();
            appender.Name = AppenderName;
            appender.File = logfileName;
            appender.StaticLogFileName = true;
            appender.AppendToFile = true;
            appender.RollingStyle = RollingFileAppender.RollingMode.Size;
            appender.MaxSizeRollBackups = 10;
            appender.MaximumFileSize = "10MB";
            appender.PreserveLogFileNameExtension = true;

            return appender;
        }



        public static void ConfigureLog4Net()
        {
#if DEBUG
            log4net.Util.LogLog.InternalDebugging = true;
#endif

            //Register Custom loglevels
            AlarmExtensions.Initialize();

            string logDirectoryName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log");
            Console.WriteLine("logDirectoryName='" + logDirectoryName + "'");



            /* *********************************************************
             * Get Hierachy
             * ********************************************************/
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();

            /* *********************************************************
             * Add custom levels to map
             * ********************************************************/

            hierarchy.LevelMap.Add(AlarmExtensions.AlarmStateLevel);
            hierarchy.LevelMap.Add(AlarmExtensions.AlarmAlertLevel);
            hierarchy.LevelMap.Add(AlarmExtensions.AlarmUrgentLevel);
            hierarchy.LevelMap.Add(AlarmExtensions.AlarmNotifyLevel);


            /* *********************************************************
             * Load log4net.config
             * ********************************************************/
            /*string configDirectoryName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config");
            
            string logFilePath = Path.Combine(configDirectoryName, "log4net.config");
            FileInfo finfo = new FileInfo(logFilePath);
            log4net.Config.XmlConfigurator.ConfigureAndWatch(finfo);
            */


            /**********************************************************
             * 
             * Filters
             * 
             * ********************************************************/
            LevelMatchFilter filterAllowNotify = new LevelMatchFilter();
            filterAllowNotify.LevelToMatch = AlarmExtensions.AlarmNotifyLevel;

            LevelMatchFilter filterAllowUrgent = new LevelMatchFilter();
            filterAllowUrgent.LevelToMatch = AlarmExtensions.AlarmUrgentLevel;

            LevelMatchFilter filterAllowAlert = new LevelMatchFilter();
            filterAllowAlert.LevelToMatch = AlarmExtensions.AlarmAlertLevel;


            LevelMatchFilter filterAllowState = new LevelMatchFilter();
            filterAllowState.LevelToMatch = AlarmExtensions.AlarmStateLevel;
            filterAllowState.AcceptOnMatch = true;


            LevelRangeFilter filterLog = new LevelRangeFilter();
#if DEBUG
            filterLog.LevelMin = Level.Debug;
            filterLog.LevelMin = Level.Info;
#else
            filterLog.LevelMin = Level.Info;
#endif            
            filterLog.LevelMax = Level.Fatal;
            filterLog.AcceptOnMatch = true;


            //Create the filter
            LevelRangeFilter filterAlarmNotify = new LevelRangeFilter();
#if DEBUG
            filterAlarmNotify.LevelMin = AlarmExtensions.AlarmVerboseLevel;
            filterAlarmNotify.LevelMin = AlarmExtensions.AlarmNotifyLevel;
#else
            filterAlarmNotify.LevelMin = AlarmExtensions.AlarmNotifyLevel;
#endif
            filterAlarmNotify.LevelMax = AlarmExtensions.AlarmAlertLevel;
            filterAlarmNotify.AcceptOnMatch = true;

            LevelRangeFilter filterAlarmAlert = new LevelRangeFilter();
            filterAlarmAlert.LevelMin = AlarmExtensions.AlarmUrgentLevel;
            filterAlarmAlert.LevelMax = AlarmExtensions.AlarmAlertLevel;
            filterAlarmAlert.AcceptOnMatch = true;


            //var filterDenyAll = new log4net.Filter.DenyAllFilter();


            /**********************************************************/
            //Configure the layout of the trace message write
            var layoutLog = new log4net.Layout.PatternLayout()
            {
                ConversionPattern = "%date{yyyy-MM-dd HH:mm:ss.fff} [%thread] %-10level - %message%newline"
            };
            layoutLog.ActivateOptions();

            /******************************************************************/
            //Configure the layout of the trace message write
            var layoutAlarm = new log4net.Layout.PatternLayout()
            {
                ConversionPattern = "%date{yyyy-MM-dd HH:mm:ss.fff} [%-10level] - %-9property{ID} - %message%newline"
            };
            layoutAlarm.ActivateOptions();

            /* *********************************************************
             * Create Loggers
             * ********************************************************/
            Logger rootLogger = hierarchy.Root;
            Logger alarmLogger = hierarchy.Root; //hierarchy.LoggerFactory.CreateLogger((ILoggerRepository)hierarchy, "AlarmLogger");
            Logger stateLogger = hierarchy.Root; //hierarchy.LoggerFactory.CreateLogger((ILoggerRepository)hierarchy, "StateLogger");

            /* *********************************************************
             * 
             * Create appenders
             * 
             * ********************************************************/

            /* *********************************************************
             * Log: Console Appender
             * ********************************************************/
#if DEBUG
            ConsoleAppender consoleAppender = new ConsoleAppender();
            consoleAppender.Layout = layoutLog;

            consoleAppender.ActivateOptions();
            hierarchy.Root.AddAppender(consoleAppender);
#endif

            /* *********************************************************
             * Log: Rolling File Appender
             * ********************************************************/
            RollingFileAppender rollingAppender = CreateRollingFileAppender("RollingAppender", Path.Combine(logDirectoryName, "File.log"));


#if DEBUG
            rollingAppender.AddFilter(new LevelMatchFilter() { LevelToMatch = Level.Debug, AcceptOnMatch = true });
#endif
            rollingAppender.AddFilter(new LevelMatchFilter() { LevelToMatch = Level.Info, AcceptOnMatch = true });
            rollingAppender.AddFilter(new LevelMatchFilter() { LevelToMatch = Level.Warn, AcceptOnMatch = true });
            rollingAppender.AddFilter(new LevelMatchFilter() { LevelToMatch = Level.Fatal, AcceptOnMatch = true });

            rollingAppender.AddFilter(new log4net.Filter.DenyAllFilter());
            rollingAppender.Layout = layoutLog;
            //Let log4net configure itself based on the values provided
            rollingAppender.ActivateOptions();
            hierarchy.Root.AddAppender(rollingAppender);

            /* *********************************************************
             * AlarmLog: Rolling File Appender
             * ********************************************************/
            RollingFileAppender alarmFileAppender = CreateRollingFileAppender("AlarmAppender", Path.Combine(logDirectoryName, "Alarm.log"));
#if DEBUG
            alarmFileAppender.AddFilter(new LevelMatchFilter() { LevelToMatch = AlarmExtensions.AlarmVerboseLevel, AcceptOnMatch = true });
#endif
            alarmFileAppender.AddFilter(new LevelMatchFilter() { LevelToMatch = AlarmExtensions.AlarmNotifyLevel, AcceptOnMatch = true });
            alarmFileAppender.AddFilter(new LevelMatchFilter() { LevelToMatch = AlarmExtensions.AlarmUrgentLevel, AcceptOnMatch = true });
            alarmFileAppender.AddFilter(new LevelMatchFilter() { LevelToMatch = AlarmExtensions.AlarmAlertLevel, AcceptOnMatch = true });

            alarmFileAppender.AddFilter(new log4net.Filter.DenyAllFilter());

            alarmFileAppender.Layout = layoutAlarm;
            //Let log4net configure itself based on the values provided
            alarmFileAppender.ActivateOptions();
            alarmLogger.AddAppender(alarmFileAppender);


            /* *********************************************************
             * AlarmLog: Rolling File Appender
             * ********************************************************/
            MailAppender mailAppender = CreateMailAppender("AlarmMailAppender");
            mailAppender.AddFilter(new LevelRangeFilter()
            {
                LevelMin = AlarmExtensions.AlarmUrgentLevel,
                LevelMax = AlarmExtensions.AlarmAlertLevel,
                AcceptOnMatch = true
            });
            mailAppender.AddFilter(new log4net.Filter.DenyAllFilter());
            alarmFileAppender.Layout = layoutAlarm;
            //Let log4net configure itself based on the values provided
            alarmFileAppender.ActivateOptions();
            alarmLogger.AddAppender(alarmFileAppender);




            /* *********************************************************
             * StateLog: Rolling File Appender
             * ********************************************************/
            RollingFileAppender stateFileAppender = CreateRollingFileAppender("StateAppender", Path.Combine(logDirectoryName, "State.log"));

            stateFileAppender.AddFilter(new LevelMatchFilter() { LevelToMatch = AlarmExtensions.AlarmStateLevel, AcceptOnMatch = true });
            stateFileAppender.AddFilter(new log4net.Filter.DenyAllFilter());

            stateFileAppender.Layout = layoutAlarm;
            layoutAlarm.ActivateOptions();
            //Let log4net configure itself based on the values provided
            stateFileAppender.ActivateOptions();

            stateLogger.AddAppender(stateFileAppender);



            hierarchy.Configured = true;


        }

    }
}
