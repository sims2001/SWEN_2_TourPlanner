using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.DbContexts;
using TourPlanner.Models;
using TourPlanner.Services.TourProviders;

namespace TourPlanner.Services.LogProviders
{
    class DatabaseLogProvider : ILogProvider
    {
        private readonly TourPlannerDbContextFactory _dbContextFactory;

        public DatabaseLogProvider(IServiceProvider serviceProvider) {
            _dbContextFactory = serviceProvider.GetRequiredService<TourPlannerDbContextFactory>();
        }

        public async Task<IEnumerable<TourLog>?> GetTourLogs(Guid tourId) {
            using (TourPlannerDbContext context = _dbContextFactory.CreateTourPlannerDbContext()) {
                //IEnumerable<TourDTO> tourDTOs = await context.Tours.ToListAsync();
                var t = await context.Tours.FindAsync(tourId);

                return t?.Logs;
            }
        }
    }
}
