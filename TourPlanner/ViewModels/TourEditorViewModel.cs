using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class TourEditorViewModel : ViewModelBase
    {
        private ObservableCollection<TransportType> _transportTypes;
        private TransportType _selectedTransportType;
        private TourViewModel? _tour;

        public TourViewModel? Tour {
            get { return _tour; }
            set {
                _tour = value; 
                OnPropertyChanged();
            }
        }

        private readonly TourManager _tourManager;
        public TourEditorViewModel(IServiceProvider serviceProvider, Guid? id = null) {
            _tourManager = serviceProvider.GetRequiredService<TourManager>();

            //Initialize Transport Types
            _transportTypes = new ObservableCollection<TransportType>(Enum.GetValues(typeof(TransportType)).Cast<TransportType>());
            _selectedTransportType = _transportTypes.FirstOrDefault();


            ToOverViewCommand = new NavigateCommand<TourOverViewModel>(
                    serviceProvider.GetService<INavigationService<TourOverViewModel>>()
                );

            SaveTourCommand = new SaveTourCommand(this, serviceProvider);

            UpdateTourCommand = new SaveEditedTourCommand(this, serviceProvider);

            ImportTourCommand = new ImportFileCommand(serviceProvider);

            if (id.HasValue) {
                LoadTourCommand = new LoadTourCommand(this, serviceProvider, id.Value);
            }
        }

        public static TourEditorViewModel LoadWithId(IServiceProvider serviceProvider, Guid id) {
            TourEditorViewModel viewModel = new TourEditorViewModel(serviceProvider, id);
            viewModel.LoadTourCommand.Execute(null);
            return viewModel;
        }

        public IEnumerable<TransportType> TransportTypes => _transportTypes;

        public TransportType SelectedTransportType {
            get => _selectedTransportType;
            set {
                _selectedTransportType = value;
                OnPropertyChanged();
            }
        }

        public ICommand ToOverViewCommand { get; }
        public ICommand SaveTourCommand { get; }
        public ICommand UpdateTourCommand { get; }
        public ICommand LoadTourCommand { get;  }
        public ICommand ImportTourCommand { get; }


        private string? _tourName;
        public string? TourName {
            get { return _tourName; }
            set { 
                _tourName = value;
                OnPropertyChanged();
            }
        }

        private string? _tourDescription;
        public string? TourDescription {
            get { return _tourDescription; }
            set {
                _tourDescription = value;
                OnPropertyChanged();
            }
        }

        private string? _tourFrom;
        public string? TourFrom {
            get { return _tourFrom; }
            set {
                _tourFrom = value;
                OnPropertyChanged();
            }
        }

        private string? _tourTo;
        public string? TourTo {
            get { return _tourTo; }
            set {
                _tourTo = value;
                OnPropertyChanged();
            }
        }

        private bool _isLoading;
        public bool IsLoading {
            get { return _isLoading; } 
            set {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        private string _importPath;

        public string ImportPath {
            get { return _importPath; }
            set {
                _importPath = value;
                OnPropertyChanged();
            }
        }

        public void LoadTourModel(Tour t) {
            IsLoading = true;
            Tour = new TourViewModel(t);
            TourName = _tour.Name;
            TourDescription = _tour.Description;
            TourFrom = _tour.From;
            TourTo = _tour.To;
            SelectedTransportType = _tour.TransportType;
            IsLoading = false;
        }
    }
}
