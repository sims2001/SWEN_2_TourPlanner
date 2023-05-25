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
        private readonly MyOwnNavigationService _navigationService;
        private readonly NavigationStore _navigationStore;
        public ToEditTourCommand(TourManager tourManager, MyOwnNavigationService myOwnNavigationService, NavigationStore store) { 
            _navigationService = myOwnNavigationService;
            _tourManager = tourManager;
            _navigationStore = store;
        }

        public override void Execute(object? parameter) {
            var id = (Guid) parameter;

            _navigationStore.CurrentViewModel = new TourEditorViewModel(_tourManager, _navigationService, id);
        }
    }
}
