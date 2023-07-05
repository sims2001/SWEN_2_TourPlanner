using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TourPlanner.DbContexts {
    public class TourPlannerDbContextFactory {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        public TourPlannerDbContextFactory(IServiceProvider serviceProvider) {
            _configuration = serviceProvider.GetService<IConfiguration>();
            _connectionString = _configuration.GetSection("ConnectionStrings:LocalPostgreSQL").Value;
        }
        public virtual TourPlannerDbContext CreateTourPlannerDbContext() {
            DbContextOptions options = new DbContextOptionsBuilder().UseNpgsql(_connectionString).Options;
            return new TourPlannerDbContext(options);
        }
    }
}
