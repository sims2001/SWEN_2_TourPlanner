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
    }
}
