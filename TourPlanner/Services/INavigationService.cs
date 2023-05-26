using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.ViewModels;

namespace TourPlanner.Services {
    public interface INavigationService<TViewModel> where TViewModel : ViewModelBase{
        void Navigate();
    }
}
