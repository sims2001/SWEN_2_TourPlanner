using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TourPlanner.DbContexts;
using TourPlanner.DTOs;
using TourPlanner.Exceptions;
using TourPlanner.Models;

namespace TourPlanner.Services.TourCreators {
    public class DatabaseTourEditor : ITourEditor {
        private readonly TourPlannerDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public DatabaseTourEditor(IServiceProvider serviceProvider) {
            _contextFactory = serviceProvider.GetService<TourPlannerDbContextFactory>();
            _mapper = serviceProvider.GetRequiredService<IMapper>();
        }

        public async Task CreateTour(Tour tour) {
            using (TourPlannerDbContext context = _contextFactory.CreateTourPlannerDbContext()) {
                TourDTO tourDTO = _mapper.Map<TourDTO>(tour);

                context.Tours.Add(tourDTO);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateTour(Tour tour) {
            using (TourPlannerDbContext context = _contextFactory.CreateTourPlannerDbContext()) {

                context.Tours.Update(_mapper.Map<TourDTO>(tour));

                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteTour(Guid id) {
            using (TourPlannerDbContext context = _contextFactory.CreateTourPlannerDbContext()) {

                TourDTO tourDTO = await context.Tours.FindAsync(id);
                context.Tours.Remove(tourDTO);
                await context.SaveChangesAsync();
            }
        }

        public async Task ImportTour(string tour) {
            using (TourPlannerDbContext context = _contextFactory.CreateTourPlannerDbContext()) {
                TourDTO newTour = JsonConvert.DeserializeObject<TourDTO>(tour);


                //Check if All Variables are there
                PropertyInfo[] properties = typeof(TourDTO).GetProperties();
                foreach (PropertyInfo propertyInfo in properties) {
                    if(propertyInfo.Name == "LogDTOs")
                        continue;
                    
                    var p = propertyInfo.GetValue(newTour);
                    if (p == null)
                        throw new InvalidImportException();

                }

                
                context.Tours.Add(newTour);

                await context.SaveChangesAsync();
            }
        }
    }
}
