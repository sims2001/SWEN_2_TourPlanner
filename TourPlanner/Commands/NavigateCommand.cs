﻿using System;
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
        private readonly NavigationService _navigationService;
        public NavigateCommand(NavigationService navigationService) { 
            _navigationService = navigationService;
        }

        public override void Execute(object? parameter) {
            _navigationService.Navigate();
        }
    }
}
