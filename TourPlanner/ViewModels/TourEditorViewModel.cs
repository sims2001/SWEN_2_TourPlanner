using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner.Commands;
using TourPlanner.Stores;

namespace TourPlanner.ViewModels
{
    class TourEditorViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        public TourEditorViewModel(NavigationStore navigationStore, Func<ViewModelBase> createOverViewModel) { 
            _navigationStore = navigationStore;
            ToOverViewCommand = new NavigateCommand(navigationStore, createOverViewModel);
        }

        public ICommand ToOverViewCommand { get; }

    }
}
