using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace TourPlanner.Common.Logging
{
    /// <summary>
    /// Wraps the log4j2 logger instances by realizing interface ILoggerWrapper.
    /// This avoids direct dependencies to log4j2 package.
    /// </summary>
    public class Log4NetWrapper : ILoggerWrapper
    {
        private log4net.ILog logger;

        protected static readonly IConfigurationRoot Config = AppSettings.GetInstance().Configuration;

        public static Log4NetWrapper CreateLogger(string configPath, Type type)
        {
            if (!File.Exists(configPath))
            {
                throw new ArgumentException("Does not exist.", nameof(configPath));
            }

            log4net.GlobalContext.Properties["LogFileName"] = Path.Combine(Config["PersistenceFolder"], "log.txt");
            log4net.Config.XmlConfigurator.Configure(new FileInfo(configPath));
            var logger = log4net.LogManager.GetLogger(type);
            return new Log4NetWrapper(logger);
        }

        private Log4NetWrapper(log4net.ILog logger)
        {
            this.logger = logger;
        }

        public void Debug(string message)
        {
            this.logger.Debug(message);
        }

        public void Info(string message)
        {
            this.logger.Info(message);
        }

        public void Warn(string message)
        {
            this.logger.Warn(message);
        }

        public void Error(string message)
        {
            this.logger.Error(message);
        }

        public void Fatal(string message)
        {
            this.logger.Fatal(message);
        }
    }
}
