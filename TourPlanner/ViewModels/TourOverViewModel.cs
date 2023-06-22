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
        private readonly TourStore _tourStore;
        private readonly LogStore _logStore;
        private ObservableCollection<TourViewModel> _allTours;
        private TourViewModel? _currentTour => _tourStore.CurrentTour;
        private ObservableCollection<TourLog>? _currentTourLogs => (ObservableCollection<TourLog>?)_currentTour?.Logs;

        public TourOverViewModel(IServiceProvider serviceProvider) {
            _allTours = new ObservableCollection<TourViewModel>();
            _tourStore = serviceProvider.GetRequiredService<TourStore>();
            _tourStore.CurrentTour = null;

            _logStore = serviceProvider.GetRequiredService<LogStore>();
            _logStore.CurrentLog = null;

            NewTourCommand = new NavigateCommand<TourEditorViewModel>(
                    serviceProvider.GetService<INavigationService<TourEditorViewModel>>()
                );

            DeleteTourCommand = new DeleteTourCommand(serviceProvider);


            EditTourCommand =
                new ToEditTourCommand(serviceProvider);

            LoadToursCommand = new LoadToursCommand(serviceProvider, this);

            NewLogCommand = new ToNewLogCommand<LogEditorViewModel>(
                    serviceProvider.GetService<INavigationService<LogEditorViewModel>>(),
                    serviceProvider
                );
        }

        public static TourOverViewModel LoadViewModel(IServiceProvider serviceProvider) {
            TourOverViewModel viewModel = new TourOverViewModel(serviceProvider);
            viewModel.LoadToursCommand.Execute(null);
            return viewModel;
        }


        public IEnumerable<TourViewModel> AllTours => _allTours;

        public TourViewModel? CurrentTour {
            get { return _currentTour; }
            set {
                _tourStore.CurrentTour = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<TourLog> AllTourLogs => _currentTourLogs;

        public ICommand EditTourCommand { get; }
        public ICommand DeleteTourCommand { get; }

        public ICommand NewTourCommand { get; }
        public ICommand LoadToursCommand { get; }
        public ICommand NewLogCommand { get; }
        public ViewModelBase CurrentViewModel => this;

        public void UpdateTours(IEnumerable<Tour> tours) {
            _allTours.Clear();
            foreach (var t in tours) {
                _allTours.Add(new TourViewModel(t));
            }
        }
    }
}
