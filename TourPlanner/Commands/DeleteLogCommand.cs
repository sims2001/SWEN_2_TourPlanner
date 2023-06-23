using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Microsoft.Extensions.DependencyInjection;
using TourPlanner.Models;
using TourPlanner.Services;
using TourPlanner.Stores;
using TourPlanner.ViewModels;

namespace TourPlanner.Commands
{
    class DeleteLogCommand<TViewModel> : AsyncCommandBase
    where TViewModel : ViewModelBase
    {
        private readonly INavigationService<TourOverViewModel> _navigationService;
        private readonly TourOverViewModel _editor;
        private readonly LogStore _logStore;
        private readonly TourManager _tourManager;
        public DeleteLogCommand(IServiceProvider serviceProvider, TourOverViewModel model) { 
            _navigationService = serviceProvider.GetService<INavigationService<TourOverViewModel>>();
            _editor = model;
            _logStore = serviceProvider.GetRequiredService<LogStore>();
            _tourManager = serviceProvider.GetRequiredService<TourManager>();

            _editor.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object? parameter) {

            bool canExecute = (_logStore.CurrentLog is not null);

            return canExecute && base.CanExecute(parameter);
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == nameof(_editor.SelectedLog)) {
                OnCanExecuteChanged();
            }
        }

        public override async Task ExecuteAsync(object? parameter) {
            await _tourManager.DeleteLog(_logStore.CurrentLog);
            _navigationService.Navigate();
        }
    }
}
