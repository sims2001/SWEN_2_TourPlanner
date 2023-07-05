using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TourPlanner.DTOs;
using TourPlanner.Models;

namespace TourPlanner.DbContexts {
    public class TourPlannerDbContext : DbContext {

        public TourPlannerDbContext(DbContextOptions options) : base(options) { }

        public virtual DbSet<TourDTO> Tours { get; set; }
        public virtual DbSet<LogDTO> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<TourDTO>()
                .HasMany(e => e.LogDTOs)
                .WithOne(e => e.TourDTO)
                .HasForeignKey(e => e.TourId)
                .HasPrincipalKey(e => e.Id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LogDTO>().Property(e => e.Date).HasConversion(m => m.ToUniversalTime(), m => m.ToLocalTime());
        }

    }
}
