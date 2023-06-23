using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using TourPlanner.DbContexts;
using TourPlanner.DTOs;
using TourPlanner.Models;

namespace TourPlanner.Services.LogEditors
{
    class DatabaseLogEditor : ILogEditor
    {
        private readonly TourPlannerDbContextFactory _dbContextFactory;
        private readonly IMapper _mapper;

        public DatabaseLogEditor(IServiceProvider serviceProvider) {
            _dbContextFactory = serviceProvider.GetRequiredService<TourPlannerDbContextFactory>();
            _mapper = serviceProvider.GetRequiredService<IMapper>();
        }

        public async Task CreateLog(TourLog log) {
            using (TourPlannerDbContext context = _dbContextFactory.CreateTourPlannerDbContext()) {

                var t = await context.Tours.FindAsync(log.TourId);
                
                var logDTO = _mapper.Map<LogDTO>(log);

                logDTO.TourDTO = t;
                
                context.Logs.Add( logDTO );
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateLog(TourLog log) {
            using (TourPlannerDbContext context = _dbContextFactory.CreateTourPlannerDbContext()) {

                context.Logs.Update( _mapper.Map<LogDTO>(log) );

                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteLog(TourLog log) {
            using (TourPlannerDbContext context = _dbContextFactory.CreateTourPlannerDbContext()) {
                
                context.Logs.Remove( _mapper.Map<LogDTO>(log) );

                await context.SaveChangesAsync();
            }
        }
    }
}
