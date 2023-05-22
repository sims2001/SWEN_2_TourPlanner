using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TourPlanner.ViewModels {
    internal class MainWindowViewModel : ViewModelBase{
        public ViewModelBase CurrentViewModel { get; }
        public MainWindowViewModel() {
            CurrentViewModel = new TourOverViewModel();
        }
        public ICommand ToEditorCommand { get; }
    }

}
