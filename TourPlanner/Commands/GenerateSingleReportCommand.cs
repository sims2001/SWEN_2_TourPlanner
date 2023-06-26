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
    public class GenerateSingleReportCommand : AsyncCommandBase {
        private readonly TourManager _tourManager;
        public GenerateSingleReportCommand(IServiceProvider serviceProvider) {
            _tourManager = serviceProvider.GetRequiredService<TourManager>();
        }


        public override async Task ExecuteAsync(object? parameter) {
            Guid id = (Guid)parameter;

            var t = await _tourManager.GetCompleteTour(id);

            var fp = MyFileDialogService.SavePdfFileDialog();

            ReportingService.GenerateReport(fp, t);

            MessageBox.Show($"Successfully Saved Report to: {fp}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
