using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
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

        private TourManager _manager;

        public TourOverViewModel(IServiceProvider serviceProvider) {
            _allTours = new ObservableCollection<TourViewModel>();
            _manager = serviceProvider.GetRequiredService<TourManager>();

            foreach(var t in _manager.GetAllTours()) {
                _allTours.Add(new TourViewModel(t));
            }

            _model = this;

            NewTourCommand = new NavigateCommand<TourEditorViewModel>(
                    serviceProvider.GetService<INavigationService<TourEditorViewModel>>()
                );

            DeleteTourCommand = new DeleteTourCommand(serviceProvider);


            IParameterNavigationService<Guid, TourEditorViewModel> parameterNavigationService =
                serviceProvider.GetService<IParameterNavigationService<Guid, TourEditorViewModel>>();

            EditTourCommand =
                new ToEditTourCommand(serviceProvider); 
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
