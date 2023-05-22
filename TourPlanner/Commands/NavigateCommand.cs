using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Stores;
using TourPlanner.ViewModels;

namespace TourPlanner.Commands
{
    class NavigateCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<ViewModelBase> _viewModelFactory;
        public NavigateCommand(NavigationStore navigationStore, Func<ViewModelBase> viewModelFactory) { 
            _navigationStore = navigationStore;
            _viewModelFactory = viewModelFactory;
        }

        public override void Execute(object? parameter) {
            _navigationStore.CurrentViewModel = _viewModelFactory();
        }
    }
}
