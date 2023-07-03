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
        private readonly TourStore _tourStore;
        private readonly LanguageService _languageService;

        private ObservableCollection<TransportType> _transportTypes;
        private TransportType _selectedTransportType;
        private TourViewModel? _tour => _tourStore.CurrentTour;

        public TourViewModel? Tour {
            get { return _tour; }
            set {
                _tourStore.CurrentTour = value; 
                OnPropertyChanged();
            }
        }

        public TourEditorViewModel(IServiceProvider serviceProvider, Guid? id = null) {
            _tourStore = serviceProvider.GetRequiredService<TourStore>();
            _languageService = serviceProvider.GetRequiredService<LanguageService>();

            //Initialize Transport Types
            _transportTypes = new ObservableCollection<TransportType>(Enum.GetValues(typeof(TransportType)).Cast<TransportType>());
            _selectedTransportType = _transportTypes.FirstOrDefault();

            if (_tour is not null) {
                setTourValues();
            }


            ToOverViewCommand = new NavigateCommand<TourOverViewModel>(
                    serviceProvider.GetService<INavigationService<TourOverViewModel>>(),
                    serviceProvider
                );

            SaveTourCommand = new SaveTourCommand(this, serviceProvider);

            UpdateTourCommand = new SaveEditedTourCommand(this, serviceProvider);

            ImportTourCommand = new ImportFileCommand(serviceProvider);
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
        
        private void setTourValues() {
            TourName = _tour.Name;
            TourDescription = _tour.Description;
            TourFrom = _tour.From;
            TourTo = _tour.To;
            SelectedTransportType = _tour.TransportType;
        }

        //Labels
        public string NameLabel => _languageService.getVariable("input_tour_name");
        public string DescriptionLabel => _languageService.getVariable("input_tour_description");
        public string FromLabel => _languageService.getVariable("input_tour_from");
        public string ToLabel => _languageService.getVariable("input_tour_to");
        public string TransportLabel => _languageService.getVariable("input_tour_transport");
        public string SaveButton => _languageService.getVariable("button_save_tour");
        public string UpdateButton => _languageService.getVariable("button_update_tour");
        public string ImportButton => _languageService.getVariable("button_import");
        public string LabelEdit => _languageService.getVariable("label_edit_tour");
        public string LabelImport => _languageService.getVariable("label_import_tour");
    }
}
