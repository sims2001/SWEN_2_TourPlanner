using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.DTOs {
    public class TourLogDTO {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; } = string.Empty;
        public Difficulty Difficulty { get; set; }
        public int TotalTime { get; set; }
        public Popularity Rating { get; set; }

        public Guid TourId { get; set; }
        public virtual TourDTO Tour { get; set; }
    }
}
