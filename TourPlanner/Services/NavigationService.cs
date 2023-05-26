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
        private readonly Func<IServiceProvider, TViewModel> _createViewModel;
        private readonly IServiceProvider _serviceProvider;

        public NavigationService(NavigationStore navigation, Func<IServiceProvider, TViewModel> createViewModel, IServiceProvider provider ) {
            _navigationStore = navigation;
            _createViewModel = createViewModel;
            _serviceProvider = provider;
        }
        
        public void Navigate() {
            _navigationStore.CurrentViewModel = _createViewModel.Invoke(_serviceProvider);
        }

    
    }
}
