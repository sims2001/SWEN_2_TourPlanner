using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TourPlanner.Services;
using TourPlanner.Stores;
using TourPlanner.ViewModels;

namespace TourPlanner.Commands
{
    class NavigateCommand<TViewModel> : CommandBase
    where TViewModel : ViewModelBase
    {
        private readonly INavigationService<TViewModel> _navigationService;
        private readonly TourStore _tourStore;

        public NavigateCommand(INavigationService<TViewModel> navigationService, IServiceProvider serviceProvider) { 
            _navigationService = navigationService;
            _tourStore = serviceProvider.GetRequiredService<TourStore>();
        }

        public override void Execute(object? parameter) {
            _tourStore.CurrentTour = null;
            _navigationService.Navigate();
        }
    }
}
