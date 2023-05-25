using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TourPlanner.Exceptions;
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

        public override async void Execute(object? parameter) {
            _tourEditorViewModel.IsLoading = true;

            try {
                
                //RouteManager Arbeit machen lassen???

                var routeInfo = await OnlineRoute.GetOnlineRoute(_tourEditorViewModel.TourFrom, _tourEditorViewModel.TourTo, _tourEditorViewModel.SelectedTransportType.ToString());


                var NewTour = new Tour {
                    Id = Guid.NewGuid(),
                    Name = _tourEditorViewModel.TourName,
                    Description = _tourEditorViewModel.TourDescription,
                    From = _tourEditorViewModel.TourFrom,
                    To = _tourEditorViewModel.TourTo,
                    TransportType = _tourEditorViewModel.SelectedTransportType,
                    Time = routeInfo.Time,
                    Distance = routeInfo.Distance,
                    PicturePath = routeInfo.PicPath, //"C:\\Users\\Simon\\Desktop\\Meme Shit\\alex_zaun.png",
                };
                _tourManager.AddTour(NewTour);

                _tourEditorViewModel.IsLoading = false;
                MessageBox.Show("Successfully Saved Tour", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                _myOwnNavigationService.NavigateTo("overview");

            } catch (RouteNotFoundException ex) {
                _tourEditorViewModel.IsLoading = false;
                MessageBox.Show($"Couldn't Find From or To!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

    }
}
