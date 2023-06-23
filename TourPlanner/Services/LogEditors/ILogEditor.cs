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
        Task CreateLog(TourLog log);
        Task UpdateLog(TourLog log);
        Task DeleteLog(TourLog log);
    }
}
