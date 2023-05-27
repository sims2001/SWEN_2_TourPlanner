using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TourPlanner.DbContexts {
    public class TourPlannerDbContextFactory {
        private readonly string _connectionString;
        public TourPlannerDbContextFactory(string connectionString) {
            _connectionString = connectionString;
        }
        public TourPlannerDbContext CreateTourPlannerDbContext() {
            DbContextOptions options = new DbContextOptionsBuilder().UseNpgsql(_connectionString).Options;
            return new TourPlannerDbContext(options);
        }
    }
}
