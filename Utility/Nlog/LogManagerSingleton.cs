using NLog.Config;
using NLog.Targets;
using NLog;
using System.Reflection;
using System.Text;
using System.Diagnostics;

namespace Utilities.Nlog
{
    public class LogManagerSingleton : Singleton<LogManagerSingleton>
    {
        public Logger logger = LogManager.GetCurrentClassLogger();
        //private string _className = MethodBase.GetCurrentMethod().DeclaringType.Name;
        public void PrintLog(string message, LogLevel logLevel, bool isShowClassName = true)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (isShowClassName)
            {
                var Class = new System.Diagnostics.StackTrace(1).GetFrame(1).GetMethod().DeclaringType.Name;
                string Method = new System.Diagnostics.StackTrace(0).GetFrame(1).GetMethod().Name;
                stringBuilder.Append("Class:");
                stringBuilder.Append("【");
                stringBuilder.Append(Class);
                stringBuilder.Append("】");
                stringBuilder.Append("Method:");
                stringBuilder.Append("【");
                stringBuilder.Append(Method);
                stringBuilder.Append("】");

            }
            stringBuilder.Append("  Message:");
            stringBuilder.Append("【");
            stringBuilder.Append(message);
            stringBuilder.Append("】");

            if (logLevel == LogLevel.Info)
                logger.Info(stringBuilder.ToString());
            else if (logLevel == LogLevel.Warn)
                logger.Warn(stringBuilder.ToString());
            else if (logLevel == LogLevel.Error)
                logger.Error(stringBuilder.ToString());
            else if (logLevel == LogLevel.Trace)
                logger.Trace(stringBuilder.ToString());
            else if (logLevel == LogLevel.Fatal)
                logger.Fatal(stringBuilder.ToString());

        }
        public LogManagerSingleton()
        {
            CreateLogger();
        }
        private static void CreateLogger()
        {
            var config = new LoggingConfiguration();
            var fileTarget = new FileTarget
            {
                FileName = "${basedir}/logs/${shortdate}.log",
                Layout = "${date:format=yyyy-MM-dd HH\\:mm\\:ss} [${uppercase:${level}}] ${message}",
            };
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, fileTarget);
            LogManager.Configuration = config;
        }
    }
}
