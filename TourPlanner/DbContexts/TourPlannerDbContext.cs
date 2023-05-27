using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TourPlanner.DTOs;

namespace TourPlanner.DbContexts {
    public class TourPlannerDbContext : DbContext {

        public TourPlannerDbContext(DbContextOptions options) : base(options) { }

        public DbSet<TourDTO> Tours { get; set; }

    }
}
