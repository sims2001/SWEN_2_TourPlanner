using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TourPlanner.Exceptions;
using TourPlanner.Logging;
using TourPlanner.Models;
using TourPlanner.Services;
using TourPlanner.ViewModels;

namespace TourPlanner.Commands
{
    public class ImportFileCommand : AsyncCommandBase {
        private readonly TourManager _tourManager;
        private readonly INavigationService<TourOverViewModel> _navigationService;
        private readonly LanguageService _languageService;
        private readonly ILoggerWrapper _logger;
        public ImportFileCommand(IServiceProvider serviceProvider) {
            _tourManager = serviceProvider.GetRequiredService<TourManager>();
            _navigationService = serviceProvider.GetService<INavigationService<TourOverViewModel>>();
            _languageService = serviceProvider.GetRequiredService<LanguageService>();
            _logger = LoggerFactory.GetLogger(serviceProvider.GetService<IConfiguration>());
        }

        public override async Task ExecuteAsync(object? parameter) {

            try {
                var fp = MyFileDialogService.OpenFileDialog();

                if (string.IsNullOrEmpty(fp))
                    return;

                if (!fp.EndsWith(".json"))
                    throw new InvalidFileTypeException();

                var fc = await File.ReadAllTextAsync(fp);

                if (await _tourManager.ImportTour(fc))
                    _navigationService.Navigate();
            }
            catch (InvalidFileTypeException ex) {
                MessageBox.Show(_languageService.getVariable("message_invalid_file"),
                    _languageService.getVariable("message_error"), MessageBoxButton.OK, MessageBoxImage.Error);
                _logger.Warn("User selected wrong filetyp for report");
            }
        }
    }
}
