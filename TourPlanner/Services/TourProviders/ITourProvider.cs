using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.Services.TourProviders {
    public interface ITourProvider {
        Task<IEnumerable<Tour>> GetAllTours();

        Task<Tour> GetTour(Guid id);
    }
}
