using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TourPlanner.DbContexts;
using TourPlanner.DTOs;
using TourPlanner.Models;

namespace TourPlanner.Services.TourProviders {
    public class DatabaseTourProvider : ITourProvider {
        private readonly TourPlannerDbContextFactory _dbContextFactory;

        public DatabaseTourProvider(IServiceProvider serviceProvider) {
            _dbContextFactory = serviceProvider.GetRequiredService<TourPlannerDbContextFactory>();
        }

        public async Task<IEnumerable<Tour>> GetAllTours() {
            using (TourPlannerDbContext context = _dbContextFactory.CreateTourPlannerDbContext()) {
                IEnumerable<TourDTO> tourDTOs = await context.Tours.ToListAsync();

                return tourDTOs.Select(r => Tour.DtoToTour(r));
            }
        }

        public async Task<Tour> GetTour(Guid id) {
            Console.WriteLine(id);

            using (TourPlannerDbContext context = _dbContextFactory.CreateTourPlannerDbContext()) {
                var t = await context.Tours.FindAsync(id);
                

                return Tour.DtoToTour(t);
            }
        }

        
    }
}
