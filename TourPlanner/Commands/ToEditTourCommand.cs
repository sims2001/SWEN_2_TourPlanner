using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Models;
using TourPlanner.Services;
using TourPlanner.Stores;
using TourPlanner.ViewModels;

namespace TourPlanner.Commands {
    internal class ToEditTourCommand : CommandBase {

        private readonly TourManager _tourManager;
        private readonly NavigationStore _navigationStore;

        private readonly ParameterNavigationService<Guid, TourEditorViewModel> _parameterNavigationService;
        public ToEditTourCommand(TourManager tourManager, ParameterNavigationService<Guid, TourEditorViewModel> navigationService){ 
            //_navigationService = myOwnNavigationService;
            _tourManager = tourManager;
            //_navigationStore = store;
            _parameterNavigationService = navigationService;
        }

        public override void Execute(object? parameter) {
            var id = (Guid) parameter;

            _parameterNavigationService.Navigate(id);
        }
    }
}
