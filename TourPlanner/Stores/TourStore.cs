using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.ViewModels;

namespace TourPlanner.Stores {
    public class TourStore {
        private TourViewModel? _currentTour;

        public TourViewModel? CurrentTour {
            get => _currentTour;
            set { 
                _currentTour = value;
            }
        }

    }
}
