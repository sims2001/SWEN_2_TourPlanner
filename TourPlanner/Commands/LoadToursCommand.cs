using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using TourPlanner.Models;
using TourPlanner.ViewModels;

namespace TourPlanner.Commands {
    public class LoadToursCommand : AsyncCommandBase {

        private readonly TourManager _manager;
        private readonly TourOverViewModel _viewModel;

        public LoadToursCommand(IServiceProvider serviceProvider, TourOverViewModel viewModel) {
            _manager = serviceProvider.GetRequiredService<TourManager>();
            _viewModel = viewModel;
        }

        public override async Task ExecuteAsync(object? parameter) {
            try {
                IEnumerable<Tour> tours = await _manager.GetAllTours();
                _viewModel.UpdateTours(tours);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
