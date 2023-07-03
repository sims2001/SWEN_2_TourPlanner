using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using log4net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using TourPlanner.DbContexts;
using TourPlanner.Exceptions;
using TourPlanner.Logging;
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
        private readonly ILoggerWrapper _logger;

        public TourManager(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
            _tourProvider = _serviceProvider.GetRequiredService<DatabaseTourProvider>();
            _tourEditor = _serviceProvider.GetRequiredService<DatabaseTourEditor>();
            _logEditor = _serviceProvider.GetRequiredService<DatabaseLogEditor>();
            _languageService = serviceProvider.GetRequiredService<LanguageService>();
            _logger = LoggerFactory.GetLogger(serviceProvider.GetRequiredService<IConfiguration>());
        }

        public async Task<IEnumerable<Tour>> GetAllTours() {  
            return await _tourProvider.GetAllTours();
        }

        public async Task<Tour> GetTour(Guid id) {
            _logger.Info($"Load Tour with Id: {id.ToString()}");
            return await _tourProvider.GetTour(id);
        }

        public async Task AddTour(Tour tour) {
            _logger.Info("Added New Tour");
            await _tourEditor.CreateTour(tour);
        }

        public async Task DeleteTour(Guid id) {
            _logger.Info($"Deleted Tour with Id: {id.ToString()}");
            await _tourEditor.DeleteTour(id);
        }

        public async Task UpdateTour(Tour tour) {
            _logger.Info($"Updated Tour with Id: {tour.Id.ToString()}");
            await _tourEditor.UpdateTour(tour);
        }


        public async Task AddLog(TourLog log) {
            _logger.Info($"Added New Log for Tour with ID: {log.TourId.ToString()}");
            await _logEditor.CreateLog(log);
        }

        public async Task UpdateLog(TourLog log) {
            _logger.Info($"Updated Log with Id: {log.Id.ToString()}");
            await _logEditor.UpdateLog(log);
        }

        public async Task DeleteLog(TourLog log) {
            _logger.Info($"Deleted Log with Id: {log.Id.ToString()}");
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
                _logger.Error("Error with importing tour");
            }
            return false;
        }

        public async Task<JObject> ExportTour(Guid exportId) {
            _logger.Info($"Exported Tour with ID: {exportId.ToString()}");
            return await _tourProvider.ExportTour(exportId);
        }

        public async Task<Tour> GetCompleteTour(Guid id) {
            _logger.Info($"Got Complete Tour-Data for Tour with ID: {id.ToString()}");
            return await _tourProvider.GetCompleteTour(id);
        }
    }
}
