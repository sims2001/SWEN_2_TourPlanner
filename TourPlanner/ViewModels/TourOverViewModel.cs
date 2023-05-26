using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner.Commands;
using TourPlanner.Models;
using TourPlanner.Services;
using TourPlanner.Stores;

namespace TourPlanner.ViewModels
{
    public class TourOverViewModel : ViewModelBase
    {
        private ObservableCollection<TourViewModel> _allTours;
        private TourViewModel? _currentTour;
        private ViewModelBase _model;
        //private readonly NavigationService _myOwnNavigationService;

        private TourManager _manager;

        public TourOverViewModel(TourManager tourManager, NavigationStore store, IServiceProvider? serviceProvider = null) {
            _allTours = new ObservableCollection<TourViewModel>();
            _manager = tourManager;

            foreach(var t in _manager.GetAllTours()) {
                _allTours.Add(new TourViewModel(t));
            }

            _model = this;
            NewTourCommand = new NavigateCommand<TourEditorViewModel>(
                new NavigationService<TourEditorViewModel>(
                    store, 
                    () => new TourEditorViewModel(_manager, store)));

            DeleteTourCommand = new DeleteTourCommand(_manager, 
                new NavigationService<TourOverViewModel>(
                    store, 
                    () => new TourOverViewModel(tourManager, store)));

            // Pass Id To Edit Tour Command
            ParameterNavigationService<Guid, TourEditorViewModel> parameterNavigationService =
                new ParameterNavigationService<Guid, TourEditorViewModel>(
                    store, 
                    (parameter) => new TourEditorViewModel(tourManager, store, parameter) );
            
            EditTourCommand =
                new ToEditTourCommand(_manager, parameterNavigationService); 
        }

        public IEnumerable<TourViewModel> AllTours => _allTours;

        public TourViewModel? CurrentTour {
            get { return _currentTour; }
            set {
                _currentTour = value;
                OnPropertyChanged();
            }
        }


        public ICommand EditTourCommand { get; }
        public ICommand DeleteTourCommand { get; }

        public ICommand NewTourCommand { get; }

        public ViewModelBase CurrentViewModel => _model;

        public void GenerateTestTours(int anz) {
            ObservableCollection<TourViewModel> touren = new ObservableCollection<TourViewModel>();

            for(int i = 0; i< anz; i++) {
                touren.Add(new TourViewModel(Tour.CreateExampleTour()));
            }
            _allTours = touren;
        }
    }
}
