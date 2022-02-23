using Metanury.Apps.Core;
using Newtonsoft.Json;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Text;

namespace Metanury.Apps.MVC
{
    public class Logger : ILogHelper
    {
        private static NLog.Logger logger;

        private static readonly Lazy<Logger> lazy = new Lazy<Logger>(() => new Logger());
        public static Logger Current { get { return lazy.Value; } }

        public Logger()
        {
            string AppID = "nLog";
            LoggingConfiguration config = new LoggingConfiguration();
            FileTarget fileTarget = new FileTarget();
            fileTarget.Encoding = Encoding.UTF8;
            fileTarget.FileName = String.Format("{0}/LogData/{1}/{2}/{3}.txt", "${basedir}", "${level}", @"${date:format=yyyyMM}", @"${date:format=yyyyMMdd}");
            fileTarget.Layout = String.Format("[{0}] : {1}", @"${date:format=yyyy-MM-dd HH\:mm\:ss.fff}", "${message}");
            fileTarget.ConcurrentWrites = true;
            fileTarget.ArchiveFileName = String.Format("{0}\\backup\\{1}\\{2}_{3}.txt", "${basedir}", "${logger}", "${level}", @"${date:format=yyyyMMdd}");
            fileTarget.ArchiveAboveSize = 1024 * 1024 * 4;
            fileTarget.ArchiveEvery = FileArchivePeriod.Day;
            fileTarget.MaxArchiveFiles = 30;
            config.AddTarget("file", fileTarget);
            LoggingRule rule = new LoggingRule("*", LogLevel.Debug, fileTarget);
            config.LoggingRules.Add(rule);
            LogManager.Configuration = config;
            logger = LogManager.GetLogger(AppID);
        }

        public void Debug(string msg)
        {
            logger.Debug(msg);
        }

        public void Debug<T>(T target)
        {
            logger.Debug(JsonConvert.SerializeObject(target));
        }

        public void Error(string msg)
        {
            logger.Error(msg);
        }

        public void Error(Exception ex)
        {
            logger.Error(ex);
        }

        public void Warn(string msg)
        {
            logger.Warn(msg);
        }

        public void Fatal(string msg)
        {
            logger.Fatal(msg);
        }

        public void Info(string msg)
        {
            logger.Info(msg);
        }

        public void Info<T>(T target)
        {
            logger.Info(JsonConvert.SerializeObject(target));
        }
    }
}
