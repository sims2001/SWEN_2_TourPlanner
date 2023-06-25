using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TourPlanner.Models;
using TourPlanner.Services;
using TourPlanner.ViewModels;

namespace TourPlanner.Commands
{
    public class ExportFileCommand : AsyncCommandBase
    {
        private readonly MyFileDialogService _dialogService;
        private readonly TourManager _tourManager;
        public ExportFileCommand(IServiceProvider serviceProvider) {
            _dialogService = serviceProvider.GetService<MyFileDialogService>();
            _tourManager = serviceProvider.GetRequiredService<TourManager>();
        }


        public override async Task ExecuteAsync(object? parameter) {
            Guid exportId = (Guid)parameter;

            var jData = await _tourManager.ExportTour(exportId);

            var fp = _dialogService.SaveFileDialog();

            await File.WriteAllTextAsync(fp, jData.ToString());

            MessageBox.Show($"Successfully Exported Tour to: {fp}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
