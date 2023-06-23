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
        public Guid TourId { get; set; }
        public Tour Tour { get; set; }

        public string FormatedTime => FormatTime(TotalTime);

        private string FormatTime(int time) {
            TimeSpan t = TimeSpan.FromSeconds(time);
            return t.ToString(@"hh\:mm\:ss");
        }
    }
}
