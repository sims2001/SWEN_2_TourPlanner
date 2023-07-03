using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using TourPlanner.Services;
using TourPlanner.Stores;

namespace TourPlanner.ViewModels {
    internal class MainWindowViewModel : ViewModelBase{
        private readonly NavigationStore _navigationStore;
        private readonly LanguageStore _languageStore;
        private readonly LanguageService _languageService;
        private readonly INavigationService<TourOverViewModel> _navigationService;


        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;
        public MainWindowViewModel(IServiceProvider serviceProvider) {
            _navigationStore = serviceProvider.GetRequiredService<NavigationStore>();
            _languageStore = serviceProvider.GetRequiredService<LanguageStore>();
            _languageService = serviceProvider.GetRequiredService<LanguageService>();
            _navigationService = serviceProvider.GetService<INavigationService<TourOverViewModel>>();

            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private string[] _languages => _languageService.getLanguages();
        public IEnumerable<string> Languages => _languages;

        private string _currentLanguage => _languageStore.CurrentLanguage;
        public string CurrentLanguage {
            get => _currentLanguage;
            set {
                _languageStore.CurrentLanguage = value;
                OnPropertyChanged();
                ReloadLanguage();
            }
        }

        private void ReloadLanguage() {
            if (MessageBox.Show(_languageService.getVariable("message_change_language"),
                    _languageService.getVariable("caption_change_language"), MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) == MessageBoxResult.Yes) {
                _navigationService.Navigate();
            }
        }

        private void OnCurrentViewModelChanged() {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }

}
