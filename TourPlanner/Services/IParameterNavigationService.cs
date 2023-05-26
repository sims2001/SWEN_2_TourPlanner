using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Stores;
using TourPlanner.ViewModels;

namespace TourPlanner.Services {
    internal interface IParameterNavigationService<TParameter, TViewModel> where TViewModel : ViewModelBase {
        public void Navigate(TParameter parameter) {
        }
    }
}
