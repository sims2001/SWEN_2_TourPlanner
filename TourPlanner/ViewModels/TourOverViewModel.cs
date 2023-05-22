using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner.Models;

namespace TourPlanner.ViewModels
{
    class TourOverViewModel : ViewModelBase
    {
        private ObservableCollection<TourViewModel> _allTours;
        private TourViewModel? _currentTour;
        private ViewModelBase _model;

        public TourOverViewModel() {
            _allTours = new ObservableCollection<TourViewModel>();
            GenerateTestTours();

            _model = this;
        }


        public IEnumerable<TourViewModel> AllTours => _allTours;
        public TourViewModel? CurrentTour {
            get { return _currentTour; }
            set {
                _currentTour = value;
                OnPropertyChanged(nameof(CurrentTour));
            }
        }


        public ICommand EditTourCommand { get; }
        public ICommand DeleteTourCommand { get; }


        /// <summary> Get this ViewModel and pass it on to the next View </summary>
        public ViewModelBase CurrentViewModel => _model;

        public void GenerateTestTours() {
            ObservableCollection<TourViewModel> touren = new ObservableCollection<TourViewModel>();

            touren.Add(new TourViewModel(Tour.CreateExampleTour()));
            touren.Add(new TourViewModel(Tour.CreateExampleTour()));
            touren.Add(new TourViewModel(Tour.CreateExampleTour()));

            _allTours = touren;
        }
    }
}
