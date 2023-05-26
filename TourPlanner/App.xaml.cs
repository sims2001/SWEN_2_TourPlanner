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
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<NavigationStore>();
            services.AddSingleton<TourManager>();

            services.AddSingleton<MainWindowViewModel>();

            services.AddSingleton<MainWindow>(s => new MainWindow() {
                DataContext = s.GetRequiredService<MainWindowViewModel>()
            });

            _serviceProvider = services.BuildServiceProvider();
            
            // var options = new DbContextOptionsBuilder().UseNpgsql("postgresql://localhost:5432");
            // _context = new TourPlannerDbContext( options. );
        }

        protected override void OnStartup(StartupEventArgs e) {


            INavigationService<TourOverViewModel> navigation = new NavigationService<TourOverViewModel>(
                _serviceProvider.GetRequiredService<NavigationStore>(),
                () => new TourOverViewModel(
                    _serviceProvider.GetRequiredService<TourManager>(),
                    _serviceProvider.GetRequiredService<NavigationStore>()
                ));
            navigation.Navigate();

            MainWindow = _serviceProvider.GetRequiredService<MainWindow>();

            MainWindow.Show();

            base.OnStartup(e);
        }


        private INavigationService<TourOverViewModel>
            createOverViewNavigationService(IServiceProvider serviceProvider) {
            return new NavigationService<TourOverViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                CreateOverViewModel(serviceProvider)
                );
        }

        

        private TourOverViewModel CreateOverViewModel(IServiceProvider serviceProvider) {
            return new TourOverViewModel(
                serviceProvider.GetRequiredService<TourManager>(),
                serviceProvider.GetRequiredService<NavigationStore>()
                );
        }
    }
}
