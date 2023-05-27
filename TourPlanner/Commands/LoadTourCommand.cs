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
    internal class LoadTourCommand : AsyncCommandBase {
        private readonly TourManager _manager;
        private readonly TourEditorViewModel _viewModel;
        private Guid _id;

        public LoadTourCommand(TourEditorViewModel viewModel, IServiceProvider serviceProvider, Guid id) {
            _viewModel = viewModel;
            _manager = serviceProvider.GetRequiredService<TourManager>();
            _id = id;
        }

        public override async Task ExecuteAsync(object? parameter) {
            try {
                Tour tour = await _manager.GetTour(_id);
                _viewModel.LoadTourModel(tour);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
