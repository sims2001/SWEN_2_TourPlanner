using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TourPlanner.DbContexts;
using TourPlanner.DTOs;
using TourPlanner.Models;

namespace TourPlanner.Services.LogEditors
{
    class DatabaseLogEditor : ILogEditor
    {
        private readonly TourPlannerDbContextFactory _dbContextFactory;

        public DatabaseLogEditor(IServiceProvider serviceProvider) {
            _dbContextFactory = serviceProvider.GetRequiredService<TourPlannerDbContextFactory>();
        }

        public async Task CreateLog(Guid tourId, TourLog log) {
            using (TourPlannerDbContext context = _dbContextFactory.CreateTourPlannerDbContext()) {

                var toUpdate = await context.Tours.FindAsync(tourId);
                toUpdate.Logs.Append(log);

                await context.SaveChangesAsync();
            }
        }

        public Task UpdateLog(Guid tourId, TourLog log) {
            throw new NotImplementedException();
        }

        public Task DeleteLog(Guid tourId, TourLog log) {
            throw new NotImplementedException();
        }
    }
}
