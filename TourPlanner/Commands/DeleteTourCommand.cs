using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using TourPlanner.Models;
using TourPlanner.Services;
using TourPlanner.ViewModels;

namespace TourPlanner.Commands {
    public class DeleteTourCommand : AsyncCommandBase
    {
        private readonly TourManager _manager;
        private readonly INavigationService<TourOverViewModel> _navigationService;

        public DeleteTourCommand(IServiceProvider serviceProvider){ 
            _manager = serviceProvider.GetRequiredService<TourManager>();
            _navigationService = serviceProvider.GetRequiredService<INavigationService<TourOverViewModel>>();
        }

        public override async Task ExecuteAsync(object? parameter) {

            if(MessageBox.Show("Would you like to delete this Tour?", "Delete Tour?", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes) {
                Guid id = (Guid)parameter;
                await _manager.RemoveTour(id);
            
                MessageBox.Show("Successfully Deleted Tour", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                _navigationService.Navigate();
            }

        }
    }
}
