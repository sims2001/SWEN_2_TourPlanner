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
    public class GenerateSummarizeReportCommand : AsyncCommandBase {
        private readonly TourManager _tourManager;
        public GenerateSummarizeReportCommand(IServiceProvider serviceProvider) {
            _tourManager = serviceProvider.GetRequiredService<TourManager>();
        }


        public override async Task ExecuteAsync(object? parameter) {

            var t = await _tourManager.GetAllTours();

            var fp = MyFileDialogService.SavePdfFileDialog();

            ReportingService.GenerateSummarizeReport(fp, t);

            MessageBox.Show($"Successfully Saved Summarize-Report to: {fp}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
