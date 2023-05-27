using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TourPlanner.DbContexts;
using TourPlanner.Services.TourCreators;
using TourPlanner.Services.TourProviders;

namespace TourPlanner.Models {
    public class TourManager {
        private readonly Dictionary<Guid, Tour> _allTours;

        private readonly ITourProvider _provider;
        private readonly ITourEditor _editor;
        private readonly IServiceProvider _serviceProvider;

        public TourManager(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
            _provider = _serviceProvider.GetRequiredService<DatabaseTourProvider>();
            _editor = _serviceProvider.GetRequiredService<DatabaseTourEditor>();

            _allTours = new Dictionary<Guid, Tour>();
        }

        public async Task<IEnumerable<Tour>> GetAllTours() {  
            return await _provider.GetAllTours();
        }

        public async Task<Tour> GetTour(Guid id) {
            return await _provider.GetTour(id);
        }

        public async Task AddTour(Tour tour) {
            await _editor.CreateTour(tour);
        }

        public async Task RemoveTour(Guid id) {
            await _editor.DeleteTour(id);
        }

        public void UpdateTour(Tour tour) {
            _editor.UpdateTour(tour);
        }

    }
}
