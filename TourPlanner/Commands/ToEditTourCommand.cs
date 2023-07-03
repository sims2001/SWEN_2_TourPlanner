using Microsoft.Extensions.DependencyInjection;
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


        private readonly NavigationStore _navigationStore;
        
        private readonly INavigationService<TourEditorViewModel> _navigationService;
        private readonly TourOverViewModel _model;
        private readonly TourStore _tourStore;
        public ToEditTourCommand(TourOverViewModel model,IServiceProvider serviceProvider) {
            _navigationService = serviceProvider.GetService<INavigationService<TourEditorViewModel>>();
            _tourStore = serviceProvider.GetRequiredService<TourStore>();
            _model = model;
        }

        public override void Execute(object? parameter) {
            var id = (Guid) parameter;

            _tourStore.CurrentTour = _model.AllTours.FirstOrDefault(s => s.Id == id);
            
            _navigationService.Navigate();
        }
    }
}
