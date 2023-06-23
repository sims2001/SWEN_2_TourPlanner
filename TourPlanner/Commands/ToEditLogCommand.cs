using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Microsoft.Extensions.DependencyInjection;
using TourPlanner.Services;
using TourPlanner.Stores;
using TourPlanner.ViewModels;

namespace TourPlanner.Commands
{
    class ToEditLogCommand<TViewModel> : CommandBase
    where TViewModel : ViewModelBase
    {
        private readonly INavigationService<LogEditorViewModel> _navigationService;
        private readonly TourOverViewModel _editor;
        private readonly LogStore _logStore;
        public ToEditLogCommand(IServiceProvider serviceProvider, TourOverViewModel model) { 
            _navigationService = serviceProvider.GetService<INavigationService<LogEditorViewModel>>();
            _editor = model;
            _logStore = serviceProvider.GetRequiredService<LogStore>();

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
        public override void Execute(object? parameter) {
            _navigationService.Navigate();
        }
    }
}
