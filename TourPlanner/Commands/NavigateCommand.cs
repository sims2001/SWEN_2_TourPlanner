using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Services;
using TourPlanner.Stores;
using TourPlanner.ViewModels;

namespace TourPlanner.Commands
{
    class NavigateCommand<TViewModel> : CommandBase
    where TViewModel : ViewModelBase
    {
        private readonly NavigationService<TViewModel> _navigationService;
        public NavigateCommand(NavigationService<TViewModel> navigationService) { 
            _navigationService = navigationService;
        }

        public override void Execute(object? parameter) {
            _navigationService.Navigate();
        }
    }
}
