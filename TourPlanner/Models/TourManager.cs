using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TourPlanner.DbContexts;
using TourPlanner.Services.LogEditors;
using TourPlanner.Services.LogProviders;
using TourPlanner.Services.TourCreators;
using TourPlanner.Services.TourProviders;

namespace TourPlanner.Models {
    public class TourManager {

        private readonly ITourProvider _tourProvider;
        private readonly ILogProvider _logProvider;
        private readonly ITourEditor _tourEditor;
        private readonly ILogEditor _logEditor;
        private readonly IServiceProvider _serviceProvider;

        public TourManager(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
            _tourProvider = _serviceProvider.GetRequiredService<DatabaseTourProvider>();
            _tourEditor = _serviceProvider.GetRequiredService<DatabaseTourEditor>();
            _logProvider = _serviceProvider.GetRequiredService<DatabaseLogProvider>();
            _logEditor = _serviceProvider.GetRequiredService<DatabaseLogEditor>();
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


        public async Task AddLog(Guid tourId, TourLog log) {
            var tour = await _tourProvider.GetTour(tourId);
            var tourDTO = Tour.createTourDto(tour);
            await _logEditor.CreateLog(tourDTO, log);
        }
    }
}
