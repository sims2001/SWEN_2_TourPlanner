using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TourPlanner.Stores;
using TourPlanner.ViewModels;
using TourPlanner.Views;

namespace TourPlanner {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        private readonly NavigationStore _navigationStore;

        public App() {
            _navigationStore = new NavigationStore();
        }

        protected override void OnStartup(StartupEventArgs e) {
            _navigationStore.CurrentViewModel = new TourOverViewModel(_navigationStore, CreateEditorViewModel);

            MainWindow = new MainWindow() {
                DataContext = new MainWindowViewModel(_navigationStore)
            };

            MainWindow.Show();

            base.OnStartup(e);
        }

        private TourEditorViewModel CreateEditorViewModel() {
            return new TourEditorViewModel(_navigationStore, CreateOverViewModel);
        }

        private TourOverViewModel CreateOverViewModel() {
            return new TourOverViewModel(_navigationStore, CreateEditorViewModel);
        }

    }
}
