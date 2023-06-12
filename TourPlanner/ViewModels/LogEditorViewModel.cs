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
    private readonly TourManager _tourManager;
    private readonly TourStore _tourStore;
    public TourViewModel? CurrentTour => _tourStore.CurrentTour;

    private ObservableCollection<Difficulty> _difficulties;
    private Difficulty _selectedDifficulty;

    private ObservableCollection<Popularity> _popularities;
    private Popularity _selectedPopularity;

    public LogEditorViewModel(IServiceProvider serviceProvider, TourLog? log = null) {
        //_tourManager = serviceProvider.GetRequiredService<TourManager>();
        _tourStore = serviceProvider.GetRequiredService<TourStore>();

        _difficulties = new ObservableCollection<Difficulty>(Enum.GetValues(typeof(Difficulty)).Cast<Difficulty>());
        _selectedDifficulty = _difficulties.FirstOrDefault();

        _popularities = new ObservableCollection<Popularity>(Enum.GetValues(typeof(Popularity)).Cast<Popularity>());
        _selectedPopularity = _popularities.FirstOrDefault();

        ToOverViewCommand = new NavigateCommand<TourOverViewModel>(
            serviceProvider.GetService<INavigationService<TourOverViewModel>>()
        );
    }

    public IEnumerable<Difficulty> Difficulties => _difficulties;
    public Difficulty SelectedDifficulty {
        get => _selectedDifficulty;
        set {
            _selectedDifficulty = value;
            OnPropertyChanged();
        }
    }

    public IEnumerable<Popularity> Popularities => _popularities;
    public Popularity SelectePopularity {
        get => _selectedPopularity;
        set {
            _selectedPopularity = value;
            OnPropertyChanged();
        }
    }

    public ICommand ToOverViewCommand { get; }
}