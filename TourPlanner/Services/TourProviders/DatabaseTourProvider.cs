using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using TourPlanner.DbContexts;
using TourPlanner.DTOs;
using TourPlanner.Models;

namespace TourPlanner.Services.TourProviders {
    public class DatabaseTourProvider : ITourProvider {
        private readonly TourPlannerDbContextFactory _dbContextFactory;
        private readonly IMapper _mapper;

        public DatabaseTourProvider(IServiceProvider serviceProvider) {
            _dbContextFactory = serviceProvider.GetRequiredService<TourPlannerDbContextFactory>();
            _mapper = serviceProvider.GetRequiredService<IMapper>();
        }

        public async Task<IEnumerable<Tour>> GetAllTours() {
            using (TourPlannerDbContext context = _dbContextFactory.CreateTourPlannerDbContext()) {
                IEnumerable<TourDTO> tourDTOs = await context.Tours.Include(t => t.LogDTOs).ToListAsync();

                return tourDTOs.Select(r => _mapper.Map<Tour>(r));
            }
        }

        public async Task<Tour> GetTour(Guid id) {
            using (TourPlannerDbContext context = _dbContextFactory.CreateTourPlannerDbContext()) {
                var t = await context.Tours.FindAsync(id);
                

                return _mapper.Map<Tour>(t);
            }
        }

        public async Task<Tour> GetCompleteTour(Guid id) {
            using (TourPlannerDbContext context = _dbContextFactory.CreateTourPlannerDbContext()) {
                var t = await context.Tours.Include(t => t.LogDTOs).FirstOrDefaultAsync(i => i.Id == id);

                return _mapper.Map<Tour>(t);
            }
        }

        public async Task<JObject> ExportTour(Guid exportId) {
            using (TourPlannerDbContext context = _dbContextFactory.CreateTourPlannerDbContext()) {
                var t = await context.Tours.FindAsync(exportId);

                return JObject.FromObject(t);
            }
        }
    }
}
