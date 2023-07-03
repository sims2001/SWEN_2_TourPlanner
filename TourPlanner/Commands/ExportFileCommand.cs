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
    public class ExportFileCommand : AsyncCommandBase {
        private readonly TourManager _tourManager;
        private readonly LanguageService _languageService;
        public ExportFileCommand(IServiceProvider serviceProvider) {
            _tourManager = serviceProvider.GetRequiredService<TourManager>();
            _languageService = serviceProvider.GetRequiredService<LanguageService>();
        }


        public override async Task ExecuteAsync(object? parameter) {
            Guid exportId = (Guid)parameter;

            var jData = await _tourManager.ExportTour(exportId);

            var fp = MyFileDialogService.SaveJsonFileDialog();

            if(string.IsNullOrEmpty(fp))
                return;

            try {
                if (!fp.EndsWith(".json"))
                    throw new InvalidFileTypeException();

                await File.WriteAllTextAsync(fp, jData.ToString());

                MessageBox.Show(_languageService.getVariable("message_exported"), _languageService.getVariable("caption_success"), MessageBoxButton.OK,
                    MessageBoxImage.Information);

            }
            catch (InvalidFileTypeException ex) {
                MessageBox.Show(_languageService.getVariable("message_invalid_file"),
                    _languageService.getVariable("message_error"), MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex) {
                MessageBox.Show(_languageService.getVariable("message_error"),
                    _languageService.getVariable("message_error"), MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
