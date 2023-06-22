using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Models;
using TourPlanner.ViewModels;

namespace TourPlanner.Stores {
    public class LogStore {
        private TourLog? _currentLog;

        public TourLog? CurrentLog {
            get => _currentLog;
            set {
                _currentLog = value;
            }
        }

    }
}
