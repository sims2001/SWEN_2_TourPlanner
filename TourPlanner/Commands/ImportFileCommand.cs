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
    public class ImportFileCommand : AsyncCommandBase {
        private readonly TourManager _tourManager;
        private readonly INavigationService<TourOverViewModel> _navigationService;
        public ImportFileCommand(IServiceProvider serviceProvider) {
            _tourManager = serviceProvider.GetRequiredService<TourManager>();
            _navigationService = serviceProvider.GetService<INavigationService<TourOverViewModel>>();
        }
        
        public override async Task ExecuteAsync(object? parameter) {
            var fp = MyFileDialogService.OpenFileDialog();

            var fc = await File.ReadAllTextAsync(fp);

            await _tourManager.ImportTour(fc);

            MessageBox.Show($"Successfully Imported Tour from: {fp}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);


            _navigationService.Navigate();
        }
    }
}
