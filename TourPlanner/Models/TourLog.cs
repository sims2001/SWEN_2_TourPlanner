using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.DTOs;

namespace TourPlanner.Models {
    public class TourLog {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; } = string.Empty;
        public Difficulty Difficulty { get; set; }
        public int TotalTime { get; set; }
        public Popularity Rating { get; set; }

        public static TourLogDTO createLogDTO(TourLog log, TourDTO tour) {
            return new TourLogDTO() {
                Id = log.Id,
                Date = log.Date,
                Comment = log.Comment,
                Difficulty = log.Difficulty,
                TotalTime = log.TotalTime,
                Rating = log.Rating,
                TourId = tour.Id,
                Tour = tour
            };
        }

        public static TourLog LogFromDTO(TourLogDTO log) {
            return new TourLog() {
                Id = log.Id,
                Date = log.Date,
                Comment = log.Comment,
                Difficulty = log.Difficulty,
                TotalTime = log.TotalTime,
                Rating = log.Rating
            };
        }
    }
}
