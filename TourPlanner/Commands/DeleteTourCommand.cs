using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Castle.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TourPlanner.Logging;
using TourPlanner.Models;
using TourPlanner.Services;
using TourPlanner.ViewModels;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace TourPlanner.Commands {
    public class DeleteTourCommand : AsyncCommandBase
    {
        private readonly TourManager _manager;
        private readonly INavigationService<TourOverViewModel> _navigationService;
        private readonly ILoggerWrapper _logger;
        private readonly LanguageService _languageService;

        public DeleteTourCommand(IServiceProvider serviceProvider){ 
            _manager = serviceProvider.GetRequiredService<TourManager>();
            _navigationService = serviceProvider.GetRequiredService<INavigationService<TourOverViewModel>>();
            _logger = LoggerFactory.GetLogger(serviceProvider.GetService<IConfiguration>());
            _languageService = serviceProvider.GetRequiredService<LanguageService>();
        }

        public override async Task ExecuteAsync(object? parameter) {

            if(MessageBox.Show(_languageService.getVariable("message_delete_tour"), _languageService.getVariable("caption_delete_tour"), MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes) {
                Guid id = (Guid)parameter;
                try {
                    await _manager.DeleteTour(id);
                    MessageBox.Show(_languageService.getVariable("message_success_delete"), _languageService.getVariable("caption_success"), MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                catch (Exception ex) {
                    MessageBox.Show(_languageService.getVariable("message_error_delete"), _languageService.getVariable("caption_error"), MessageBoxButton.OK, MessageBoxImage.Error);
                    _logger.Error("Couldn't Save Tour: ", ex);
                }
                finally {
                    _navigationService.Navigate();
                }
            
            }

        }
    }
}
