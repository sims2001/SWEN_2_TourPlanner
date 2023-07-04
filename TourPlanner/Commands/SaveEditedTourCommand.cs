using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TourPlanner.Exceptions;
using TourPlanner.Logging;
using TourPlanner.Models;
using TourPlanner.Services;
using TourPlanner.ViewModels;

namespace TourPlanner.Commands
{
    class SaveEditedTourCommand : AsyncCommandBase
    {
        private readonly TourManager _tourManager;
        private readonly TourEditorViewModel _tourEditorViewModel;
        private readonly INavigationService<TourOverViewModel> _navigationService;
        private readonly LanguageService _languageService;
        private readonly ILoggerWrapper _logger;
        private readonly IServiceProvider _serviceProvider;
        public SaveEditedTourCommand(TourEditorViewModel tourEditorViewModel, IServiceProvider serviceProvider) {
            _tourEditorViewModel = tourEditorViewModel;
            _tourManager = serviceProvider.GetService<TourManager>();
            _navigationService = serviceProvider.GetService<INavigationService<TourOverViewModel>>();
            _languageService = serviceProvider.GetRequiredService<LanguageService>();
            _logger = LoggerFactory.GetLogger(serviceProvider.GetService<IConfiguration>());
            _serviceProvider = serviceProvider;
            _tourEditorViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object? parameter) {
            bool canExecute = (!string.IsNullOrEmpty(_tourEditorViewModel.TourName) 
                && !string.IsNullOrEmpty(_tourEditorViewModel.TourFrom)
                && !string.IsNullOrEmpty(_tourEditorViewModel.TourTo)
                && !string.IsNullOrEmpty(_tourEditorViewModel.TourDescription)
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

        public override async Task ExecuteAsync(object? parameter) {
            _tourEditorViewModel.IsLoading = true;


            var editedTour = await _tourManager.GetTour(_tourEditorViewModel.Tour.Id);
            var oldPic = editedTour.PicturePath;

            //TourManager Arbeit machen lassen???
            bool newDirections = (_tourEditorViewModel.TourFrom != editedTour.From ||
                                       _tourEditorViewModel.TourTo != editedTour.To ||
                                       _tourEditorViewModel.SelectedTransportType != editedTour.TransportType);

            try {
                if (newDirections) {
                    var routeInfo = await OnlineRoute.GetOnlineRoute(_tourEditorViewModel.TourFrom,
                        _tourEditorViewModel.TourTo, _tourEditorViewModel.SelectedTransportType.ToString(), _serviceProvider);
                    editedTour.Time = routeInfo.Time;
                    editedTour.Distance = routeInfo.Distance;
                    editedTour.PicturePath = routeInfo.PicPath;

                    File.Delete(oldPic);
                }


                editedTour.Name = _tourEditorViewModel.TourName != editedTour.Name
                    ? _tourEditorViewModel.TourName
                    : editedTour.Name;
                editedTour.Description = _tourEditorViewModel.TourDescription != editedTour.Description
                    ? _tourEditorViewModel.TourDescription
                    : editedTour.Description;
                editedTour.From = _tourEditorViewModel.TourFrom != editedTour.From
                    ? _tourEditorViewModel.TourFrom
                    : editedTour.From;
                editedTour.To = _tourEditorViewModel.TourTo != editedTour.To
                    ? _tourEditorViewModel.TourTo
                    : editedTour.To;
                editedTour.TransportType = _tourEditorViewModel.SelectedTransportType != editedTour.TransportType
                    ? _tourEditorViewModel.SelectedTransportType
                    : editedTour.TransportType;

                await _tourManager.UpdateTour(editedTour);

                _tourEditorViewModel.IsLoading = false;

                MessageBox.Show(_languageService.getVariable("message_success_tour_update"), _languageService.getVariable("caption_success"), MessageBoxButton.OK, MessageBoxImage.Information);
                _navigationService.Navigate();
            } catch (RouteNotFoundException ex) {
                _tourEditorViewModel.IsLoading = false;
                MessageBox.Show(_languageService.getVariable("message_error_locations"), _languageService.getVariable("caption_error"), MessageBoxButton.OK, MessageBoxImage.Error);
                _logger.Error("Couldn't Find Route or Location! ", ex);
            }
            catch (Exception ex) {
                MessageBox.Show(_languageService.getVariable("message_error_tour_update"), _languageService.getVariable("caption_error"), MessageBoxButton.OK, MessageBoxImage.Error);
                _logger.Error("Couldn't Update Tour: ", ex);
            }


        }

    }
}
