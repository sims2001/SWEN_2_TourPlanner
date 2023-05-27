using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.Services.TourCreators {
    internal interface ITourEditor {
        Task CreateTour(Tour tour);
        Task UpdateTour(Tour tour);
        Task DeleteTour(Guid id);
    }
}
