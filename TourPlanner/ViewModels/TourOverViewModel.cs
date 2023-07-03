using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
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

        private readonly LanguageService _languageService;

        public TourOverViewModel(IServiceProvider serviceProvider) {
            _allTours = new ObservableCollection<TourViewModel>();
            _allLogs = new ObservableCollection<TourLog>();
            _languageService = serviceProvider.GetRequiredService<LanguageService>();

            _tourStore = serviceProvider.GetRequiredService<TourStore>();
            _tourStore.CurrentTour = null;

            _logStore = serviceProvider.GetRequiredService<LogStore>();
            _logStore.CurrentLog = null;

            NewTourCommand = new NavigateCommand<TourEditorViewModel>(
                    serviceProvider.GetService<INavigationService<TourEditorViewModel>>(),
                    serviceProvider
                );

            DeleteTourCommand = new DeleteTourCommand(serviceProvider);


            EditTourCommand =
                new ToEditTourCommand(this, serviceProvider);

            LoadToursCommand = new LoadToursCommand(serviceProvider, this);

            NewLogCommand = new ToNewLogCommand<LogEditorViewModel>(
                    serviceProvider.GetService<INavigationService<LogEditorViewModel>>(),
                    serviceProvider
                );

            EditLogCommand = new ToEditLogCommand<LogEditorViewModel>(serviceProvider, this);
            DeleteLogCommand = new DeleteLogCommand<TourOverViewModel>(serviceProvider, this);

            ExportTourCommand = new ExportFileCommand(serviceProvider);
            GenerateSingleReportCommand = new GenerateSingleReportCommand(serviceProvider);
            GenerateSummarizeReportCommand = new GenerateSummarizeReportCommand(serviceProvider);
        }

        public static TourOverViewModel LoadViewModel(IServiceProvider serviceProvider) {
            TourOverViewModel viewModel = new TourOverViewModel(serviceProvider);
            viewModel.LoadToursCommand.Execute(null);
            return viewModel;
        }


        public IEnumerable<TourViewModel> AllTours => _allTours.Where(s => s.Visible);

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

        private string? _searchString;

        public string? SearchString {
            get => _searchString;
            set {
                _searchString = value;
                OnPropertyChanged();
                SearchTours();
            }
        }

        // Labels
        public string TourLabel => _languageService.getVariable("label_tours");
        public string SearchLabel => _languageService.getVariable("label_search");
        public string InformationLabel => _languageService.getVariable("label_information");
        public string NewTourTooltip => _languageService.getVariable("tooltip_new_tour");
        public string EditTooltip => _languageService.getVariable("tooltip_edit_tour");
        public string DeleteTooltip => _languageService.getVariable("tooltip_delete_tour");
        public string LabelSummarizeReport => _languageService.getVariable("button_summarize_report");
        //Not working yet?
        public string LabelChildFriendly => _currentTour is null || _currentTour.IsChildFriendly
            ? _languageService.getVariable("label_child_friendly")
            : _languageService.getVariable("label_not_child_friendly");

        public string LabelTourFrom => _languageService.getVariable("label_tour_from");
        public string LabelTourTo => _languageService.getVariable("label_tour_to");
        public string LabelTourTransport => _languageService.getVariable("label_tour_transport");
        public string LabelTourTime => _languageService.getVariable("label_tour_time");
        public string LabelTourDistance => _languageService.getVariable("label_tour_distance");
        public string LabelTourAvgTime => _languageService.getVariable("label_tour_avg_time");
        public string LabelTourPopularity => _languageService.getVariable("label_tour_popularity");
        public string LabelGenerateReport => _languageService.getVariable("button_tour_report");
        public string LabelExportTour => _languageService.getVariable("button_tour_export");
        public string TabInformation => _languageService.getVariable("tab_information");
        public string TabImage => _languageService.getVariable("tab_image");
        public string TabLogs => _languageService.getVariable("tab_logs");
        public string LabelNewLog => _languageService.getVariable("button_new_log");
        public string LabelEditLog => _languageService.getVariable("button_edit_log");
        public string LabelDeleteLog => _languageService.getVariable("button_delete_log");
        public string LabelLogDate => _languageService.getVariable("label_log_date");
        public string LabelLogComment => _languageService.getVariable("label_log_comment");
        public string LabelLogDifficulty => _languageService.getVariable("label_log_difficulty");
        public string LabelLogTime => _languageService.getVariable("label_log_time");
        public string LabelLogRating => _languageService.getVariable("label_log_rating");

        // Buttons/Commands
        public ICommand EditTourCommand { get; }
        public ICommand DeleteTourCommand { get; }
        public ICommand NewTourCommand { get; }
        public ICommand LoadToursCommand { get; }
        public ICommand NewLogCommand { get; }
        public ICommand EditLogCommand { get; }
        public ICommand DeleteLogCommand { get; }
        public ICommand ExportTourCommand { get; }
        public ICommand GenerateSingleReportCommand { get; }
        public ICommand GenerateSummarizeReportCommand { get; }
        public ViewModelBase CurrentViewModel => this;


        //Functions for Updating Tours and Logs
        public void UpdateTours(IEnumerable<Tour> tours) {
            _allTours.Clear();
            foreach (var t in tours) {
                _allTours.Add(new TourViewModel(t));
            }
            
            OnPropertyChanged(nameof(AllTours));
        }

        public void UpdateTourLogs() {
            _allLogs.Clear();
            foreach(var t in _currentTour.Logs) {
                _allLogs.Add(t);
            }

        }

        private void SearchTours() {
            if (_searchString == null)
                return;

            try {
                var r = new Regex(_searchString);

                foreach (var t in _allTours) {
                    t.Visible = r.IsMatch(t.Searchstring);
                }

                OnPropertyChanged(nameof(AllTours));
            }
            catch(Exception e) {
                MessageBox.Show("Invalid Input! Try to Search for Full Words only!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                SearchString = null;
            }
        }
    }
}
