using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Exceptions {
    public class TourPlannerExceptions : Exception {
        public TourPlannerExceptions() { }
        public TourPlannerExceptions(string message) : base(message) { }
        public TourPlannerExceptions(string message, Exception inner) : base(message, inner) { }
    }

    [Serializable]
    public class RouteNotFoundException : Exception { 
        public RouteNotFoundException() { }
        public RouteNotFoundException(string message) : base(message) { }
        public RouteNotFoundException(string message, Exception inner) : base(message, inner) { }
    }


}
