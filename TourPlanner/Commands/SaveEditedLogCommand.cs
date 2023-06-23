using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using TourPlanner.Models;
using TourPlanner.Services;
using TourPlanner.Stores;
using TourPlanner.ViewModels;

namespace TourPlanner.Commands {
    internal class SaveEditedLogCommand : AsyncCommandBase {
        private readonly INavigationService<TourOverViewModel> _navigationService;
        private readonly LogEditorViewModel _editor;
        private readonly TourManager _tourManager;
        private readonly LogStore _logStore;
        private readonly Regex timeFormat = new Regex("[0-9]{2}:[0-5][0-9]:[0-5][0-9]");

        public SaveEditedLogCommand(IServiceProvider serviceProvider, LogEditorViewModel model) {
            _editor = model;
            _navigationService = serviceProvider.GetService<INavigationService<TourOverViewModel>>();
            _tourManager = serviceProvider.GetRequiredService<TourManager>();
            _logStore = serviceProvider.GetRequiredService<LogStore>();

            _editor.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object? parameter) {

            bool canExecute = (!string.IsNullOrEmpty(_editor.LogTime)
                               && !string.IsNullOrEmpty(_editor.LogComment)
                               && timeFormat.IsMatch(_editor.LogTime)
                );

            return canExecute && base.CanExecute(parameter);
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == nameof(_editor.LogTime)
                || e.PropertyName == nameof(_editor.LogComment)) {
                OnCanExecuteChanged();
            }
        }


        public async override Task ExecuteAsync(object? parameter) {
            
            try {
                var newLog = _logStore.CurrentLog;

                newLog.TotalTime = _editor.LogIntTime();
                newLog.Comment = _editor.LogComment;
                newLog.Date = _editor.LogDate;
                newLog.Rating = _editor.SelectedPopularity;
                newLog.Difficulty = _editor.SelectedDifficulty;

                await _tourManager.UpdateLog(newLog);

                MessageBox.Show("Successfully Updated Log", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                _navigationService.Navigate();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine(ex.InnerException);
            }

        }
    }
}
