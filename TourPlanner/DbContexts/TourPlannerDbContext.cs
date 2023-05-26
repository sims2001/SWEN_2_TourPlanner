using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.DTOs;

namespace TourPlanner.DbContexts {
    internal class TourPlannerDbContext : DbContext {
        public TourPlannerDbContext(DbContextOptions<TourPlannerDbContext> options) : base(options) {

        }

        public DbSet<TourDTO> Tours { get; set; }

    }
}
