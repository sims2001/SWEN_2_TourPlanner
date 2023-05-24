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
    class NavigateCommand : CommandBase
    {
        private readonly MyOwnNavigationService _navigationService;
        private readonly string _whereTo;
        public NavigateCommand(string wherTo, MyOwnNavigationService navigationService) { 
            _navigationService = navigationService;
            _whereTo = wherTo;
        }

        public override void Execute(object? parameter) {
            _navigationService.NavigateTo(_whereTo);
        }
    }
}
