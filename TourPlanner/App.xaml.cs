using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using TourPlanner.DbContexts;
using TourPlanner.Services;
using TourPlanner.Stores;
using TourPlanner.ViewModels;
using TourPlanner.Views;
using TourPlanner.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TourPlanner {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        private readonly NavigationStore _navigationStore;
        private readonly TourManager _tourManager;
        private readonly TourPlannerDbContext _context;
        private readonly IServiceProvider _serviceProvider;
        public IConfiguration Configuration { get; private set; }

        public App() {
            //IServiceCollection services = new ServiceCollection();

            //services.AddSingleton<NavigationStore>();
            //services.AddSingleton<TourManager>();
            //services.AddSingleton<MyOwnNavigationService>();


            //services.AddSingleton<MainWindowViewModel>();


            _navigationStore = new NavigationStore();
            _tourManager = new TourManager();


            // var options = new DbContextOptionsBuilder().UseNpgsql("postgresql://localhost:5432");
            // _context = new TourPlannerDbContext( options. );
        }

        protected override void OnStartup(StartupEventArgs e) {

            for (int i = 0; i < 1; i++) {
                _tourManager.AddTour( Tour.CreateExampleTour() );
                _tourManager.AddTour( Tour.CreateExampleTour() );
                _tourManager.AddTour( Tour.CreateExampleTour() );
            }

            _navigationStore.CurrentViewModel = new TourOverViewModel(_tourManager, _navigationStore);
            
            MainWindow = new MainWindow() {
                DataContext = new MainWindowViewModel(_navigationStore)
            };

            MainWindow.Show();

            base.OnStartup(e);
        }
        

    }
}
