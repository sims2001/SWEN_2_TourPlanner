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

        private readonly IParameterNavigationService<Guid, TourEditorViewModel> _parameterNavigationService;
        public ToEditTourCommand(IServiceProvider serviceProvider){
            _parameterNavigationService = serviceProvider.GetService<IParameterNavigationService<Guid, TourEditorViewModel>>();
        }

        public override void Execute(object? parameter) {
            var id = (Guid) parameter;
            Console.WriteLine(id);
            _parameterNavigationService.Navigate(id);
        }
    }
}
