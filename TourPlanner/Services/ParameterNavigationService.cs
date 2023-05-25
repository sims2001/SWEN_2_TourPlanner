using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Stores;
using TourPlanner.ViewModels;

namespace TourPlanner.Services {
    internal class ParameterNavigationService<TParameter, TViewModel> where TViewModel : ViewModelBase 
    {

        private readonly NavigationStore _navigationStore;
        private readonly Func<TParameter, TViewModel> _viewModelFactory;

        public ParameterNavigationService(NavigationStore navigationStore, Func<TParameter, TViewModel> viewModelFactory) {
            _navigationStore = navigationStore;
            _viewModelFactory = viewModelFactory;
        }

        public void Navigate(TParameter parameter) {
            _navigationStore.CurrentViewModel = _viewModelFactory(parameter);
        }
    }
}
