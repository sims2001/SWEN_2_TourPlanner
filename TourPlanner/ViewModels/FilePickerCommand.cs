using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Commands;
using TourPlanner.Services;

namespace TourPlanner.ViewModels {
    public class FilePickerCommand : CommandBase {
        private readonly TourEditorViewModel _viewModel;
        private readonly OpenFileDialogService _dialogService;
        public FilePickerCommand(TourEditorViewModel viewModel, IServiceProvider serviceProvider) {
            _viewModel = viewModel;
            _dialogService = serviceProvider.GetService<OpenFileDialogService>();
        }

        public override void Execute(object? parameter) {
            Console.WriteLine("Picking File!");
            _viewModel.ImportPath = _dialogService.OpenFileDialog();
        }
    }
}
