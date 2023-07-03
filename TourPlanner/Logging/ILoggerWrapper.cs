using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Logging
{
    interface ILoggerWrapper {
        void Info(string message);
        void Info(string message, Exception ex);
        void Debug(string message);
        void Debug(string message, Exception ex);
        void Error(string message);
        void Error(string message, Exception ex);
        void Fatal(string message);
        void Fatal(string message, Exception ex);
        void Warn(string message);
        void Warn(string message, Exception ex);
    }
}
