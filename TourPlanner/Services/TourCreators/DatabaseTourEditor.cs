using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TourPlanner.DbContexts;
using TourPlanner.DTOs;
using TourPlanner.Models;

namespace TourPlanner.Services.TourCreators {
    public class DatabaseTourEditor : ITourEditor {
        private readonly TourPlannerDbContextFactory _contextFactory;

        public DatabaseTourEditor(IServiceProvider serviceProvider) {
            _contextFactory = serviceProvider.GetService<TourPlannerDbContextFactory>();
        }

        public async Task CreateTour(Tour tour) {
            using (TourPlannerDbContext context = _contextFactory.CreateTourPlannerDbContext()) {
                TourDTO tourDTO = createTourDto(tour);

                context.Tours.Add(tourDTO);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateTour(Tour tour) {
            using (TourPlannerDbContext context = _contextFactory.CreateTourPlannerDbContext()) {

                context.Tours.Update( createTourDto(tour) );

                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteTour(Guid id) {
            using (TourPlannerDbContext context = _contextFactory.CreateTourPlannerDbContext()) {

                TourDTO tourDTO = await context.Tours.FindAsync(id);
                context.Tours.Remove(tourDTO);
                await context.SaveChangesAsync();
            }
        }

        private static TourDTO createTourDto(Tour tour) {
            return new TourDTO() {
                Id = tour.Id,
                Name = tour.Name,
                Description = tour.Description,
                From = tour.From,
                To = tour.To,
                TransportType = tour.TransportType,
                Distance = tour.Distance,
                Time = tour.Time,
                PicturePath = tour.PicturePath,
                Logs = tour.TourLogs,
            };
        }
    }
}
