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
    class SaveEditedTourCommand : CommandBase
    {
        private readonly TourManager _tourManager;
        private readonly TourEditorViewModel _tourEditorViewModel;
        private readonly MyOwnNavigationService _myOwnNavigationService;
        public SaveEditedTourCommand(TourEditorViewModel tourEditorViewModel, TourManager tourManager, MyOwnNavigationService myOwnNavigationService) {
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


            var editedTour = _tourManager.GetTour(_tourEditorViewModel.Tour.Id);
            //RouteManager Arbeit machen lassen???
            bool newDirections = (_tourEditorViewModel.TourFrom != editedTour.From ||
                                       _tourEditorViewModel.TourTo != editedTour.To ||
                                       _tourEditorViewModel.SelectedTransportType != editedTour.TransportType);

            try {
                if (newDirections) {
                    var routeInfo = await OnlineRoute.GetOnlineRoute(_tourEditorViewModel.TourFrom, _tourEditorViewModel.TourTo, _tourEditorViewModel.SelectedTransportType.ToString());
                    editedTour.Time = routeInfo.Time;
                    editedTour.Distance = routeInfo.Distance;
                    editedTour.PicturePath = "C:\\Users\\Simon\\Desktop\\Meme Shit\\alex_zaun.png";// routeInfo.PicPath;
                }


                editedTour.Name = _tourEditorViewModel.TourName != editedTour.Name ? _tourEditorViewModel.TourName : editedTour.Name;
                editedTour.Description = _tourEditorViewModel.TourDescription != editedTour.Description ? _tourEditorViewModel.TourDescription : editedTour.Description;
                editedTour.From = _tourEditorViewModel.TourFrom != editedTour.From ? _tourEditorViewModel.TourFrom : editedTour.From;
                editedTour.To = _tourEditorViewModel.TourTo != editedTour.To ? _tourEditorViewModel.TourTo : editedTour.To;
                editedTour.TransportType = _tourEditorViewModel.SelectedTransportType != editedTour.TransportType ? _tourEditorViewModel.SelectedTransportType : editedTour.TransportType;

                _tourManager.UpdateTour(editedTour.Id, editedTour);

                _tourEditorViewModel.IsLoading = false;
                MessageBox.Show("Successfully Updated Tour", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                _myOwnNavigationService.NavigateTo("overview");

            } catch (RouteNotFoundException ex) {
                _tourEditorViewModel.IsLoading = false;
                MessageBox.Show($"Couldn't Find Locations!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

    }
}
