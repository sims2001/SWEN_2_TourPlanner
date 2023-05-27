using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TourPlanner.DbContexts {
    internal class TourPlannerDesignTimeDbContextFactory : IDesignTimeDbContextFactory<TourPlannerDbContext> {
        public TourPlannerDbContext CreateDbContext(string[] args) {
            DbContextOptions options = new DbContextOptionsBuilder().UseNpgsql("Host=localhost; Database=tourplannerdb; Username=postgres; Password=admin; Port=5432").Options;
            return new TourPlannerDbContext(options);
        }
    }
}
