using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
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
    public class GenerateSingleReportCommand : AsyncCommandBase {
        private readonly TourManager _tourManager;
        private readonly LanguageService _languageService;
        public GenerateSingleReportCommand(IServiceProvider serviceProvider) {
            _tourManager = serviceProvider.GetRequiredService<TourManager>();
            _languageService = serviceProvider.GetRequiredService<LanguageService>();
        }


        public override async Task ExecuteAsync(object? parameter) {
            Guid id = (Guid)parameter;

            var t = await _tourManager.GetCompleteTour(id);


            try {
                var fp = MyFileDialogService.SavePdfFileDialog();

                if (string.IsNullOrEmpty(fp))
                    return;

                if (!fp.EndsWith(".pdf"))
                    throw new InvalidFileTypeException();

                ReportingService.GenerateReport(fp, t);

                MessageBox.Show(_languageService.getVariable("message_success_default_report"), _languageService.getVariable("caption_success"), MessageBoxButton.OK,
                    MessageBoxImage.Information);
            } catch (InvalidFileTypeException ex) {
                MessageBox.Show(_languageService.getVariable("message_invalid_file"),
                    _languageService.getVariable("message_error"), MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (Exception ex) {
                MessageBox.Show(_languageService.getVariable("message_error"),
                    _languageService.getVariable("message_error"), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
