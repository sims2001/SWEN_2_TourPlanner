using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using TourPlanner.DbContexts;
using TourPlanner.Exceptions;
using TourPlanner.Services;
using TourPlanner.Services.LogEditors;
using TourPlanner.Services.TourCreators;
using TourPlanner.Services.TourProviders;

namespace TourPlanner.Models {
    public class TourManager {
         
        private readonly ITourProvider _tourProvider;
        private readonly ITourEditor _tourEditor;
        private readonly ILogEditor _logEditor;
        private readonly IServiceProvider _serviceProvider;
        private readonly LanguageService _languageService;

        public TourManager(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
            _tourProvider = _serviceProvider.GetRequiredService<DatabaseTourProvider>();
            _tourEditor = _serviceProvider.GetRequiredService<DatabaseTourEditor>();
            _logEditor = _serviceProvider.GetRequiredService<DatabaseLogEditor>();
            _languageService = serviceProvider.GetRequiredService<LanguageService>();
        }

        public async Task<IEnumerable<Tour>> GetAllTours() {  
            return await _tourProvider.GetAllTours();
        }

        public async Task<Tour> GetTour(Guid id) {
            return await _tourProvider.GetTour(id);
        }

        public async Task AddTour(Tour tour) {
            await _tourEditor.CreateTour(tour);
        }

        public async Task DeleteTour(Guid id) {
            await _tourEditor.DeleteTour(id);
        }

        public async Task UpdateTour(Tour tour) {
            await _tourEditor.UpdateTour(tour);
        }


        public async Task AddLog(TourLog log) {
            await _logEditor.CreateLog(log);
        }

        public async Task UpdateLog(TourLog log) {
            await _logEditor.UpdateLog(log);
        }

        public async Task DeleteLog(TourLog log) {
            await _logEditor.DeleteLog(log);
        }

        public async Task<bool> ImportTour(string tour) {
            try {
                await _tourEditor.ImportTour(tour);

                MessageBox.Show(_languageService.getVariable("message_success_import"), _languageService.getVariable("caption_success"), MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            } catch (InvalidImportException ex) {
                MessageBox.Show(_languageService.getVariable("message_error_importfile"),
                    _languageService.getVariable("caption_error"), MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return false;
        }

        public async Task<JObject> ExportTour(Guid exportId) {
            return await _tourProvider.ExportTour(exportId);
        }

        public async Task<Tour> GetCompleteTour(Guid id) {
            return await _tourProvider.GetCompleteTour(id);
        }
    }
}
