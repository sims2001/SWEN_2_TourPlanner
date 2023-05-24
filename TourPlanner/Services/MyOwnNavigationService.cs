using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Stores;
using TourPlanner.ViewModels;

namespace TourPlanner.Services {
    public class MyOwnNavigationService {

        private readonly NavigationStore _navigationStore;
        private readonly Dictionary<string, Func<ViewModelBase>> _viewModelDictionary;
        public MyOwnNavigationService(NavigationStore navigation) { 
            _navigationStore = navigation;
            _viewModelDictionary = new Dictionary<string, Func<ViewModelBase>>();
        }

        public void NavigateTo(string whereTo) {
            _navigationStore.CurrentViewModel = _viewModelDictionary[whereTo].Invoke();
        }

        public void RegisterRoute(string name, Func<ViewModelBase> viewModel) {
            _viewModelDictionary.Add(name, viewModel);
        }
    }
}
