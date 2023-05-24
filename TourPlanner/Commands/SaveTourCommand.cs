using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TourPlanner.Models;
using TourPlanner.Services;
using TourPlanner.ViewModels;

namespace TourPlanner.Commands
{
    class SaveTourCommand : CommandBase
    {
        private readonly TourManager _tourManager;
        private readonly TourEditorViewModel _tourEditorViewModel;
        private readonly MyOwnNavigationService _myOwnNavigationService;
        public SaveTourCommand(TourEditorViewModel tourEditorViewModel, TourManager tourManager, MyOwnNavigationService myOwnNavigationService) {
            _tourEditorViewModel = tourEditorViewModel;
            _tourManager = tourManager;
            _myOwnNavigationService = myOwnNavigationService;

            _tourEditorViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object? parameter) {
            bool canExecute = (!string.IsNullOrEmpty(_tourEditorViewModel.TourName) 
                && !string.IsNullOrEmpty(_tourEditorViewModel.TourFrom)
                && !string.IsNullOrEmpty(_tourEditorViewModel.TourTo)
                );

            return canExecute && base.CanExecute(parameter);
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e) {
            if(e.PropertyName == nameof(_tourEditorViewModel.TourName)
                || e.PropertyName == nameof(_tourEditorViewModel.TourFrom)
                || e.PropertyName == nameof(_tourEditorViewModel.TourTo) ) {
                OnCanExecuteChanged();
            }
        }

        public override void Execute(object? parameter) {
            try {
                OnlineRoute routeInfo = new OnlineRoute(_tourEditorViewModel.TourFrom, _tourEditorViewModel.TourTo, _tourEditorViewModel.SelectedTransportType.ToString());


                _tourManager.AddTour(new Tour {
                    Id = new Guid(),
                    Name = _tourEditorViewModel.TourName,
                    Description = _tourEditorViewModel.TourDescription,
                    From = _tourEditorViewModel.TourFrom,
                    To = _tourEditorViewModel.TourTo,
                    TransportType = _tourEditorViewModel.SelectedTransportType,
                    Time = routeInfo.Time,
                    Distance = routeInfo.Distance,
                });

                MessageBox.Show("Successfully Saved Tour", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                _myOwnNavigationService.NavigateTo("overview");

            } catch (Exception ex) {
                MessageBox.Show("Couldn't Find From or To", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

    }
}
