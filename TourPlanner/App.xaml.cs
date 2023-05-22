﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TourPlanner.ViewModels;
using TourPlanner.Views;

namespace TourPlanner {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) {
            MainWindow = new MainWindow() {
                DataContext = new MainWindowViewModel()
            };

            MainWindow.Show();

            base.OnStartup(e);
        }

    }
}
