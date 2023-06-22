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
    class ToNewLogCommand<TViewModel> : CommandBase
    where TViewModel : ViewModelBase
    {
        private readonly INavigationService<TViewModel> _navigationService;
        private readonly LogStore _logStore;
        public ToNewLogCommand(INavigationService<TViewModel> navigationService, IServiceProvider serviceProvider) { 
            _navigationService = navigationService;
            _logStore = serviceProvider.GetRequiredService<LogStore>();
        }

        public override void Execute(object? parameter) {
            _logStore.CurrentLog = null;

            _navigationService.Navigate();
        }
    }
}
