using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TourPlanner.Models;
using TourPlanner.Services;
using TourPlanner.ViewModels;

namespace TourPlanner.Commands {
    public class DeleteTourCommand : CommandBase
    {
        private readonly TourManager _manager;
        private readonly NavigationService<TourOverViewModel> _navigationService;

        public DeleteTourCommand(TourManager tourManager, NavigationService<TourOverViewModel> myOwnNavigationService) { 
            _manager = tourManager;
            _navigationService = myOwnNavigationService;
        }

        public override void Execute(object? parameter) {

            if(MessageBox.Show("Would you like to delete this Tour?", "Delete Tour?", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes) {
                Guid id = (Guid)parameter;
                _manager.RemoveTour(id);
            
                MessageBox.Show("Successfully Deleted Tour", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                _navigationService.Navigate();
            }

        }
    }
}
