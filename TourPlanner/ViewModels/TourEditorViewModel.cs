using CommunityToolkit.Mvvm.ComponentModel;
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
    class TourEditorViewModel : ViewModelBase
    {
        private ObservableCollection<TransportType> _transportTypes;
        private TransportType? _selectedTransportType;
        private TourViewModel _tour;

        public TourEditorViewModel(NavigationService overViewNavigationService) {
            _transportTypes = new ObservableCollection<TransportType>();

            foreach (var transportType in Enum.GetValues(typeof(TransportType)).Cast<TransportType>()) {
                _transportTypes.Add(transportType);
            }
            
            _selectedTransportType = _transportTypes.FirstOrDefault();

            ToOverViewCommand = new NavigateCommand(overViewNavigationService);
            SaveTourCommand = new SaveTourCommand();
        }

        public IEnumerable<TransportType> TransportTypes => _transportTypes;

        public TransportType? SelectedTransportType {
            get => _selectedTransportType;
            set {
                _selectedTransportType = value;
                OnPropertyChanged();
            }
        }

        public ICommand ToOverViewCommand { get; }
        public ICommand SaveTourCommand { get; }


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

    }
}
