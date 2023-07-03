using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Configuration;
using TourPlanner.Exceptions;
using TourPlanner.Logging;
using TourPlanner.Models;
using TourPlanner.Services;
using TourPlanner.ViewModels;

namespace TourPlanner.Commands
{
    public class GenerateSummarizeReportCommand : AsyncCommandBase {
        private readonly TourManager _tourManager;
        private readonly LanguageService _languageService;
        private readonly ILoggerWrapper _logger;
        public GenerateSummarizeReportCommand(IServiceProvider serviceProvider) {
            _tourManager = serviceProvider.GetRequiredService<TourManager>();
            _languageService = serviceProvider.GetRequiredService<LanguageService>();
            _logger = LoggerFactory.GetLogger(serviceProvider.GetService<IConfiguration>());
        }


        public override async Task ExecuteAsync(object? parameter) {

            var t = await _tourManager.GetAllTours();

            try {
                var fp = MyFileDialogService.SavePdfFileDialog();

                if (string.IsNullOrEmpty(fp))
                    return;

                if (! fp.EndsWith(".pdf"))
                    throw new InvalidFileTypeException();


                ReportingService.GenerateSummarizeReport(fp, t);

                MessageBox.Show(_languageService.getVariable("message_success_summarize_report"), _languageService.getVariable("caption_success"), MessageBoxButton.OK, MessageBoxImage.Information);
                _logger.Info($"Successfully Generated Report at: {fp}");
            } catch (InvalidFileTypeException ex) {
                MessageBox.Show(_languageService.getVariable("message_invalid_file"),
                    _languageService.getVariable("message_error"), MessageBoxButton.OK, MessageBoxImage.Error);
                _logger.Warn("User selected wrong filetyp for report");
            } catch (Exception ex) {
                MessageBox.Show(_languageService.getVariable("message_error"),
                    _languageService.getVariable("message_error"), MessageBoxButton.OK, MessageBoxImage.Error);
                _logger.Error("Couldn't export Tour!", ex);
            }
        }
    }
}
