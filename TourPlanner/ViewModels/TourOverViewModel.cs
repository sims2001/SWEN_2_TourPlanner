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

        public TourOverViewModel(IServiceProvider serviceProvider) {
            _allTours = new ObservableCollection<TourViewModel>();
            _allLogs = new ObservableCollection<TourLog>();
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

            EditLogCommand = new ToEditLogCommand<LogEditorViewModel>(serviceProvider, this);
            DeleteLogCommand = new DeleteLogCommand<TourOverViewModel>(serviceProvider, this);

            ExportTourCommand = new ExportFileCommand(serviceProvider);
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
                UpdateTourLogs();
            }
        }

        private ObservableCollection<TourLog>? _allLogs;
        public IEnumerable<TourLog> AllTourLogs => _allLogs;

        private TourLog _selectedLog => _logStore.CurrentLog;

        public TourLog SelectedLog {
            get => _selectedLog;
            set {
                _logStore.CurrentLog = value;
                OnPropertyChanged();
            }
        }

        //Buttons/Commands
        public ICommand EditTourCommand { get; }
        public ICommand DeleteTourCommand { get; }
        public ICommand NewTourCommand { get; }
        public ICommand LoadToursCommand { get; }
        public ICommand NewLogCommand { get; }
        public ICommand EditLogCommand { get; }
        public ICommand DeleteLogCommand { get; }
        public ICommand ExportTourCommand { get; }
        public ViewModelBase CurrentViewModel => this;

        public void UpdateTours(IEnumerable<Tour> tours) {
            _allTours.Clear();
            foreach (var t in tours) {
                _allTours.Add(new TourViewModel(t));
            }
        }

        public void UpdateTourLogs() {
            
            Console.WriteLine(_currentTour);

            _allLogs.Clear();
            foreach(var t in _currentTour.Logs) {
                _allLogs.Add(t);
            }

        }
    }
}
