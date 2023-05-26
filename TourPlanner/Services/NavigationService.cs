using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Stores;
using TourPlanner.ViewModels;

namespace TourPlanner.Services {
    public class NavigationService<TViewModel> : INavigationService<TViewModel> where TViewModel : ViewModelBase{

        private readonly NavigationStore _navigationStore;
        private readonly Func<TViewModel> _createViewModel;

        public NavigationService(NavigationStore navigation, Func<TViewModel> createViewModel) {
            _navigationStore = navigation;
            _createViewModel = createViewModel;
        }

        //public NavigationService(NavigationStore navigationStore, Func<TViewModel, IServiceProvider> makeViewModel) {
        //    _navigationStore = navigationStore;
        //}

        public void Navigate() {
            _navigationStore.CurrentViewModel = _createViewModel();
        }

    
    }
}
