using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using TourPlanner.Models;
using TourPlanner.Services;
using TourPlanner.ViewModels;

namespace TourPlanner.Commands {
    internal class SaveLogCommand : AsyncCommandBase {
        private readonly INavigationService<TourOverViewModel> _navigationService;
        private readonly LogEditorViewModel _editor;
        private readonly TourManager _tourManager;

        public SaveLogCommand(IServiceProvider serviceProvider, LogEditorViewModel model) {
            _editor = model;
            _navigationService = serviceProvider.GetService<INavigationService<TourOverViewModel>>();
            _tourManager = serviceProvider.GetRequiredService<TourManager>();
        }
        public async override Task ExecuteAsync(object? parameter) {

            var timeFormat = new Regex("[0-9]{2}:[0-5][0-9]:[0-5][0-9]");
            if (!timeFormat.IsMatch(_editor.LogTime)) {
                MessageBox.Show($"Invalid time: {_editor.LogTime}! Format HH:MM:SS", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            
            Guid tourId = _editor.CurrentTour.Id;

            try {


                TourLog newLog = new TourLog() {
                    Id = Guid.NewGuid(),
                    Date = _editor.LogDate.ToUniversalTime(),
                    Comment = _editor.LogComment,
                    Difficulty = _editor.SelectedDifficulty,
                    TotalTime = _editor.LogIntTime(),
                    Rating = _editor.SelectePopularity
                };

                await _tourManager.AddLog(tourId, newLog);

                MessageBox.Show("Successfully Saved Log", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                _navigationService.Navigate();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine(ex.InnerException);
            }

        }
    }
}
