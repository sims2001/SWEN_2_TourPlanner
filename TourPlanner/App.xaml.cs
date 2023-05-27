using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
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
using TourPlanner.Services.TourCreators;
using TourPlanner.Services.TourProviders;

namespace TourPlanner {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        private readonly TourPlannerDbContext _context;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public App() {

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _configuration = builder.Build();



            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<TourPlannerDbContextFactory>(s => new TourPlannerDbContextFactory(_configuration.GetConnectionString("LocalPostgreSQL")));
            services.AddTransient<DatabaseTourProvider>(s => new DatabaseTourProvider(s));
            services.AddTransient<DatabaseTourEditor>(s => new DatabaseTourEditor(s));


            services.AddSingleton<NavigationStore>();
            services.AddSingleton<TourManager>(s => new TourManager(s));
            services.AddSingleton<OpenFileDialogService>();

            services.AddSingleton<MainWindowViewModel>();

            services.AddTransient<INavigationService<TourOverViewModel>>(s => CreateOverViewNavigationService(s));
            services.AddTransient<INavigationService<TourEditorViewModel>>(s => CreateEditorViewNavigationService(s));
            //services.AddTransient<INavigationService<LogEditorViewModel>>(s => CreateLogEditorViewNavigationService(s));

            services.AddTransient<IParameterNavigationService<Guid, TourEditorViewModel>>(s => CreateEditorParameterNavigationService(s));
            //services.AddTransient<IParameterNavigationService<Guid, LogEditorViewModel>>(s => CreateLogEditorParameterNavigationService(s));

            services.AddSingleton<MainWindow>(s => new MainWindow() {
                DataContext = s.GetRequiredService<MainWindowViewModel>()
            });

            
            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e) {

            using (var dbContext = _serviceProvider.GetRequiredService<TourPlannerDbContextFactory>()
                       .CreateTourPlannerDbContext()) {
                dbContext.Database.Migrate();
            }


            INavigationService<TourOverViewModel> navigation = _serviceProvider.GetService<INavigationService<TourOverViewModel>>();
            navigation.Navigate();

            MainWindow = _serviceProvider.GetRequiredService<MainWindow>();

            MainWindow.Show();

            base.OnStartup(e);
        }


        private INavigationService<TourOverViewModel>
            CreateOverViewNavigationService(IServiceProvider serviceProvider) {
            return new NavigationService<TourOverViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                CreateOverViewModel,
                serviceProvider
            );
        }
        
        private TourOverViewModel CreateOverViewModel(IServiceProvider serviceProvider) {
            return TourOverViewModel.LoadViewModel(serviceProvider);
        }

        private INavigationService<TourEditorViewModel>
            CreateEditorViewNavigationService(IServiceProvider serviceProvider) {
            return new NavigationService<TourEditorViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                CreateEditorViewModel,
                serviceProvider
            );
        }

        private TourEditorViewModel CreateEditorViewModel(IServiceProvider serviceProvider) {
            return new TourEditorViewModel(serviceProvider);
        }
        private TourEditorViewModel CreateEditorIdViewModel(IServiceProvider serviceProvider, Guid id) {
            return TourEditorViewModel.LoadWithId(serviceProvider, id);
        }

        //private INavigationService<LogEditorViewModel>
        //    createEditorViewNavigationService(IServiceProvider serviceProvider) {
        //    return new NavigationService<LogEditorViewModel>(
        //        serviceProvider.GetRequiredService<NavigationStore>(),
        //        CreateLogEditorViewModel,
        //        serviceProvider
        //    );
        //}

        //private LogEditorViewModel CreateLogEditorViewModel(IServiceProvider serviceProvider) {
        //    return new LogEditorViewModel(serviceProvider);
        //}

        //private LogEditorViewModel CreateLogEditorIdViewModel(IServiceProvider serviceProvider, Guid id) {
        //    return new LogEditorViewModel(serviceProvider, id);
        //}

        private IParameterNavigationService<Guid, TourEditorViewModel> CreateEditorParameterNavigationService(IServiceProvider serviceProvider) {
            return new ParameterNavigationService<Guid, TourEditorViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                (parameter) => CreateEditorIdViewModel(serviceProvider, parameter));
        }

        //private IParameterNavigationService<Guid, LogEditorViewModel> CreateLogEditorParameterNavigationService(IServiceProvider serviceProvider) {
        //    return new ParameterNavigationService<Guid, LogEditorIdViewModel>(
        //        serviceProvider.GetRequiredService<NavigationStore>(),
        //        (parameter) => CreateLogEditorIdViewModel(serviceProvider, parameter));
        //}
    }
}
