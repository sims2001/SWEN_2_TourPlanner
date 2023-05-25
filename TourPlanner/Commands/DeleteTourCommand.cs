using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TourPlanner.Models;
using TourPlanner.Services;

namespace TourPlanner.Commands {
    public class DeleteTourCommand : CommandBase
    {
        private readonly TourManager _manager;
        private readonly MyOwnNavigationService _myOwnNavigationService;

        public DeleteTourCommand(TourManager tourManager, MyOwnNavigationService myOwnNavigationService) { 
            _manager = tourManager;
            _myOwnNavigationService = myOwnNavigationService;
        }

        public override void Execute(object? parameter) {

            if(MessageBox.Show("Would you like to delete this Tour?", "Delete Tour?", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes) {
                Guid id = (Guid)parameter;
                _manager.RemoveTour(id);
            
                MessageBox.Show("Successfully Deleted Tour", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                _myOwnNavigationService.NavigateTo("overview");
            }

        }
    }
}
