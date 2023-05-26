using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TourPlanner.Services;
using TourPlanner.Stores;
using TourPlanner.ViewModels;
using TourPlanner.Views;
using TourPlanner.Models;

namespace TourPlanner {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        private readonly NavigationStore _navigationStore;
        private readonly TourManager _tourManager;
        private readonly MyOwnNavigationService _myOwnNavigationService;

        public App() {
            _navigationStore = new NavigationStore();
            _tourManager = new TourManager();
            _myOwnNavigationService = new MyOwnNavigationService(_navigationStore);
        }

        protected override void OnStartup(StartupEventArgs e) {
            _myOwnNavigationService.RegisterRoute("overview", CreateOverViewModel);
            _myOwnNavigationService.RegisterRoute("toureditor", CreateEditorViewModel);

            for (int i = 0; i < 1; i++) {
                _tourManager.AddTour( Tour.CreateExampleTour() );
                _tourManager.AddTour( Tour.CreateExampleTour() );
                _tourManager.AddTour( Tour.CreateExampleTour() );
            }

            _navigationStore.CurrentViewModel = new TourOverViewModel(_tourManager, _myOwnNavigationService, _navigationStore);
            
            MainWindow = new MainWindow() {
                DataContext = new MainWindowViewModel(_navigationStore)
            };

            MainWindow.Show();

            base.OnStartup(e);
        }

        private TourEditorViewModel CreateEditorViewModel() {
            return new TourEditorViewModel(_tourManager, _myOwnNavigationService);
        }


        private TourOverViewModel CreateOverViewModel() {
            return new TourOverViewModel(_tourManager, _myOwnNavigationService, _navigationStore);
        }

    }
}
