using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    class SaveTourCommand : AsyncCommandBase
    {
        private readonly TourManager _tourManager;
        private readonly TourEditorViewModel _tourEditorViewModel;
        private readonly INavigationService<TourOverViewModel> _navigationService;
        private readonly LanguageService _languageService;
        private readonly ILoggerWrapper _logger;
        private readonly IServiceProvider _serviceProvider;

        public SaveTourCommand(TourEditorViewModel tourEditorViewModel, IServiceProvider serviceProvider) {
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

            try {
                
                var routeInfo = await OnlineRoute.GetOnlineRoute(_tourEditorViewModel.TourFrom, _tourEditorViewModel.TourTo, _tourEditorViewModel.SelectedTransportType.ToString(), _serviceProvider);


                var NewTour = new Tour {
                    Id = Guid.NewGuid(),
                    Name = _tourEditorViewModel.TourName,
                    Description = _tourEditorViewModel.TourDescription,
                    From = _tourEditorViewModel.TourFrom,
                    To = _tourEditorViewModel.TourTo,
                    TransportType = _tourEditorViewModel.SelectedTransportType,
                    Time = routeInfo.Time,
                    Distance = routeInfo.Distance,
                    PicturePath = routeInfo.PicPath, //"C:\\Users\\Simon\\Desktop\\Meme Shit\\alex_zaun.png",  //"C:\\Users\\Simon\\Desktop\\Meme Shit\\alex_zaun.png",
                    Logs = new List<TourLog>()
                };

                _tourManager.AddTour(NewTour);

                _tourEditorViewModel.IsLoading = false;
                MessageBox.Show(_languageService.getVariable("message_success_save_tour"), _languageService.getVariable("caption_success"), MessageBoxButton.OK, MessageBoxImage.Information);
                _navigationService.Navigate();
            } catch (RouteNotFoundException ex) {
                _tourEditorViewModel.IsLoading = false;
                MessageBox.Show(_languageService.getVariable("message_error_locations"), _languageService.getVariable("caption_error"), MessageBoxButton.OK, MessageBoxImage.Error);
                _logger.Error("Couldn't Find Route or Location! ", ex);
            } catch (Exception ex) {
                MessageBox.Show(_languageService.getVariable("message_error_save_tour"), _languageService.getVariable("caption_error"), MessageBoxButton.OK, MessageBoxImage.Error);
                _logger.Error("Couldn't Save Tour: ", ex);
            }

        }

    }
}
