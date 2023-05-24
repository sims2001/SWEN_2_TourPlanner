using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Stores;
using TourPlanner.ViewModels;

namespace TourPlanner.Services
{
    public class NavigationService
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<ViewModelBase> _viewModelFactory;
        public NavigationService(NavigationStore navigationStore, Func<ViewModelBase> viewModelFactory) {
            _navigationStore = navigationStore;
            _viewModelFactory = viewModelFactory;
        }
        public void Navigate() {
            _navigationStore.CurrentViewModel = _viewModelFactory();
        }
    }
}
