using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using TourPlanner.Commands;
using TourPlanner.Models;
using TourPlanner.Services;
using TourPlanner.Stores;

namespace TourPlanner.ViewModels;

public class LogEditorViewModel : ViewModelBase {
    private readonly TourStore _tourStore;
    private readonly LogStore _logStore;
    public TourViewModel? CurrentTour => _tourStore.CurrentTour;

    private ObservableCollection<Difficulty> _difficulties;
    private Difficulty _selectedDifficulty;

    private ObservableCollection<Popularity> _popularities;
    private Popularity _selectedPopularity;

    public LogEditorViewModel(IServiceProvider serviceProvider, TourLog? log = null) {
        _tourStore = serviceProvider.GetRequiredService<TourStore>();
        _logStore = serviceProvider.GetRequiredService<LogStore>();

        _difficulties = new ObservableCollection<Difficulty>(Enum.GetValues(typeof(Difficulty)).Cast<Difficulty>());
        _selectedDifficulty = _difficulties.FirstOrDefault();

        _popularities = new ObservableCollection<Popularity>(Enum.GetValues(typeof(Popularity)).Cast<Popularity>());
        _selectedPopularity = _popularities.FirstOrDefault();

        _logDate = DateTime.UtcNow;

        if (_log is not null) {
            setLogValues();
        }


        ToOverViewCommand = new NavigateCommand<TourOverViewModel>(
            serviceProvider.GetService<INavigationService<TourOverViewModel>>()
        );
        SaveLogCommand = new SaveLogCommand(serviceProvider, this);
        UpdateLogCommand = new SaveEditedLogCommand(serviceProvider, this);
    }

    private TourLog _log => _logStore.CurrentLog;

    public IEnumerable<Difficulty> Difficulties => _difficulties;
    public Difficulty SelectedDifficulty {
        get => _selectedDifficulty;
        set {
            _selectedDifficulty = value;
            OnPropertyChanged();
        }
    }

    public IEnumerable<Popularity> Popularities => _popularities;
    public Popularity SelectedPopularity {
        get => _selectedPopularity;
        set {
            _selectedPopularity = value;
            OnPropertyChanged();
        }
    }

    public ICommand ToOverViewCommand { get; }
    public ICommand SaveLogCommand { get; }
    public ICommand UpdateLogCommand { get; }

    private TourLog _currentLog => _logStore.CurrentLog;

    public TourLog Log => _currentLog;

    private DateTime _logDate;

    public DateTime LogDate {
        get => _logDate;
        set {
            _logDate = value;
            OnPropertyChanged();
        }
    }

    private string _logComment;

    public string LogComment {
        get => _logComment;
        set {
            _logComment = value;
            OnPropertyChanged();
        }
    }

    private string _logTime;

    public string LogTime {
        get => _logTime;
        set {
            _logTime = value;
            OnPropertyChanged();
        }
    }

    public int LogIntTime() {
        var h = Int32.Parse(_logTime.Substring(0, 2));
        var m = Int32.Parse(_logTime.Substring(3, 2));
        var s = Int32.Parse(_logTime.Substring(6, 2));

        var intTime = s + (m * 60) + (h * 3600);

        return intTime;
    }

    private void setLogValues() {
        LogDate = _log.Date;
        LogComment = _log.Comment;
        SelectedPopularity = _log.Rating;
        SelectedDifficulty = _log.Difficulty;
        LogTime = _log.FormatedTime;
    }
}