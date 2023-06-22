using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.DTOs;
using TourPlanner.Models;

namespace TourPlanner.Services.LogEditors
{
    interface ILogEditor
    {
        Task CreateLog(TourDTO tour, TourLog log);
        Task UpdateLog(Guid tourId, TourLog log);
        Task DeleteLog(Guid tourId, TourLog log);
    }
}
