using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace TourPlanner.Logging
{
    class LoggerFactory {
        public static ILoggerWrapper GetLogger(IConfiguration config) {
            StackTrace stackTrace = new StackTrace(1, false); //Captures 1 frame, false for not collecting information about the file
            var type = stackTrace.GetFrame(1).GetMethod().DeclaringType;
            var c = config["Log4NetConfig:path"];
            return Log4NetWrapper.CreateLogger(c, type.FullName);
        }

    }
}
