﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AutoMapper;
using log4net;
using Microsoft.EntityFrameworkCore;
using TourPlanner.DbContexts;
using TourPlanner.Services;
using TourPlanner.Stores;
using TourPlanner.ViewModels;
using TourPlanner.Views;
using TourPlanner.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using TourPlanner.DTOs;
using TourPlanner.Services.TourCreators;
using TourPlanner.Services.TourProviders;
using TourPlanner.Services.LogEditors;
using TourPlanner.Exceptions;
using TourPlanner.Logging;

namespace TourPlanner {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public App() {

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("languages.json", optional: false, reloadOnChange:true);

            _configuration = builder.Build();

            var autoMapperConfig = new MapperConfiguration(cfg => {
                    cfg.AllowNullCollections = true;
                    cfg.CreateMap<TourLog, LogDTO>()
                        .ForMember(
                            sel => sel.TourDTO, 
                            act => act.MapFrom(
                                src => src.Tour
                                )
                            )
                        .ReverseMap();

                    cfg.CreateMap<TourDTO, Tour>()
                        .ForMember(
                            sel => sel.Logs,
                            act => act.MapFrom(
                                src => src.LogDTOs
                                )
                            )
                        .ReverseMap();

                }
            );

            autoMapperConfig.AssertConfigurationIsValid();

            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<TourPlannerDbContextFactory>(s => new TourPlannerDbContextFactory(s));
            services.AddTransient<DatabaseTourProvider>(s => new DatabaseTourProvider(s));
            services.AddTransient<DatabaseTourEditor>(s => new DatabaseTourEditor(s));
            services.AddTransient<DatabaseLogEditor>(s => new DatabaseLogEditor(s));


            services.AddSingleton<NavigationStore>();
            services.AddSingleton<TourStore>();
            services.AddSingleton<LogStore>();
            services.AddSingleton<LanguageStore>(s => new LanguageStore(s));
            services.AddSingleton<LanguageService>(s => new LanguageService(s, _configuration));
            services.AddSingleton<IConfiguration>(_configuration);

            services.AddSingleton<TourManager>(s => new TourManager(s));
            services.AddSingleton<IMapper>(new Mapper(autoMapperConfig));

            services.AddSingleton<MainWindowViewModel>(s => new MainWindowViewModel(s));

            services.AddTransient<INavigationService<TourOverViewModel>>(s => CreateOverViewNavigationService(s));
            services.AddTransient<INavigationService<TourEditorViewModel>>(s => CreateEditorViewNavigationService(s));
            services.AddTransient<INavigationService<LogEditorViewModel>>(s => CreateLogEditorViewNavigationService(s));
            
            services.AddSingleton<MainWindow>(s => new MainWindow() {
                DataContext = s.GetRequiredService<MainWindowViewModel>()
            });
            
            _serviceProvider = services.BuildServiceProvider();
            
        }

        protected override void OnStartup(StartupEventArgs e) {
            ILoggerWrapper log = LoggerFactory.GetLogger(_configuration);
            log.Debug(" ============ STARTED PROGRAM ============ ");

            //Migrate db on startup
            using (var dbContext = _serviceProvider.GetRequiredService<TourPlannerDbContextFactory>()
                       .CreateTourPlannerDbContext()) {
                dbContext.Database.Migrate();
            }

            //Set Language
            var languageStore = _serviceProvider.GetRequiredService<LanguageStore>();
            languageStore.CurrentLanguage = "English";

            //Set initial Navigation
            INavigationService<TourOverViewModel> navigation = _serviceProvider.GetService<INavigationService<TourOverViewModel>>();
            navigation.Navigate();

            MainWindow = _serviceProvider.GetRequiredService<MainWindow>();

            MainWindow.Show();

            base.OnStartup(e);
        }


        private INavigationService<TourOverViewModel>
            CreateOverViewNavigationService(IServiceProvider serviceProvider) {
            return new NavigationService<TourOverViewModel>(
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
                CreateEditorViewModel,
                serviceProvider
            );
        }

        private TourEditorViewModel CreateEditorViewModel(IServiceProvider serviceProvider) {
            return new TourEditorViewModel(serviceProvider);
        }

        private INavigationService<LogEditorViewModel>  CreateLogEditorViewNavigationService(IServiceProvider serviceProvider) {
            return new NavigationService<LogEditorViewModel>(
                CreateLogEditorViewModel,
                serviceProvider
            );
        }

        private LogEditorViewModel CreateLogEditorViewModel(IServiceProvider serviceProvider) {
            return new LogEditorViewModel(serviceProvider);
        }




    }
}
