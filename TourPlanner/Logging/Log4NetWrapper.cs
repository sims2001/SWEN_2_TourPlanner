using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace TourPlanner.Logging
{
    class Log4NetWrapper : ILoggerWrapper {
        private ILog _logger;

        public static Log4NetWrapper CreateLogger(string configPath, string caller) {
            var l = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            
            configPath = Path.Combine(l, configPath);

            if (!File.Exists(configPath)) {
                throw new ArgumentException("Does not exist.", nameof(configPath));
            }

            log4net.Config.XmlConfigurator.Configure(new FileInfo(configPath));
            var logger = log4net.LogManager.GetLogger(caller);  // System.Reflection.MethodBase.GetCurrentMethod().DeclaringType
            return new Log4NetWrapper(logger);

        }

        private Log4NetWrapper(ILog logger) {
            _logger = logger;
        }

        public void Info(string message) {
            _logger.Info(message);
        }
        public void Info(string message, Exception ex) {
            _logger.Info(message, ex);
        }

        public void Debug(string message) {
            _logger.Debug(message);
        }
        public void Debug(string message, Exception ex) {
            _logger.Debug(message, ex);
        }

        public void Error(string message) {
            _logger.Error(message);
        }

        public void Error(string message, Exception ex) {
            _logger.Error(message, ex);
        }

        public void Fatal(string message) {
            _logger.Fatal(message);
        }

        public void Fatal(string message, Exception ex) {
            _logger.Fatal(message, ex);
        }

        public void Warn(string message) {
            _logger.Warn(message);
        }
        public void Warn(string message, Exception ex) {
            _logger.Warn(message, ex);
        }
    }
}
